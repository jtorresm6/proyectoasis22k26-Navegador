@echo off
rem Uso: compilar_compras.bat [Debug|Release] [--no-menu]
rem Compila componentes y modulos sin crear unidades virtuales.
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
$planningErrors = New-Object 'System.Collections.Generic.List[string]'
$referenceOverrides = New-Object 'System.Collections.Generic.List[object]'
$referenceRemovals = New-Object 'System.Collections.Generic.List[object]'
$securityExecutable = $null
$comprasExecutable = $null
$result = 1
$current = $null
$runDirectory = $null
$repositoryRoot = $null
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
        foreach ($entry in Get-ChildItem -LiteralPath $dir -Force -ErrorAction SilentlyContinue) {
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
        Projects=@(); Ordered=@(); Invalid=$false; State='No ejecutada'; Code=$null; Packages=$false }
    $steps.Add($step)
    $step
}

function Write-IfChanged([string]$path, [string]$content) {
    if (!(Test-Path -LiteralPath $path) -or [IO.File]::ReadAllText($path) -cne $content) {
        [IO.File]::WriteAllText($path, $content, [Text.UTF8Encoding]::new($false))
    }
}

# Quita un proyecto del plan de compilacion (por ejemplo, si no se pudo evaluar).
function Remove-ProjectFromPlan($project) {
    foreach ($step in $steps) {
        if ($step.Projects -contains $project) { $step.Invalid = $true }
        $step.Projects = @($step.Projects | Where-Object { !([object]::ReferenceEquals($_, $project)) })
    }
    $planningErrors.Add($project.Path)
    $projects.Remove($project.Path)
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
      <_ComprasDato Include="P|NuGetPackageRoot|$(NuGetPackageRoot)" />
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
    $body += '</ItemGroup><Target Name="Consultar"><MSBuild Projects="@(Consulta)" Targets="ConsultarCompras" BuildInParallel="true" StopOnFirstFailure="false" ContinueOnError="true" /></Target></Project>'
    [IO.File]::WriteAllText($runner, $body, [Text.UTF8Encoding]::new($false))
    & $msbuild $runner /t:Consultar /nologo /verbosity:minimal "/m:$workers" /nr:false "/p:CustomAfterMicrosoftCommonTargets=$queryTargets"
    if ($LASTEXITCODE -ne 0) { Write-Status 'Algunos proyectos no se pudieron evaluar; se omitiran del plan.' Warning }
    # El SDK moderno usa su propio MSBuild, independiente de la version de VS.
    foreach ($project in $projects.Values | Where-Object IsSdk | Sort-Object Path) {
        $project.QueryFile = Join-Path $runDirectory ("consulta_sdk_$index.txt")
        pushd (Split-Path $project.Path)
        try {
            & $dotnet msbuild $project.Path /t:ConsultarCompras /nologo /verbosity:minimal /nr:false "/p:Configuration=$($project.Configuration)" "/p:Platform=$($project.Platform)" "/p:ComprasArchivoConsulta=$($project.QueryFile)" "/p:CustomAfterMicrosoftCommonTargets=$queryTargets"
            if ($LASTEXITCODE -ne 0) { Write-Status "No se pudo evaluar el proyecto SDK: $($project.Path). Compruebe dotnet --list-sdks y global.json." Warning }
        } catch {
            Write-Status "Error evaluando el proyecto SDK $($project.Path): $($_.Exception.Message)" Warning
        } finally { popd }
        $index++
    }
    foreach ($project in @($projects.Values)) {
        if (!$project.QueryFile -or !(Test-Path -LiteralPath $project.QueryFile -PathType Leaf)) {
            Write-Status "Proyecto omitido (sin datos de evaluacion): $($project.Path)" Warning
            Remove-ProjectFromPlan $project
            continue
        }
        $properties = @{}
        $references = @()
        $projectReferences = @()
        $packages = @()
        foreach ($line in [IO.File]::ReadAllLines($project.QueryFile, [Text.Encoding]::Unicode)) {
            $parts = $line.Split([char[]]@('|'), 3, [StringSplitOptions]::None)
            if ($parts.Count -ne 3) { Write-Status "Linea de consulta ignorada ($($project.Path)): $line" Warning; continue }
            switch ($parts[0]) {
                'P' { $properties[$parts[1]] = $parts[2] }
                'R' { $references += [pscustomobject]@{ Identity=$parts[1]; HintPath=$parts[2] } }
                'J' { $projectReferences += [pscustomobject]@{ Identity=$parts[1] } }
                'N' { $packages += [pscustomobject]@{ Identity=$parts[1] } }
                default { Write-Status "Consulta desconocida ignorada: $line" Warning }
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
            Write-Status "Proyecto incluido con configuraciones distintas; se conserva la primera: $path" Warning
        }
        # Las soluciones pueden mapear el mismo proyecto a distintas plataformas.
        # Se conserva una sola instancia para evitar escrituras simultaneas.
        if (!($step.Projects -contains $projects[$path])) { $step.Projects += $projects[$path] }
        return $projects[$path]
    }
    if (!(Test-Path -LiteralPath $path -PathType Leaf)) { Write-Status "Falta el proyecto (se omite): $path" Warning; $planningErrors.Add($path); $step.Invalid = $true; return $null }
    $isSdk = $false
    try {
        [xml]$projectDefinition = [IO.File]::ReadAllText($path)
        $isSdk = $projectDefinition.DocumentElement.HasAttribute('Sdk')
    } catch {
        Write-Status "No se pudo leer el proyecto (se omite): $path" Warning
        $planningErrors.Add($path)
        $step.Invalid = $true
        return $null
    }
    $project = [pscustomobject]@{ Path=$path; Configuration=$configuration; Platform=$platform; Step=$step;
        IsSdk=$isSdk; Data=$null; Output=$null; Dependencies=@(); QueryFile=$null; Marker=$null; Wave=-1; Visiting=$false; Visited=$false }
    $projects[$path] = $project
    $step.Projects += $project
    $packages = Join-Path (Split-Path $path) 'packages.config'
    if (Test-Path -LiteralPath $packages) { $step.Packages=$true }
    $project
}

function Get-ProjectWave($project, [string[]]$trail = @()) {
    if ($project.Wave -ge 0) { return $project.Wave }
    if ($project.Visiting) {
        throw ("Dependencia circular real. Separe la carga de modulos de sus referencias de compilacion:`n" + (($trail + $project.Path) -join "`n -> "))
    }
    $project.Visiting = $true
    $wave = 0
    foreach ($dependency in $project.Dependencies) {
        $wave = [Math]::Max($wave, (Get-ProjectWave $dependency ($trail + $project.Path)) + 1)
    }
    $project.Visiting = $false
    $project.Wave = $wave
    $wave
}

function SdkCommand($project, [string]$target, [string]$referenceTargets = '') {
    $arguments = @($dotnet, 'msbuild', $project.Path, ('/t:' + $target), '/nologo', '/verbosity:minimal', '/nr:false',
        ('/p:Configuration=' + $project.Configuration), ('/p:Platform=' + $project.Platform))
    if ($target -eq 'Build') {
        $arguments += @('/p:BuildProjectReferences=false', ('/p:ComprasEstadoProyecto=' + $project.Marker),
            ('/p:CustomAfterMicrosoftCommonTargets=' + $referenceTargets),
            '/p:CreateHardLinksForCopyLocalIfPossible=true',
            '/p:CreateHardLinksForCopyFilesToOutputDirectoryIfPossible=true',
            '/p:CreateHardLinksForCopyAdditionalFilesIfPossible=true')
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

# Registro de salidas reales: rutas relativas y configuracion evaluadas por MSBuild.
# Seguridad usa este indice para cargar modulos sin depender de ellos al compilar.
function Write-ModuleRegistries {
    $runtime = @($projects.Values | Where-Object { $_.Output -and (Test-Path -LiteralPath $_.Output -PathType Leaf) })
    $unique = @()
    foreach ($group in $runtime | Group-Object { [IO.Path]::GetFileNameWithoutExtension($_.Output) }) {
        $producers = @($group.Group | Sort-Object Output -Unique)
        if ($producers.Count -gt 1) {
            Write-Status "Ensamblado ambiguo, no se registra para carga dinamica: $($group.Name)" Warning
            continue
        }
        $unique += $producers[0]
    }
    $directories = @($runtime | ForEach-Object { Split-Path $_.Output } | Sort-Object -Unique)
    foreach ($directory in $directories) {
        $origin = [Uri]($directory.TrimEnd('\') + '\')
        $xml = '<?xml version="1.0" encoding="utf-8"?><modulos configuracion="' + (XmlEscape $configuration) + '">'
        foreach ($project in $unique | Sort-Object Output) {
            $relative = [Uri]::UnescapeDataString($origin.MakeRelativeUri([Uri]$project.Output).ToString()).Replace('/', '\')
            $xml += '<ensamblado nombre="' + (XmlEscape ([IO.Path]::GetFileNameWithoutExtension($project.Output))) + '" ruta="' + (XmlEscape $relative) + '" />'
        }
        $xml += '</modulos>'
        Write-IfChanged (Join-Path $directory 'Modulos.xml') $xml
    }
    Write-Status "Registro dinamico generado en $($directories.Count) carpetas de salida." Success
}

try {
    # Compilar directamente en el repositorio, sin asignar letras de unidad.
    $repositoryRoot = FullPath $base '..\..\..'
    $hash = [Security.Cryptography.SHA256]::Create()
    try { $cacheKey = ([BitConverter]::ToString($hash.ComputeHash([Text.Encoding]::UTF8.GetBytes($repositoryRoot.ToUpperInvariant())))).Replace('-', '').Substring(0, 16) }
    finally { $hash.Dispose() }
    $cacheDirectory = Join-Path ([IO.Path]::GetTempPath()) ("ComprasBuild_$cacheKey")
    $null = [IO.Directory]::CreateDirectory($cacheDirectory)
    $packageCache = Join-Path $cacheDirectory 'packages'
    $null = [IO.Directory]::CreateDirectory($packageCache)
    $runDirectory = Join-Path $cacheDirectory ([guid]::NewGuid().ToString('N'))
    $null = [IO.Directory]::CreateDirectory($runDirectory)
    Write-Status "Ruta de compilacion: $base (los archivos permanecen en su ubicacion original)."
    $configuration = $env:CC_CONFIG
    if (!$configuration) { $configuration='Debug' }
    if ($env:CC_MORE -and $env:CC_MORE -ine '--no-menu') { throw 'Uso: compilar_compras.bat [Debug|Release] [--no-menu]' }
    if ($configuration -ieq 'Release') { $configuration='Release' }
    else {
        if ($configuration -ine 'Debug') { Write-Status "Configuracion '$configuration' no reconocida; se usa Debug." Warning }
        $configuration='Debug'
    }

    # Descubrir todo el codigo de produccion: componentes y todos los modulos.
    # El grafo de referencias determina el orden, no los nombres de las carpetas.
    $codeRoot = FullPath $base '..\..'
    $solutions = @(Find-Files $codeRoot @('*.sln') | Sort-Object)
    if (!$solutions.Count) { throw "No se encontraron soluciones en $codeRoot" }
    foreach ($solution in $solutions) { $null = Add-Step $solution }
    Write-Status "Descubiertas $($steps.Count) soluciones en componentes y modulos."

    # Cualquier version de MSBuild (Visual Studio, Build Tools o PATH); se usa la version mas alta.
    $candidates = @()
    $vsRoots = @(@(${env:ProgramFiles(x86)},$env:ProgramFiles,$env:ProgramW6432) | Where-Object { $_ } | Select-Object -Unique)
    $whereTools = @($vsRoots | ForEach-Object { Join-Path $_ 'Microsoft Visual Studio\Installer\vswhere.exe' })
    $whereTools += @(Get-Command vswhere.exe -ErrorAction SilentlyContinue | ForEach-Object { $_.Source })
    foreach ($where in $whereTools | Select-Object -Unique) {
        if (Test-Path -LiteralPath $where) {
            $previousPreference = $ErrorActionPreference
            $ErrorActionPreference = 'Continue'
            try {
                $candidates += @(& $where -all -prerelease -products '*' -find 'MSBuild\**\Bin\MSBuild.exe' 2>$null)
            } finally { $ErrorActionPreference = $previousPreference }
        }
    }
    foreach ($root in $vsRoots) {
        $vsDir = Join-Path $root 'Microsoft Visual Studio'
        $candidates += @(Get-ChildItem -Path (Join-Path $vsDir '*\*\MSBuild\*\Bin\MSBuild.exe') -ErrorAction SilentlyContinue | ForEach-Object { $_.FullName })
        $candidates += @(Get-ChildItem -Path (Join-Path $vsDir '*\*\MSBuild\*\Bin\amd64\MSBuild.exe') -ErrorAction SilentlyContinue | ForEach-Object { $_.FullName })
        # MSBuild clasico de .NET Framework (versiones anteriores a Visual Studio 2017).
        $candidates += @(Get-ChildItem -Path (Join-Path $root 'MSBuild\*\Bin\MSBuild.exe') -ErrorAction SilentlyContinue | ForEach-Object { $_.FullName })
    }
    foreach ($frameworkRoot in @("$env:SystemRoot\Microsoft.NET\Framework64", "$env:SystemRoot\Microsoft.NET\Framework")) {
        $candidates += @(Get-ChildItem -Path (Join-Path $frameworkRoot 'v*\MSBuild.exe') -ErrorAction SilentlyContinue | ForEach-Object { $_.FullName })
    }
    $candidates += @(Get-Command MSBuild.exe -CommandType Application -All -ErrorAction SilentlyContinue | ForEach-Object { $_.Source })
    $msbuild = $null
    $bestVersion = [version]'0.0'
    $seen = @()
    foreach ($candidate in $candidates | Where-Object { $_ } | Select-Object -Unique) {
        if (!(Test-Path -LiteralPath $candidate -PathType Leaf)) { continue }
        $previousPreference = $ErrorActionPreference
        $ErrorActionPreference = 'Continue'
        try {
            $versionOutput = & $candidate /nologo /version 2>&1
            $toolCode = $LASTEXITCODE
        } catch { $versionOutput = ''; $toolCode = 1 }
        finally { $ErrorActionPreference = $previousPreference }
        if ($toolCode -ne 0) { $seen += "$candidate (no responde)"; continue }
        $versionMatch = [regex]::Match(($versionOutput -join "`n"),'(?m)^\d+\.\d+(?:\.\d+)?(?:\.\d+)?\s*$')
        $candidateVersion = [version]'0.0.1'
        if ($versionMatch.Success) { $candidateVersion = [version]$versionMatch.Value.Trim() }
        $seen += "$candidate ($candidateVersion)"
        if ($candidateVersion -gt $bestVersion) { $bestVersion = $candidateVersion; $msbuild = $candidate }
    }
    if (!$msbuild) {
        throw ('No se encontro ningun MSBuild.exe utilizable. Instale Visual Studio o Build Tools. Encontrados: ' + $(if ($seen.Count) { $seen -join ' | ' } else { 'ninguno' }))
    }
    if ($bestVersion.Major -lt 16) { Write-Status "MSBuild $bestVersion es anterior a VS 2019; se intentara compilar de todos modos." Warning }
    $targetPacks = @(@(${env:ProgramFiles(x86)},$env:ProgramFiles) | Where-Object { $_ } | ForEach-Object {
        Join-Path $_ 'Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\mscorlib.dll'
    } | Where-Object { Test-Path -LiteralPath $_ -PathType Leaf })
    if (!$targetPacks.Count) { Write-Status 'Falta el Developer/Targeting Pack de .NET Framework 4.8; los proyectos que lo requieran fallaran.' Warning }
    Write-Status "MSBuild: $msbuild"
    Write-Status "Configuracion: $configuration. Validando proyectos y dependencias antes de compilar..."

    foreach ($step in $steps) {
        $current = $step
        Write-Status "Validando: $($step.Name)"
        try {
            if ([IO.Path]::GetExtension($step.Path) -eq '.csproj') {
                [xml]$xml = Get-Content -LiteralPath $step.Path -Raw
                $platforms = @($xml.SelectNodes('//*[local-name()="PropertyGroup"]/@Condition') | ForEach-Object {
                    if ($_.Value -match "==\s*'$configuration\|([^']+)'") { $Matches[1] }
                })
                $step.Platform = @('x64','AnyCPU','x86' | Where-Object { $_ -in $platforms } | Select-Object -First 1)[0]
                if (!$step.Platform) {
                    Write-Status "Sin plataforma declarada para $configuration en $($step.Path); se usa AnyCPU." Warning
                    $step.Platform = 'AnyCPU'
                }
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
                if (!$step.Platform -and $platforms.Count) { $step.Platform = $platforms[0] }
                if (!$step.Platform) {
                    Write-Status "No hay plataforma declarada para $configuration en $($step.Path); se usa Any CPU." Warning
                    $step.Platform = 'Any CPU'
                }
                foreach ($match in [regex]::Matches($sln,'(?m)^Project\("[^"\r\n]+"\) = "[^"]+", "([^"]+\.csproj)", "(\{[^}]+\})"')) {
                    $path = FullPath (Split-Path $step.Path) $match.Groups[1].Value
                    $key = [regex]::Escape($match.Groups[2].Value + ".$configuration|" + $step.Platform)
                    if ($sln -notmatch "(?m)^\s*$key\.Build\.0\s*=") { Write-Status "Proyecto excluido de la solucion; se compila de todos modos: $path" Warning }
                    $mapping = [regex]::Match($sln,"(?m)^\s*$key\.ActiveCfg\s*=\s*([^|\r\n]+)\|([^\r\n]+)")
                    if ($mapping.Success) {
                        $null = Add-Project $path $mapping.Groups[1].Value.Trim() $mapping.Groups[2].Value.Trim().Replace('Any CPU','AnyCPU') $step
                    } else {
                        Write-Status "Falta el mapeo de plataforma; se usa $configuration|$($step.Platform): $path" Warning
                        $null = Add-Project $path $configuration $step.Platform.Replace('Any CPU','AnyCPU') $step
                    }
                }
                # Una solucion puede contener solamente proyectos ya registrados.
                if ($sln -notmatch '\.csproj"') { Write-Status "Solucion sin proyectos C# admitidos (se omite): $($step.Path)" Warning }
            }
        } catch {
            Write-Status "Error validando $($step.Name) (se continua con las demas): $($_.Exception.Message)" Warning
            $planningErrors.Add($step.Path)
            $step.Invalid = $true
        }
    }
    $current = $null
    if (!$projects.Count) { throw 'No se encontro ningun proyecto para compilar.' }

    $dotnet = $null
    if (@($projects.Values | Where-Object IsSdk).Count) {
        # Preferir x64: el host x86 puede existir sin tener ningun SDK instalado.
        $dotnetCandidates = @()
        foreach ($root in @($env:ProgramW6432, $env:ProgramFiles, $env:DOTNET_ROOT) | Where-Object { $_ } | Select-Object -Unique) {
            $dotnetCandidates += Join-Path $root 'dotnet.exe'
            $dotnetCandidates += Join-Path $root 'dotnet\dotnet.exe'
        }
        $dotnetCandidates += @(Get-Command dotnet.exe -All -ErrorAction SilentlyContinue | ForEach-Object Source)
        $fallbackDotnet = $null
        foreach ($candidate in $dotnetCandidates | Select-Object -Unique) {
            if (!(Test-Path -LiteralPath $candidate -PathType Leaf)) { continue }
            $previousPreference = $ErrorActionPreference
            $ErrorActionPreference = 'Continue'
            try { $installed = @(& $candidate --list-sdks 2>$null); $sdkCode = $LASTEXITCODE }
            catch { $installed = @(); $sdkCode = 1 }
            finally { $ErrorActionPreference = $previousPreference }
            if ($sdkCode -ne 0 -or !$installed.Count) { continue }
            if (@($installed | Where-Object { $_ -match '^([89]|[1-9][0-9]+)\.' }).Count) { $dotnet=$candidate; break }
            if (!$fallbackDotnet) { $fallbackDotnet = $candidate }
        }
        if (!$dotnet -and $fallbackDotnet) {
            $dotnet = $fallbackDotnet
            Write-Status 'Solo hay un SDK .NET anterior al 8; los proyectos Blazor podrian fallar.' Warning
        }
        if (!$dotnet) {
            Write-Status 'No hay SDK de .NET instalado: se omiten los proyectos SDK (Blazor). Instale el SDK x64: https://dotnet.microsoft.com/download/dotnet/8.0' Warning
            foreach ($sdkProject in @($projects.Values | Where-Object IsSdk)) { Remove-ProjectFromPlan $sdkProject }
        } else {
            $env:DOTNET_ROOT = Split-Path $dotnet
            $env:PATH = (Split-Path $dotnet) + ';' + $env:PATH
            Write-Status "Proyectos SDK con dotnet: $dotnet"
        }
    }
    if (!$projects.Count) { throw 'No quedo ningun proyecto compilable.' }

    Write-Status "Evaluando $($projects.Count) proyectos en paralelo con $workers procesos..."
    Invoke-ProjectQueries
    if (!$projects.Count) { throw 'No quedo ningun proyecto evaluable.' }
    $outputs = @{}
    $assemblies = @{}
    foreach ($project in $projects.Values) {
        if (!$project.Output) { Write-Status "Salida no definida (el proyecto se compila sin registrarse): $($project.Path)" Warning; continue }
        if ($outputs.ContainsKey($project.Output)) { Write-Status "Dos productores para $($project.Output); se conserva el primero." Warning }
        else { $outputs[$project.Output] = $project }
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
            # NuGet moderno gestiona su propia cache (id/version). No confundirla
            # con packages.config (id.version), incluidos los imports de MSTest.
            $nugetRoot = $project.Data.Properties.NuGetPackageRoot
            # Los proyectos clasicos con Reference pueden no definir NuGetPackageRoot.
            # Su cache global usa id\version, no el formato id.version de packages.config.
            if (!$nugetRoot) {
                $nugetRoot = $env:NUGET_PACKAGES
                if (!$nugetRoot) { $nugetRoot = Join-Path $env:USERPROFILE '.nuget\packages' }
            }
            if ($nugetRoot -and $reference.HintPath) {
                $nugetRoot = (FullPath (Split-Path $project.Path) $nugetRoot).TrimEnd('\') + '\'
                if ($hint.StartsWith($nugetRoot, [StringComparison]::OrdinalIgnoreCase)) { continue }
            }
            # Algunas vistas conservan rutas absolutas de otra computadora.
            # Usar la misma version declarada por Seguridad, restaurada abajo.
            if ($assembly -like 'Microsoft.ReportViewer.*.dll') {
                $reportPackages = Join-Path $codeRoot 'componentes\seguridad\Seguridad\CapaVista_Seguridad\packages.config'
                if (Test-Path -LiteralPath $reportPackages) {
                    [xml]$reportManifest = [IO.File]::ReadAllText($reportPackages)
                    $reportPackage = $reportManifest.packages.package | Where-Object id -eq 'Microsoft.ReportingServices.ReportViewerControl.Winforms' | Select-Object -First 1
                    if ($reportPackage) {
                        if ($assembly -in @('Microsoft.ReportViewer.WebForms.dll','Microsoft.ReportViewer.WebDesign.dll')) {
                            $webManifest = $reportManifest.packages.package | Where-Object id -eq 'Microsoft.ReportingServices.ReportViewerControl.WebForms' | Select-Object -First 1
                            if (!$webManifest) { continue }
                            $reportPackage = $webManifest
                        }
                        $reportPath = Join-Path $packageCache ("$($reportPackage.id).$($reportPackage.version)\lib\net40\$assembly")
                        $referenceOverrides.Add([pscustomobject]@{ Project=$project.Path; Identity=$reference.Identity; NewIdentity=($reference.Identity -split ',')[0].Trim(); Output=$reportPath })
                        continue
                    }
                }
            }
            # RAR no siempre normaliza rutas con muchos '..' antes de aplicar
            # MAX_PATH. Entregar rutas absolutas cortas tambien a los paquetes.
            if ($reference.HintPath -and $hint -match '[\\/]packages[\\/]') {
                $packageRelative = [regex]::Split($hint, '(?i)[\\/]packages[\\/]', 2)[1]
                $packageHint = Join-Path $packageCache $packageRelative
                $referenceOverrides.Add([pscustomobject]@{ Project=$project.Path; Identity=$reference.Identity; NewIdentity=$null; Output=$packageHint })
                continue
            }
            if ($outputs.ContainsKey($hint)) {
                $project.Dependencies += $outputs[$hint]
                $referenceOverrides.Add([pscustomobject]@{ Project=$project.Path; Identity=$reference.Identity; NewIdentity=$null; Output=$outputs[$hint].Output })
            }
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
                if ($producers.Count -ne 1) {
                    Write-Status "Referencia sin productor unico para $configuration en $($project.Path): $hint" Warning
                    continue
                }
                $producer = $producers[0]
                $project.Dependencies += $producer
                $referenceOverrides.Add([pscustomobject]@{ Project=$project.Path; Identity=$reference.Identity; NewIdentity=$renamedIdentity; Output=$producer.Output })
                Write-Status "Referencia corregida: $assembly -> $($producer.Output)" Warning
            }
        }
        foreach ($reference in @($project.Data.Items.ProjectReference)) {
            $path = FullPath (Split-Path $project.Path) $reference.Identity
            if (!$projects.ContainsKey($path)) { Write-Status "ProjectReference fuera del plan (se ignora): $path" Warning; continue }
            $project.Dependencies += $projects[$path]
        }
        foreach ($dependency in $project.Dependencies) {
            if ($project.Data.Properties.PlatformTarget -eq 'x86' -and $dependency.Data.Properties.PlatformTarget -eq 'x64') {
                Write-Status "Arquitecturas incompatibles: $($project.Path) (x86) -> $($dependency.Path) (x64)" Warning
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
    # El grafo ya compila las dependencias: usar su salida evaluada evita
    # que ProjectReference vuelva a calcular otra plataforma (Debug vs x64).
    $referenceBody += '</ItemGroup></Target><Target Name="ComprasConfigurarProjectReferences" BeforeTargets="AssignProjectConfiguration"><ItemGroup>'
    foreach ($project in $projects.Values | Where-Object { !$_.IsSdk }) {
        foreach ($reference in @($project.Data.Items.ProjectReference)) {
            $path = FullPath (Split-Path $project.Path) $reference.Identity
            if (!$projects.ContainsKey($path)) { continue }
            $producer = $projects[$path]
            if ($producer.IsSdk -or !$producer.Output) { continue }
            $condition = "'`$(MSBuildProjectFullPath)' == '" + (MsbuildEscape $project.Path) + "'"
            $referenceBody += '<ProjectReference Remove="' + (XmlEscape (MsbuildEscape $reference.Identity)) + '" Condition="' + (XmlEscape $condition) + '" />'
            $referenceBody += '<Reference Include="' + (XmlEscape ([IO.Path]::GetFileNameWithoutExtension($producer.Output))) + '" Condition="' + (XmlEscape $condition) + '"><HintPath>' + (XmlEscape $producer.Output) + '</HintPath><Private>true</Private></Reference>'
        }
    }
    $referenceBody += '</ItemGroup></Target><Target Name="ComprasMarcarInicio" BeforeTargets="PrepareForBuild" Condition="&apos;$(ComprasEstadoProyecto)&apos; != &apos;&apos;"><WriteLinesToFile File="$(ComprasEstadoProyecto).started" Lines="$(MSBuildProjectFullPath)" Overwrite="true" /></Target><Target Name="ComprasMarcarExito" AfterTargets="Build" Condition="&apos;$(ComprasEstadoProyecto)&apos; != &apos;&apos;"><WriteLinesToFile File="$(ComprasEstadoProyecto).done" Lines="$(MSBuildProjectFullPath)" Overwrite="true" /></Target></Project>'
    Write-IfChanged $referenceTargets $referenceBody
    $securityProject = @($projects.Values | Where-Object { [IO.Path]::GetFileName($_.Path) -ieq 'Ejecucion_Seguridad.csproj' })
    if ($securityProject.Count -eq 1 -and $securityProject[0].Output) {
        $securityExecutable = $securityProject[0].Output
    }
    $comprasProject = @($projects.Values | Where-Object { [IO.Path]::GetFileName($_.Path) -ieq 'Ejecucion_Compras.csproj' })
    if ($comprasProject.Count -eq 1 -and $comprasProject[0].Output) {
        $comprasExecutable = $comprasProject[0].Output
    }
    # Orden topologico: los ciclos bloquean solo a sus consumidores.
    # No retirar referencias ni usar DLL antiguas para ocultar un ciclo.
    $pending = @($projects.Values | Sort-Object Path)
    while ($pending.Count) {
        $ready = @($pending | Where-Object {
            !@($_.Dependencies | Where-Object { $_.Wave -lt 0 }).Count
        })
        if (!$ready.Count) { break }
        foreach ($project in $ready) {
            $wave = 0
            foreach ($dependency in $project.Dependencies) {
                $wave = [Math]::Max($wave, $dependency.Wave + 1)
            }
            $project.Wave = $wave
        }
        $pending = @($pending | Where-Object { $_.Wave -lt 0 })
    }
    foreach ($project in $pending) {
        $unresolved = @($project.Dependencies | Where-Object { $_.Wave -lt 0 } | ForEach-Object Path)
        Write-Status ("Bloqueado por ciclo o dependencia de un ciclo: {0}`n -> {1}" -f $project.Path, ($unresolved -join "`n -> ")) Warning
        $planningErrors.Add($project.Path)
    }
    foreach ($step in $steps) {
        if (@($step.Projects | Where-Object { $_.Wave -lt 0 }).Count) { $step.State = 'Bloqueada' }
    }
    if (!@($projects.Values | Where-Object { $_.Wave -ge 0 }).Count) {
        throw 'Todos los proyectos estan bloqueados por dependencias circulares.'
    }
    $current = $null
    $runner = Join-Path $runDirectory 'Build.proj'
    $body = '<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003"><ItemGroup>'
    # Un proyecto compartido puede registrarse primero desde una solucion de
    # ejecucion. Sus paquetes pertenecen al proyecto, no a esa solucion.
    foreach ($project in $projects.Values | Where-Object { !$_.IsSdk } | Sort-Object Path) {
        if (!(Test-Path -LiteralPath (Join-Path (Split-Path $project.Path) 'packages.config')) -and !@($project.Data.Items.PackageReference).Count) { continue }
        $properties = "Configuration=$($project.Configuration);Platform=$($project.Platform);RestorePackagesConfig=true;RestoreRepositoryPath=$(MsbuildEscape $packageCache)"
        $body += '<Restauracion Include="' + (XmlEscape (MsbuildEscape $project.Path)) + '"><AdditionalProperties>' + (XmlEscape $properties) + '</AdditionalProperties></Restauracion>'
    }
    # Algunas versiones de NuGet conservan solo el nupkg en la cache.
    $extractScript = Join-Path $runDirectory 'ExtraerPaquetes.ps1'
    $extractCode = @'
param([string]$Root)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression.FileSystem
foreach ($archive in Get-ChildItem -LiteralPath $Root -Filter '*.nupkg' -Recurse) {
    $destination = $archive.DirectoryName.TrimEnd('\') + '\'
    $zip = [IO.Compression.ZipFile]::OpenRead($archive.FullName)
    try {
        foreach ($entry in $zip.Entries) {
            $path = [IO.Path]::GetFullPath((Join-Path $destination $entry.FullName))
            if (!$path.StartsWith($destination, [StringComparison]::OrdinalIgnoreCase)) { throw "Ruta de paquete invalida: $path" }
            if (!$entry.Name) { $null = [IO.Directory]::CreateDirectory($path); continue }
            if ([IO.File]::Exists($path)) { continue }
            $null = [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($path))
            [IO.Compression.ZipFileExtensions]::ExtractToFile($entry, $path, $false)
        }
    } finally { $zip.Dispose() }
}
'@
    Write-IfChanged $extractScript $extractCode
    $extractCommand = '"' + (Join-Path $PSHOME 'powershell.exe') + '" -NoProfile -ExecutionPolicy Bypass -File "' + $extractScript + '" -Root "' + $packageCache + '"'
    $packageCopies = ''
    $copyDestinations = @{}
    foreach ($project in $projects.Values | Where-Object { !$_.IsSdk }) {
        $manifest = Join-Path (Split-Path $project.Path) 'packages.config'
        if (!(Test-Path -LiteralPath $manifest)) { continue }
        [xml]$manifestXml = [IO.File]::ReadAllText($manifest)
        foreach ($package in $manifestXml.packages.package) {
            $name = "$($package.id).$($package.version)"
            $destination = Join-Path (Join-Path (Split-Path (Split-Path $project.Path)) 'packages') $name
            if ($copyDestinations.ContainsKey($destination)) { continue }
            $copyDestinations[$destination] = $true
            $source = Join-Path $packageCache $name
            # Robocopy admite rutas largas sin crear unidades ni cambiar Windows.
            $command = 'robocopy "' + $source + '" "' + $destination + '" /E /NFL /NDL /NJH /NJS /NP /R:1 /W:1 & if errorlevel 8 (exit /b 1) else (exit /b 0)'
            $packageCopies += '<Exec Command="' + (XmlEscape $command) + '" />'
        }
    }
    $index = 0
    foreach ($project in $projects.Values | Where-Object { $_.Wave -ge 0 } | Sort-Object Path) {
        $project.Marker = Join-Path $runDirectory ("proyecto_$index")
        $properties = "Configuration=$($project.Configuration);Platform=$($project.Platform);BuildProjectReferences=false;ComprasEstadoProyecto=$(MsbuildEscape $project.Marker)"
        if ($project.Data.Properties.OutputType -in @('Exe','WinExe') -and $project.Data.Properties.PlatformTarget -eq 'AnyCPU') { $properties += ';Prefer32Bit=false' }
        if ($project.IsSdk) { $index++; continue }
        $body += '<Nivel' + $project.Wave + ' Include="' + (XmlEscape (MsbuildEscape $project.Path)) + '"><AdditionalProperties>' + (XmlEscape $properties) + '</AdditionalProperties></Nivel' + $project.Wave + '>'
        $index++
    }
    # ContinueOnError + StopOnFirstFailure=false: un fallo no detiene los demas proyectos.
    $body += '</ItemGroup><Target Name="Restaurar"><MSBuild Projects="@(Restauracion)" Targets="Restore" BuildInParallel="false" StopOnFirstFailure="false" ContinueOnError="false" /><Exec Command="' + (XmlEscape $extractCommand) + '" />' + $packageCopies + '</Target>'
    $sdkRestore = ''
    foreach ($project in $projects.Values | Where-Object IsSdk | Sort-Object Path) {
        $sdkRestore += '<Exec WorkingDirectory="' + (XmlEscape (Split-Path $project.Path)) + '" Command="' + (XmlEscape (SdkCommand $project 'Restore')) + '" IgnoreStandardErrorWarningFormat="true" ContinueOnError="true" />'
    }
    $body += '<Target Name="RestaurarSdk">' + $sdkRestore + '</Target>'
    $targets = @('Restaurar','RestaurarSdk')
    $waves = @($projects.Values | Where-Object { $_.Wave -ge 0 } | Select-Object -ExpandProperty Wave | Sort-Object -Unique)
    foreach ($wave in $waves) {
        $targets += "Nivel$wave"
        $body += '<Target Name="Nivel' + $wave + '"><Message Importance="high" Text="[COMPILANDO] Nivel ' + ($wave + 1) + '/' + $waves.Count + '" /><MSBuild Projects="@(Nivel' + $wave + ')" Targets="Build" BuildInParallel="true" StopOnFirstFailure="false" ContinueOnError="true" />'
        foreach ($project in $projects.Values | Where-Object { $_.IsSdk -and $_.Wave -eq $wave } | Sort-Object Path) {
            $body += '<Exec WorkingDirectory="' + (XmlEscape (Split-Path $project.Path)) + '" Command="' + (XmlEscape (SdkCommand $project 'Build' $referenceTargets)) + '" IgnoreStandardErrorWarningFormat="true" ContinueOnError="true" />'
        }
        $body += '</Target>'
    }
    $body += '<Target Name="Build" DependsOnTargets="' + ($targets -join ';') + '" /></Project>'
    [IO.File]::WriteAllText($runner, $body, [Text.UTF8Encoding]::new($false))
    Write-Status "Compilando $($projects.Count) proyectos en $($waves.Count) niveles, con hasta $workers procesos en paralelo." Build
    # Sin /warnAsError: ningun aviso detiene la compilacion.
    # Los enlaces duros evitan duplicar DLL grandes; MSBuild copia normalmente
    # cuando el sistema de archivos no permite enlazarlas. No se crea una unidad.
    & $msbuild $runner /t:Build /nologo /verbosity:minimal "/m:$workers" /nr:false '/consoleloggerparameters:ForceConsoleColor;Summary' "/p:CustomAfterMicrosoftCommonTargets=$referenceTargets" /fileLogger ("/fileLoggerParameters:LogFile=" + (Join-Path $runDirectory 'compilacion.log') + ";Verbosity=normal;Encoding=UTF-8") /p:CreateHardLinksForCopyLocalIfPossible=true /p:CreateHardLinksForCopyFilesToOutputDirectoryIfPossible=true /p:CreateHardLinksForCopyAdditionalFilesIfPossible=true
    $code = $LASTEXITCODE
    foreach ($step in $steps) {
        if ($step.Invalid) { $step.State = 'Fallida'; $step.Code = 1; continue }
        if (!$step.Projects.Count -or $step.State -eq 'Bloqueada') { continue }
        $complete = @($step.Projects | Where-Object { !(Test-Path -LiteralPath ($_.Marker + '.done')) })
        if (!$complete.Count) {
            $missingOutput = @($step.Projects | Where-Object { !$_.Output -or !(Test-Path -LiteralPath $_.Output -PathType Leaf) })
            if ($missingOutput.Count) {
                Write-Status "Falta la salida esperada: $($missingOutput[0].Output)" Warning
                $step.State = 'Fallida'
                $step.Code = if ($code) { $code } else { 1 }
            } else {
                $step.State = 'Correcta'
                $step.Code = 0
            }
        } elseif (@($complete | Where-Object { Test-Path -LiteralPath ($_.Marker + '.started') }).Count) {
            $step.State = 'Fallida'
            $step.Code = if ($code) { $code } else { 1 }
        }
    }
    if ($code -ne 0) {
        if (!@($steps | Where-Object State -eq 'Fallida').Count) {
            $first = $steps | Where-Object { $_.State -notin @('Correcta','Bloqueada') -and $_.Projects.Count } | Select-Object -First 1
            if ($first) { $first.State = 'Fallida'; $first.Code = $code }
        }
        throw "La compilacion termino con codigo $code (se compilo todo lo que fue posible)."
    }
    if (@($steps | Where-Object { $_.State -ne 'Correcta' -and $_.Projects.Count }).Count) { throw 'La compilacion termino sin completar todos los proyectos (se compilo todo lo que fue posible).' }
    if ($planningErrors.Count) { throw "No se completaron $($planningErrors.Count) proyectos o soluciones omitidos durante la validacion." }
    Write-ModuleRegistries
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
    Write-Status ('Bloqueadas por dependencias: {0}' -f @($steps | Where-Object State -eq 'Bloqueada').Count) Warning
    Write-Status ('No ejecutadas: {0}' -f @($steps | Where-Object State -eq 'No ejecutada').Count) Warning
    foreach ($step in $steps) {
        $kind = switch ($step.State) { 'Correcta' { 'Success' } 'Fallida' { 'Error' } default { 'Warning' } }
        Write-Status ("{0}: {1}" -f $step.State, $step.Name) $kind
    }
    if ($runDirectory -and [IO.Directory]::Exists($runDirectory) -and $result -ne 0) {
        Write-Status "Diagnostico conservado en: $runDirectory" Warning
    }
    if ($runDirectory -and [IO.Directory]::Exists($runDirectory) -and $result -eq 0) {
        try {
            foreach ($file in [IO.Directory]::GetFiles($runDirectory)) { [IO.File]::Delete($file) }
            [IO.Directory]::Delete($runDirectory)
        } catch { Write-Warning "No se pudo limpiar: $runDirectory" }
    }

}
if ([Console]::IsInputRedirected -or $env:CC_MORE -ieq '--no-menu') { exit $result }
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
        # Se permite ejecutar aunque haya fallos en otros modulos, si el .exe existe.
        if ($result -ne 0) { Write-Status 'Hubo errores de compilacion; se intentara ejecutar si el .exe existe (puede estar desactualizado).' Warning }
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
