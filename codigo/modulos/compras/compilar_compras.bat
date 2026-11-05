@echo off
setlocal DisableDelayedExpansion
rem La expansion retardada queda desactivada para conservar rutas con !.
set "CC_BASE=%~dp0"
set "CC_SELF=%~f0"
set "CC_CONFIG=%~1"
set "CC_MORE=%2"
pushd "%~dp0"
if errorlevel 1 exit /b 1
:CC_BUILD
rem PowerShell forma parte de Windows; el archivo contiene su propio coordinador.
"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoLogo -NoProfile -ExecutionPolicy Bypass -Command "$s=[IO.File]::ReadAllText($env:CC_SELF,[Text.Encoding]::UTF8);$p=[regex]::Split($s,'(?m)^#--COMPRAS-POWERSHELL--\r?$');& ([scriptblock]::Create($p[1]))"
set "CC_EXIT=%errorlevel%"
if "%CC_EXIT%"=="10" goto CC_BUILD
popd
endlocal & exit /b %CC_EXIT%

#--COMPRAS-POWERSHELL--
$ErrorActionPreference = 'Stop'
$base = [IO.Path]::GetFullPath($env:CC_BASE)
$steps = New-Object 'System.Collections.Generic.List[object]'
$projects = @{}
$referenceOverrides = New-Object 'System.Collections.Generic.List[object]'
$referenceRemovals = New-Object 'System.Collections.Generic.List[object]'
$securityExecutable = $null
$comprasExecutable = $null
$result = 1
$current = $null
$shortDrive = $null
$runDirectory = $null
$timer = [Diagnostics.Stopwatch]::StartNew()
$workers = [Math]::Max(1, [Environment]::ProcessorCount)

function Write-Status([string]$message, [string]$kind = 'Info') {
    $colors = @{ Info='Cyan'; Build='Yellow'; Success='Green'; Error='Red'; Warning='DarkYellow'; Menu='White' }
    $labels = @{ Info='INFO'; Build='COMPILANDO'; Success='CORRECTO'; Error='ERROR'; Warning='AVISO'; Menu='MENU' }
    Write-Host ("[{0}] {1}" -f $labels[$kind], $message) -ForegroundColor $colors[$kind]
}

function FullPath($folder, $relative) {
    [IO.Path]::GetFullPath([IO.Path]::Combine($folder, $relative))
}

# No se siguen enlaces ni se buscan soluciones en salidas, paquetes o respaldos.
function Find-Files($root, [string[]]$names) {
    $queue = New-Object 'System.Collections.Generic.Queue[string]'
    $queue.Enqueue($root)
    while ($queue.Count) {
        $dir = $queue.Dequeue()
        foreach ($entry in Get-ChildItem -LiteralPath $dir -Force) {
            if ($entry.Attributes -band [IO.FileAttributes]::ReparsePoint) { continue }
            if ($entry.PSIsContainer) {
                if ($entry.Name -match '^(\.git|\.vs|\.svn|\.hg|bin|obj|logs|packages|node_modules|TestResults|temp|tmp)$' -or
                    $entry.Name -match '(?i)(^|[ ._-])(backups?|respaldos?|archives?|copias?)([ ._-]|$)') { continue }
                $queue.Enqueue($entry.FullName)
            } elseif ($names -contains $entry.Name -or ($names -contains '*.sln' -and $entry.Extension -ieq '.sln')) { $entry.FullName }
        }
    }
}

function Unique-Path($name, [object[]]$paths, [bool]$optional = $false) {
    $matches = @($paths | Where-Object { [IO.Path]::GetFileName($_) -ieq $name })
    if (!$matches.Count -and $optional) { return $null }
    if (!$matches.Count) { throw "No se encontro la dependencia obligatoria: $name" }
    if ($matches.Count -gt 1) { throw "Varias coincidencias para ${name}:`n$($matches -join "`n")" }
    $matches[0]
}

function Add-Step($path) {
    $step = [pscustomobject]@{ Path=$path; Name=[IO.Path]::GetFileName($path); Platform=$null;
        Projects=@(); Ordered=@(); State='No ejecutada'; Code=$null; Packages=$false }
    $steps.Add($step)
    $step
}

function Write-IfChanged([string]$path, [string]$content) {
    if (!(Test-Path -LiteralPath $path) -or [IO.File]::ReadAllText($path) -cne $content) {
        [IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
    }
}

function Invoke-ProjectQueries {
    $queryTargets = Join-Path $cacheDirectory 'Consulta.targets'
    $queryXml = @'
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Target Name="ConsultarCompras">
    <ItemGroup>
      <_ComprasDato Include="P|TargetPath|$(TargetPath)" />
      <_ComprasDato Include="P|PlatformTarget|$(PlatformTarget)" />
      <_ComprasDato Include="P|TargetFrameworkVersion|$(TargetFrameworkVersion)" />
      <_ComprasDato Include="P|OutputType|$(OutputType)" />
      <_ComprasDato Include="P|Prefer32Bit|$(Prefer32Bit)" />
      <_ComprasDato Include="@(Reference->'R|%(Identity)|%(HintPath)')" />
      <_ComprasDato Include="@(ProjectReference->'J|%(Identity)|')" />
      <_ComprasDato Include="@(PackageReference->'N|%(Identity)|')" />
    </ItemGroup>
    <WriteLinesToFile File="$(ComprasArchivoConsulta)" Lines="@(_ComprasDato)" Overwrite="true" Encoding="Unicode" />
  </Target>
</Project>
'@
    Write-IfChanged $queryTargets $queryXml
    $runner = Join-Path $runDirectory 'Consultas.proj'
    $body = '<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003"><ItemGroup>'
    $index = 0
    foreach ($project in $projects.Values | Where-Object { !$_.IsSdk } | Sort-Object Path) {
        $project.QueryFile = Join-Path $runDirectory ("consulta_$index.txt")
        $properties = "Configuration=$($project.Configuration);Platform=$($project.Platform);ComprasArchivoConsulta=$(MsbuildEscape $project.QueryFile)"
        $body += '<Consulta Include="' + (XmlEscape (MsbuildEscape $project.Path)) + '"><AdditionalProperties>' + (XmlEscape $properties) + '</AdditionalProperties></Consulta>'
        $index++
    }
    $body += '</ItemGroup><Target Name="Consultar"><MSBuild Projects="@(Consulta)" Targets="ConsultarCompras" BuildInParallel="true" StopOnFirstFailure="true" /></Target></Project>'
    [IO.File]::WriteAllText($runner, $body, [Text.UTF8Encoding]::new($false))
    & $msbuild $runner /t:Consultar /nologo /verbosity:minimal "/m:$workers" /nr:false "/p:CustomAfterMicrosoftCommonTargets=$queryTargets"
    if ($LASTEXITCODE -ne 0) { throw 'No se pudieron evaluar los proyectos.' }
    # El SDK moderno usa su propio MSBuild, independiente de la version de VS.
    foreach ($project in $projects.Values | Where-Object IsSdk | Sort-Object Path) {
        $project.QueryFile = Join-Path $runDirectory ("consulta_sdk_$index.txt")
        pushd (Split-Path $project.Path)
        try {
            & $dotnet msbuild $project.Path /t:ConsultarCompras /nologo /verbosity:minimal /nr:false "/p:Configuration=$($project.Configuration)" "/p:Platform=$($project.Platform)" "/p:ComprasArchivoConsulta=$($project.QueryFile)" "/p:CustomAfterMicrosoftCommonTargets=$queryTargets"
            if ($LASTEXITCODE -ne 0) { throw "No se pudo evaluar el proyecto SDK: $($project.Path). Compruebe dotnet --list-sdks y global.json." }
        } finally { popd }
        $index++
    }
    foreach ($project in $projects.Values) {
        $properties = @{}
        $references = @()
        $projectReferences = @()
        $packages = @()
        foreach ($line in [IO.File]::ReadAllLines($project.QueryFile, [Text.Encoding]::Unicode)) {
            $parts = $line.Split([char[]]@('|'), 3, [StringSplitOptions]::None)
            if ($parts.Count -ne 3) { throw "Consulta no valida: $($project.Path): $line" }
            switch ($parts[0]) {
                'P' { $properties[$parts[1]] = $parts[2] }
                'R' { $references += [pscustomobject]@{ Identity=$parts[1]; HintPath=$parts[2] } }
                'J' { $projectReferences += [pscustomobject]@{ Identity=$parts[1] } }
                'N' { $packages += [pscustomobject]@{ Identity=$parts[1] } }
                default { throw "Consulta desconocida: $line" }
            }
        }
        $project.Data = [pscustomobject]@{ Properties=[pscustomobject]$properties; Items=[pscustomobject]@{
            Reference=$references; ProjectReference=$projectReferences; PackageReference=$packages } }
        $project.Output = $properties.TargetPath
        # Se admiten tanto .NET Framework como proyectos SDK (.NET).
        if ($packages.Count -and !$project.IsSdk) { $project.Step.Packages = $true }
    }
}

function Add-Project($path, $configuration, $platform, $step) {
    $path = [IO.Path]::GetFullPath($path)
    if ($projects.ContainsKey($path)) {
        # Seguridad comparte modelo/controlador con el paso previo que necesita Navegador.
        # Reutilizar su productor; la solucion completa compila los proyectos restantes.
        if ($projects[$path].Configuration -ne $configuration) {
            throw "Proyecto incluido con configuraciones incompatibles: $path"
        }
        # Las soluciones pueden mapear el mismo proyecto a distintas plataformas.
        # Se conserva una sola instancia para evitar escrituras simultaneas.
        if (!($step.Projects -contains $projects[$path])) { $step.Projects += $projects[$path] }
        return $projects[$path]
    }
    if (!(Test-Path -LiteralPath $path -PathType Leaf)) { throw "Falta el proyecto: $path" }
    $project = [pscustomobject]@{ Path=$path; Configuration=$configuration; Platform=$platform; Step=$step;
        IsSdk=$false; Data=$null; Output=$null; Dependencies=@(); QueryFile=$null; Marker=$null; Wave=-1; Visiting=$false; Visited=$false }
    $projects[$path] = $project
    $step.Projects += $project
    $packages = Join-Path (Split-Path $path) 'packages.config'
    [xml]$projectDefinition = [IO.File]::ReadAllText($path)
    $project.IsSdk = $projectDefinition.DocumentElement.HasAttribute('Sdk')
    if (Test-Path -LiteralPath $packages) { $step.Packages=$true }
    $project
}

function Get-ProjectWave($project) {
    if ($project.Wave -ge 0) { return $project.Wave }
    if ($project.Visiting) { throw "Dependencia circular: $($project.Path)" }
    $project.Visiting = $true
    $wave = 0
    foreach ($dependency in $project.Dependencies) { $wave = [Math]::Max($wave, (Get-ProjectWave $dependency) + 1) }
    $project.Visiting = $false
    $project.Wave = $wave
    $wave
}

function SdkCommand($project, [string]$target, [string]$referenceTargets = '') {
    $arguments = @($dotnet, 'msbuild', $project.Path, ('/t:' + $target), '/nologo', '/verbosity:minimal', '/nr:false',
        ('/p:Configuration=' + $project.Configuration), ('/p:Platform=' + $project.Platform))
    if ($target -eq 'Build') {
        $arguments += @('/p:BuildProjectReferences=false', ('/p:ComprasEstadoProyecto=' + $project.Marker),
            ('/p:CustomAfterMicrosoftCommonTargets=' + $referenceTargets))
    }
    ($arguments | ForEach-Object { '"' + $_ + '"' }) -join ' '
}

function XmlEscape([string]$text) { [Security.SecurityElement]::Escape($text) }
function MsbuildEscape([string]$text) {
    foreach ($char in @('%','$','@',';','?','*',"'",'(',')')) {
        $text = $text.Replace($char,('%{0:X2}' -f [int][char]$char))
    }
    $text
}

try {
    # NuGet packages.config todavia puede fallar con MAX_PATH. SUBST solo crea
    # un alias temporal: no mueve archivos ni cambia la configuracion del equipo.
    # Incluir tambien ayuda y los demas recursos hermanos de codigo.
    $repositoryRoot = FullPath $base '..\..\..'
    $subst = Join-Path $env:SystemRoot 'System32\subst.exe'
    foreach ($letter in 90..68) {
        $drive = ([char]$letter).ToString() + ':'
        if (Test-Path -LiteralPath ($drive + '\')) { continue }
        & $subst $drive $repositoryRoot 2>$null
        if ($LASTEXITCODE -eq 0) { $shortDrive=$drive; break }
    }
    if (!$shortDrive) { throw 'No se pudo crear una unidad temporal libre (D: a Z:) para evitar rutas largas de NuGet.' }
    $base = FullPath ($shortDrive + '\') 'codigo\modulos\compras'
    $hash = [Security.Cryptography.SHA256]::Create()
    try { $cacheKey = ([BitConverter]::ToString($hash.ComputeHash([Text.Encoding]::UTF8.GetBytes($repositoryRoot.ToUpperInvariant())))).Replace('-', '').Substring(0, 16) }
    finally { $hash.Dispose() }
    $cacheDirectory = Join-Path ([IO.Path]::GetTempPath()) ("ComprasBuild_$cacheKey")
    $null = [IO.Directory]::CreateDirectory($cacheDirectory)
    $runDirectory = Join-Path $cacheDirectory ([guid]::NewGuid().ToString('N'))
    $null = [IO.Directory]::CreateDirectory($runDirectory)
    Write-Status "Ruta temporal de compilacion: $base (los archivos permanecen en su ubicacion original)."
    $configuration = $env:CC_CONFIG
    if (!$configuration) { $configuration='Debug' }
    if ($env:CC_MORE -or $configuration -notin @('Debug','Release')) {
        throw 'Uso: compilar_compras.bat [Debug|Release]. Se permite un solo argumento.'
    }
    if ($configuration -ieq 'Debug') { $configuration='Debug' } else { $configuration='Release' }

    # Descubrir todo el codigo de produccion: componentes y todos los modulos.
    # El grafo de referencias determina el orden, no los nombres de las carpetas.
    $codeRoot = FullPath $base '..\..'
    $solutions = @(Find-Files $codeRoot @('*.sln') | Sort-Object)
    if (!$solutions.Count) { throw "No se encontraron soluciones en $codeRoot" }
    foreach ($solution in $solutions) { $null = Add-Step $solution }
    Write-Status "Descubiertas $($steps.Count) soluciones en componentes y modulos."

    # VS 2022, incluidas Build Tools; PATH es un segundo mecanismo, no dotnet build.
    $candidates = @()
    $whereTools = @(@(${env:ProgramFiles(x86)},$env:ProgramFiles) | Where-Object { $_ } | ForEach-Object {
        Join-Path $_ 'Microsoft Visual Studio\Installer\vswhere.exe'
    })
    $whereTools += @(Get-Command vswhere.exe -ErrorAction SilentlyContinue | ForEach-Object { $_.Source })
    foreach ($where in $whereTools | Select-Object -Unique) {
        if (Test-Path -LiteralPath $where) {
            $candidates += @(& $where -latest -products '*' -version '[17.8,18.0)' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe')
        }
    }
    $candidates += @(Get-Command MSBuild.exe -CommandType Application -All -ErrorAction SilentlyContinue | ForEach-Object { $_.Source })
    $msbuild = $null
    foreach ($candidate in $candidates | Select-Object -Unique) {
        if (!(Test-Path -LiteralPath $candidate -PathType Leaf)) { continue }
        $versionOutput = & $candidate /nologo /version 2>&1
        $toolCode = $LASTEXITCODE
        $versionMatch = [regex]::Match(($versionOutput -join "`n"),'(?m)^17\.\d+(?:\.\d+){1,2}\s*$')
        # Las consultas sin compilar /getProperty y /getItem requieren 17.8 o posterior.
        if ($toolCode -eq 0 -and $versionMatch.Success -and [version]$versionMatch.Value.Trim() -ge [version]'17.8') {
            $msbuild=$candidate; break
        }
    }
    if (!$msbuild) { throw 'Se requiere MSBuild.exe de Visual Studio 2022/Build Tools 17.8 o posterior, con herramientas .NET Framework 4.8.' }
    $targetPacks = @(@(${env:ProgramFiles(x86)},$env:ProgramFiles) | Where-Object { $_ } | ForEach-Object {
        Join-Path $_ 'Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\mscorlib.dll'
    } | Where-Object { Test-Path -LiteralPath $_ -PathType Leaf })
    if (!$targetPacks.Count) { throw 'Falta el Developer/Targeting Pack de .NET Framework 4.8.' }
    Write-Status "MSBuild: $msbuild"
    Write-Status "Configuracion: $configuration. Validando proyectos y dependencias antes de compilar..."

    foreach ($step in $steps) {
        $current = $step
        Write-Status "Validando: $($step.Name)"
        if ([IO.Path]::GetExtension($step.Path) -eq '.csproj') {
            [xml]$xml = Get-Content -LiteralPath $step.Path -Raw
            $platforms = @($xml.SelectNodes('//*[local-name()="PropertyGroup"]/@Condition') | ForEach-Object {
                if ($_.Value -match "==\s*'$configuration\|([^']+)'") { $Matches[1] }
            })
            $step.Platform = @('x64','AnyCPU','x86' | Where-Object { $_ -in $platforms } | Select-Object -First 1)[0]
            if (!$step.Platform) { throw "Configuracion no admitida: $($step.Path)" }
            $null = Add-Project $step.Path $configuration $step.Platform $step
            # Este productor tiene un ProjectReference real al modelo.
            for ($i=0; $i -lt $step.Projects.Count; $i++) {
                [xml]$projectXml = [IO.File]::ReadAllText($step.Projects[$i].Path)
                foreach ($reference in @($projectXml.SelectNodes('//*[local-name()="ProjectReference"]'))) {
                    $null = Add-Project (FullPath (Split-Path $step.Projects[$i].Path) $reference.Include) $configuration $step.Platform $step
                }
            }
        } else {
            $sln = [IO.File]::ReadAllText($step.Path)
            $section = [regex]::Match($sln,'(?s)GlobalSection\(SolutionConfigurationPlatforms\).*?EndGlobalSection').Value
            $platforms = @([regex]::Matches($section,"(?m)^\s*$configuration\|([^=\r\n]+)\s*=") | ForEach-Object { $_.Groups[1].Value.Trim() })
            $step.Platform = @('x64','Any CPU','x86' | Where-Object { $_ -in $platforms } | Select-Object -First 1)[0]
            if (!$step.Platform) { throw "No hay plataforma compatible con $configuration en $($step.Path)" }
            foreach ($match in [regex]::Matches($sln,'(?m)^Project\("[^"\r\n]+"\) = "[^"]+", "([^"]+\.csproj)", "(\{[^}]+\})"')) {
                $path = FullPath (Split-Path $step.Path) $match.Groups[1].Value
                $key = [regex]::Escape($match.Groups[2].Value + ".$configuration|" + $step.Platform)
                if ($sln -notmatch "(?m)^\s*$key\.Build\.0\s*=") { throw "Proyecto excluido de compilacion: $path" }
                $mapping = [regex]::Match($sln,"(?m)^\s*$key\.ActiveCfg\s*=\s*([^|\r\n]+)\|([^\r\n]+)")
                if (!$mapping.Success) { throw "Falta el mapeo de plataforma: $path" }
                $null = Add-Project $path $mapping.Groups[1].Value.Trim() $mapping.Groups[2].Value.Trim().Replace('Any CPU','AnyCPU') $step
            }
            # Una solucion puede contener solamente proyectos ya registrados.
            if ($sln -notmatch '\.csproj"') { throw "Solucion sin proyectos C# admitidos: $($step.Path)" }
        }
    }

    $dotnet = $null
    if (@($projects.Values | Where-Object IsSdk).Count) {
        # Preferir x64: el host x86 puede existir sin tener ningun SDK instalado.
        $dotnetCandidates = @()
        foreach ($root in @($env:ProgramW6432, $env:ProgramFiles, $env:DOTNET_ROOT) | Where-Object { $_ } | Select-Object -Unique) {
            $dotnetCandidates += Join-Path $root 'dotnet.exe'
            $dotnetCandidates += Join-Path $root 'dotnet\dotnet.exe'
        }
        $dotnetCandidates += @(Get-Command dotnet.exe -All -ErrorAction SilentlyContinue | ForEach-Object Source)
        foreach ($candidate in $dotnetCandidates | Select-Object -Unique) {
            if (!(Test-Path -LiteralPath $candidate -PathType Leaf)) { continue }
            $installed = @(& $candidate --list-sdks)
            if ($LASTEXITCODE -eq 0 -and @($installed | Where-Object { $_ -match '^([89]|[1-9][0-9]+)\.' }).Count) { $dotnet=$candidate; break }
        }
        if (!$dotnet) { throw 'Blazor requiere un SDK .NET 8 o posterior. Instale el SDK x64: https://dotnet.microsoft.com/download/dotnet/8.0 (el runtime solo no basta).' }
        $env:DOTNET_ROOT = Split-Path $dotnet
        $env:PATH = (Split-Path $dotnet) + ';' + $env:PATH
        Write-Status "Proyectos SDK con dotnet: $dotnet"
    }

    Write-Status "Evaluando $($projects.Count) proyectos en paralelo con $workers procesos..."
    Invoke-ProjectQueries
    $outputs = @{}
    $assemblies = @{}
    foreach ($project in $projects.Values) {
        if (!$project.Output) { throw "Salida no definida: $($project.Path)" }
        if ($outputs.ContainsKey($project.Output)) { throw "Dos productores para $($project.Output)" }
        $outputs[$project.Output] = $project
        $name = [IO.Path]::GetFileName($project.Output)
        if (!$assemblies.ContainsKey($name)) { $assemblies[$name] = @() }
        $assemblies[$name] += $project
    }
    $referenceTargets = Join-Path $cacheDirectory ('References_' + $configuration + '.targets')
    foreach ($project in $projects.Values) {
        $current=$project.Step
        foreach ($reference in @($project.Data.Items.Reference)) {
            # El ejecutor del Navegador solo compila FrmPrincipal y usa el
            # Navegador actual. Estas dos DLL de Consultas ya no existen y
            # ninguno de los archivos Compile del ejecutor las utiliza.
            if ($project.Path -match '[\\/]componentes[\\/]navegador[\\/]Ejecucion_Navegador[\\/]Ejecucion_Navegador[\\/]Ejecucion_Navegador\.csproj$' -and
                ($reference.Identity -split ',')[0].Trim() -in @('CapaVista_Mantenimiento','CapaVista_Componentes')) {
                $referenceRemovals.Add([pscustomobject]@{ Project=$project.Path; Identity=$reference.Identity })
                Write-Status "Referencia obsoleta retirada de Ejecucion_Navegador: $($reference.Identity)" Warning
                continue
            }
            $assembly = ($reference.Identity -split ',')[0].Trim() + '.dll'
            if (!$reference.HintPath -and !$assemblies.ContainsKey($assembly)) { continue }
            $hint = if ($reference.HintPath) { FullPath (Split-Path $project.Path) $reference.HintPath } else { $assembly }
            if ($outputs.ContainsKey($hint)) { $project.Dependencies += $outputs[$hint] }
            elseif ($hint -notmatch '[\\/]packages[\\/]') {
                $assembly = ($reference.Identity -split ',')[0].Trim() + '.dll'
                $producers = @($assemblies[$assembly] | Where-Object { $_ })
                # TipoCambio fue renombrado a TipoCambioBancos; la vista conserva
                # el nombre antiguo. Resolver solo dentro de ese mismo proceso.
                $renamedIdentity = $null
                if (!$producers.Count -and $assembly -ieq 'CapaModelo_TipoCambio.dll') {
                    $processRoot = Split-Path (Split-Path $project.Path)
                    if ([IO.Path]::GetFileName($processRoot) -ieq 'Tipo_CambioBancos') {
                        $renamedPath = FullPath $processRoot 'CapaModelo_TipoCambioBancos\CapaModelo_TipoCambioBancos.csproj'
                        if ($projects.ContainsKey($renamedPath)) {
                            $producers = @($projects[$renamedPath])
                            $renamedIdentity = [IO.Path]::GetFileNameWithoutExtension($producers[0].Output)
                        }
                    }
                }
                if (!$producers.Count -and (Test-Path -LiteralPath $hint -PathType Leaf)) { continue }
                if ($producers.Count -ne 1) { throw "Referencia sin productor unico para $configuration en $($project.Path): $hint" }
                $producer = $producers[0]
                $project.Dependencies += $producer
                $referenceOverrides.Add([pscustomobject]@{ Project=$project.Path; Identity=$reference.Identity; NewIdentity=$renamedIdentity; Output=$producer.Output })
                Write-Status "Referencia corregida: $assembly -> $($producer.Output)" Warning
            }
        }
        foreach ($reference in @($project.Data.Items.ProjectReference)) {
            $path = FullPath (Split-Path $project.Path) $reference.Identity
            if (!$projects.ContainsKey($path)) { throw "ProjectReference fuera del plan: $path" }
            $project.Dependencies += $projects[$path]
        }
        foreach ($dependency in $project.Dependencies) {
            if ($project.Data.Properties.PlatformTarget -eq 'x86' -and $dependency.Data.Properties.PlatformTarget -eq 'x64') {
                throw "Arquitecturas incompatibles: $($project.Path) (x86) -> $($dependency.Path) (x64)"
            }
        }
    }
    $referenceBody = '<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003"><Target Name="CorregirReferenciasCompras" BeforeTargets="ResolveAssemblyReferences"><ItemGroup>'
    foreach ($removal in $referenceRemovals | Sort-Object Project,Identity) {
        $condition = "'`$(MSBuildProjectFullPath)' == '" + (MsbuildEscape $removal.Project) + "'"
        $referenceBody += '<Reference Remove="' + (XmlEscape $removal.Identity) + '" Condition="' + (XmlEscape $condition) + '" />'
    }
    foreach ($override in $referenceOverrides | Sort-Object Project,Identity) {
        $condition = "'`$(MSBuildProjectFullPath)' == '" + (MsbuildEscape $override.Project) + "' And '%(Reference.Identity)' == '" + (MsbuildEscape $override.Identity) + "'"
        if ($override.NewIdentity) {
            $projectCondition = "'`$(MSBuildProjectFullPath)' == '" + (MsbuildEscape $override.Project) + "'"
            $referenceBody += '<Reference Remove="' + (XmlEscape $override.Identity) + '" Condition="' + (XmlEscape $projectCondition) + '" />'
            $referenceBody += '<Reference Include="' + (XmlEscape $override.NewIdentity) + '" Condition="' + (XmlEscape $projectCondition) + '"><HintPath>' + (XmlEscape $override.Output) + '</HintPath></Reference>'
        } else {
            $referenceBody += '<Reference Condition="' + (XmlEscape $condition) + '"><HintPath>' + (XmlEscape $override.Output) + '</HintPath></Reference>'
        }
    }
    $referenceBody += '</ItemGroup></Target><Target Name="ComprasMarcarInicio" BeforeTargets="PrepareForBuild" Condition="&apos;$(ComprasEstadoProyecto)&apos; != &apos;&apos;"><WriteLinesToFile File="$(ComprasEstadoProyecto).started" Lines="$(MSBuildProjectFullPath)" Overwrite="true" /></Target><Target Name="ComprasMarcarExito" AfterTargets="Build" Condition="&apos;$(ComprasEstadoProyecto)&apos; != &apos;&apos;"><WriteLinesToFile File="$(ComprasEstadoProyecto).done" Lines="$(MSBuildProjectFullPath)" Overwrite="true" /></Target></Project>'
    Write-IfChanged $referenceTargets $referenceBody
    $securityProject = @($projects.Values | Where-Object { [IO.Path]::GetFileName($_.Path) -ieq 'Ejecucion_Seguridad.csproj' })
    if ($securityProject.Count -eq 1) {
        $securityExecutable = $repositoryRoot + $securityProject[0].Output.Substring($shortDrive.Length)
    }
    $comprasProject = @($projects.Values | Where-Object { [IO.Path]::GetFileName($_.Path) -ieq 'Ejecucion_Compras.csproj' })
    if ($comprasProject.Count -eq 1) {
        $comprasExecutable = $repositoryRoot + $comprasProject[0].Output.Substring($shortDrive.Length)
    }
    foreach ($project in $projects.Values) { $null = Get-ProjectWave $project }
    $current = $null
    $runner = Join-Path $runDirectory 'Build.proj'
    $body = '<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003"><ItemGroup>'
    foreach ($step in $steps | Where-Object { $_.Packages -and !@($_.Projects | Where-Object IsSdk).Count }) {
        $packageRoot = Join-Path (Split-Path $step.Path) 'packages'
        if ([IO.Path]::GetExtension($step.Path) -eq '.csproj') { $packageRoot = Join-Path (Split-Path (Split-Path $step.Path)) 'packages' }
        $properties = "Configuration=$configuration;Platform=$($step.Platform);RestorePackagesConfig=true;RestoreRepositoryPath=$(MsbuildEscape $packageRoot)"
        $body += '<Restauracion Include="' + (XmlEscape (MsbuildEscape $step.Path)) + '"><AdditionalProperties>' + (XmlEscape $properties) + '</AdditionalProperties></Restauracion>'
    }
    $index = 0
    foreach ($project in $projects.Values | Sort-Object Path) {
        $project.Marker = Join-Path $runDirectory ("proyecto_$index")
        $properties = "Configuration=$($project.Configuration);Platform=$($project.Platform);BuildProjectReferences=false;ComprasEstadoProyecto=$(MsbuildEscape $project.Marker)"
        if ($project.Data.Properties.OutputType -in @('Exe','WinExe') -and $project.Data.Properties.PlatformTarget -eq 'AnyCPU') { $properties += ';Prefer32Bit=false' }
        if ($project.IsSdk) { $index++; continue }
        $body += '<Nivel' + $project.Wave + ' Include="' + (XmlEscape (MsbuildEscape $project.Path)) + '"><AdditionalProperties>' + (XmlEscape $properties) + '</AdditionalProperties></Nivel' + $project.Wave + '>'
        $index++
    }
    $body += '</ItemGroup><Target Name="Restaurar"><MSBuild Projects="@(Restauracion)" Targets="Restore" BuildInParallel="false" StopOnFirstFailure="true" /></Target>'
    $sdkRestore = ''
    foreach ($project in $projects.Values | Where-Object IsSdk | Sort-Object Path) {
        $sdkRestore += '<Exec WorkingDirectory="' + (XmlEscape (Split-Path $project.Path)) + '" Command="' + (XmlEscape (SdkCommand $project 'Restore')) + '" />'
    }
    $body += '<Target Name="RestaurarSdk">' + $sdkRestore + '</Target>'
    $targets = @('Restaurar','RestaurarSdk')
    $waves = @($projects.Values | Select-Object -ExpandProperty Wave | Sort-Object -Unique)
    foreach ($wave in $waves) {
        $targets += "Nivel$wave"
        $body += '<Target Name="Nivel' + $wave + '"><Message Importance="high" Text="[COMPILANDO] Nivel ' + ($wave + 1) + '/' + $waves.Count + '" /><MSBuild Projects="@(Nivel' + $wave + ')" Targets="Build" BuildInParallel="true" StopOnFirstFailure="true" />'
        foreach ($project in $projects.Values | Where-Object { $_.IsSdk -and $_.Wave -eq $wave } | Sort-Object Path) {
            $body += '<Exec WorkingDirectory="' + (XmlEscape (Split-Path $project.Path)) + '" Command="' + (XmlEscape (SdkCommand $project 'Build' $referenceTargets)) + '" />'
        }
        $body += '</Target>'
    }
    $body += '<Target Name="Build" DependsOnTargets="' + ($targets -join ';') + '" /></Project>'
    [IO.File]::WriteAllText($runner, $body, [Text.UTF8Encoding]::new($false))
    Write-Status "Compilando $($projects.Count) proyectos en $($waves.Count) niveles, con hasta $workers procesos en paralelo." Build
    & $msbuild $runner /t:Build /nologo /verbosity:minimal "/m:$workers" /nr:false /warnAsError:MSB3245 '/consoleloggerparameters:ForceConsoleColor;Summary' "/p:CustomAfterMicrosoftCommonTargets=$referenceTargets"
    $code = $LASTEXITCODE
    foreach ($step in $steps) {
        $complete = @($step.Projects | Where-Object { !(Test-Path -LiteralPath ($_.Marker + '.done')) })
        if (!$complete.Count) {
            foreach ($project in $step.Projects) {
                if (!(Test-Path -LiteralPath $project.Output -PathType Leaf)) { throw "Falta la salida esperada: $($project.Output)" }
            }
            $step.State = 'Correcta'
            $step.Code = 0
        } elseif (@($complete | Where-Object { Test-Path -LiteralPath ($_.Marker + '.started') }).Count) {
            $step.State = 'Fallida'
            $step.Code = $code
        }
    }
    if ($code -ne 0) {
        if (!@($steps | Where-Object State -eq 'Fallida').Count) {
            $first = $steps | Where-Object State -ne 'Correcta' | Select-Object -First 1
            if ($first) { $first.State = 'Fallida'; $first.Code = $code }
        }
        throw "La compilacion termino con codigo $code."
    }
    if (@($steps | Where-Object State -ne 'Correcta').Count) { throw 'La compilacion termino sin completar todos los proyectos.' }
    $result=0
} catch {
    if ($current -and $current.State -ne 'Correcta') { $current.State='Fallida' }
    Write-Status $_.Exception.Message Error
} finally {
    $timer.Stop()
    Write-Status ('Tiempo total: {0:N1} segundos.' -f $timer.Elapsed.TotalSeconds)
    Write-Status 'RESUMEN DE COMPILACION'
    Write-Status ('Correctas: {0}' -f @($steps | Where-Object State -eq 'Correcta').Count) Success
    Write-Status ('Fallidas: {0}' -f @($steps | Where-Object State -eq 'Fallida').Count) $(if ($result -eq 0) { 'Success' } else { 'Error' })
    Write-Status ('No ejecutadas: {0}' -f @($steps | Where-Object State -eq 'No ejecutada').Count) Warning
    foreach ($step in $steps) {
        $kind = switch ($step.State) { 'Correcta' { 'Success' } 'Fallida' { 'Error' } default { 'Warning' } }
        Write-Status ("{0}: {1}" -f $step.State, $step.Name) $kind
    }
    if ($runDirectory -and [IO.Directory]::Exists($runDirectory)) {
        try {
            foreach ($file in [IO.Directory]::GetFiles($runDirectory)) { [IO.File]::Delete($file) }
            [IO.Directory]::Delete($runDirectory)
        } catch { Write-Warning "No se pudo limpiar: $runDirectory" }
    }
    if ($shortDrive) {
        & $subst $shortDrive /D
        if ($LASTEXITCODE -ne 0) { Write-Warning "No se pudo quitar el alias temporal $shortDrive" }
    }
}
while ($true) {
    Write-Status 'Seleccione la siguiente accion' Menu
    Write-Host '1. Volver a compilar' -ForegroundColor Yellow
    Write-Host '2. Ejecutar Ejecucion_Seguridad' -ForegroundColor Green
    Write-Host '3. Ejecutar Ejecucion_Compras' -ForegroundColor Green
    Write-Host '4. Salir' -ForegroundColor Gray
    Write-Host 'Presione 1, 2, 3 o 4: ' -NoNewline -ForegroundColor Cyan
    $option = [Console]::ReadKey($true).KeyChar.ToString()
    Write-Host $option
    if ($option -eq '1') { exit 10 }
    if ($option -eq '4') { exit $result }
    if ($option -in @('2','3')) {
        if ($result -ne 0) { Write-Status 'Corrija los errores y vuelva a compilar antes de ejecutar.' Warning; continue }
        $executable = if ($option -eq '2') { $securityExecutable } else { $comprasExecutable }
        $executableName = if ($option -eq '2') { 'Ejecucion_Seguridad.exe' } else { 'Ejecucion_Compras.exe' }
        if (!$executable -or !(Test-Path -LiteralPath $executable -PathType Leaf)) {
            Write-Status "No se encontro $executableName." Error
            continue
        }
        Write-Status "Iniciando: $executable" Success
        Start-Process -FilePath $executable -WorkingDirectory (Split-Path $executable)
        exit 0
    }
    Write-Status 'Opcion no valida. Escriba 1, 2, 3 o 4.' Warning
}

