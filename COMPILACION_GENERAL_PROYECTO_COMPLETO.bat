@echo off
setlocal EnableExtensions DisableDelayedExpansion
:: Compilador autonomo: conservar este BAT dentro del repositorio completo.
:: No necesita ejecutar otros BAT ni editar rutas de usuario.
set "SEGURIDAD_ROOT=%~dp0"
set "SEGURIDAD_SELF=%~f0"
set "SEGURIDAD_NAME=%~nx0"
set "AUTO=0"
set "LISTA_PS="
set "AYUDAS_PS="
set "MODO_PS="
set "COLOR_PS="

:ARGUMENTOS
if "%~1"=="" goto COMPILAR
if /i "%~1"=="/auto" (
    set "AUTO=1"
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/listar" (
    set "LISTA_PS=-SoloListar"
    set "AUTO=1"
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/rebuild" (
    set "MODO_PS=-RecompilarTodo"
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/sin-color" (
    set "COLOR_PS=-SinColor"
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/con-ayudas" (
    set "AYUDAS_PS=-CompilarAyudas"
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/sin-ayudas" (
    set "AYUDAS_PS="
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/?" goto AYUDA
echo [ERROR] Opcion desconocida: "%~1"
set "BUILD_EXIT=1"
goto FIN

:COMPILAR
powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -Command "$texto=[IO.File]::ReadAllText($env:SEGURIDAD_SELF); $marca=[regex]::Match($texto,'(?m)^:__SEGURIDAD_POWERSHELL__\r?$'); if(-not $marca.Success){Write-Error 'Falta el compilador incluido en este BAT.';exit 1}; & ([scriptblock]::Create($texto.Substring($marca.Index+$marca.Length))) -RepoRoot $env:SEGURIDAD_ROOT %LISTA_PS% %AYUDAS_PS% %MODO_PS% %COLOR_PS%"
set "BUILD_EXIT=%ERRORLEVEL%"
goto FIN

:AYUDA
echo Uso: %SEGURIDAD_NAME% [/auto] [/listar] [/rebuild] [/sin-color] [/con-ayudas ^| /sin-ayudas]
echo.
echo Compila todos los componentes y modulos de codigo en Debug.
echo Sin opciones: compilacion incremental y pausa al terminar.
echo /auto        Termina sin pausar. Codigo 0: correcto; 1: error.
echo /listar      Muestra los proyectos sin ejecutar MSBuild ni modificar ayudas.
echo /rebuild     Restaura paquetes y recompila todos los proyectos desde cero.
echo /sin-color   Usa texto simple, sin colores ni barra interactiva.
echo /con-ayudas  Compila tambien los archivos HHP con HTML Help Workshop.
echo /sin-ayudas  Conserva las ayudas existentes. Es el modo predeterminado.
echo.
echo Busca el repositorio desde la ubicacion del BAT, sin rutas de usuario fijas.
echo Requiere Windows, Visual Studio 2022 o Build Tools actualizado con
echo desarrollo de escritorio .NET, .NET Framework 4.8 y SDK .NET 8 o posterior
echo compatible con MSBuild. NuGet puede necesitar Internet.
echo Variables opcionales: MSBUILD_MANUAL y HHC_MANUAL.
set "BUILD_EXIT=0"
set "AUTO=1"

:FIN
if "%AUTO%"=="0" pause
endlocal & exit /b %BUILD_EXIT%

:__SEGURIDAD_POWERSHELL__
param(
    [switch]$SoloListar,
    [switch]$RecompilarTodo,
    [switch]$SinColor,
    [switch]$CompilarAyudas,
    [string]$RepoRoot
)

# Coordinador compatible con Windows PowerShell 5.1.
$ErrorActionPreference = 'Stop'
$repo = $null
$script:globalLog = $null
$script:failedFile = $null
$script:compileDrive = $null
$script:substPath = Join-Path $env:SystemRoot 'System32\subst.exe'
$script:referenceOverrides = New-Object 'System.Collections.Generic.List[object]'
$script:restoreForImports = $false
$script:timer = [Diagnostics.Stopwatch]::StartNew()
$script:useColor = -not $SinColor -and -not [Console]::IsOutputRedirected -and -not $env:NO_COLOR
$utf8 = New-Object Text.UTF8Encoding($false)

function Get-RepositoryRoot([string]$StartDirectory) {
    if ([string]::IsNullOrWhiteSpace($StartDirectory)) { throw 'No se recibio la ubicacion del BAT.' }
    $cursor = Get-Item -LiteralPath $StartDirectory
    while ($cursor) {
        if ((Test-Path -LiteralPath (Join-Path $cursor.FullName 'codigo\componentes') -PathType Container) -and
            (Test-Path -LiteralPath (Join-Path $cursor.FullName 'codigo\modulos') -PathType Container)) {
            return $cursor.FullName.TrimEnd('\', '/')
        }
        $cursor = $cursor.Parent
    }
    throw 'No se encontro el repositorio: faltan codigo\componentes y codigo\modulos. Coloca este BAT dentro de una copia completa del proyecto.'
}

function Write-Status([string]$Message, [ConsoleColor]$Color = [ConsoleColor]::Gray) {
    if ($Message -match '\[ERROR') { $Color = [ConsoleColor]::Red }
    elseif ($Message -match '\[(AVISO|BLOQUEADO|SIN PROYECTOS)') { $Color = [ConsoleColor]::Yellow }
    elseif ($Message -match '\[OK\]') { $Color = [ConsoleColor]::Green }
    if ($script:useColor) { Write-Host $Message -ForegroundColor $Color }
    else { Write-Host $Message }
    if ($script:globalLog) {
        [IO.File]::AppendAllText($script:globalLog, $Message + [Environment]::NewLine, $utf8)
    }
}

function Write-Header([int]$Count) {
    $mode = 'Incremental'
    if ($RecompilarTodo) { $mode = 'Recompilacion completa' }
    Write-Status ''
    Write-Status '  ================================================================' Cyan
    Write-Status '       SEGURIDAD  |  COMPILADOR DEL PROYECTO' Cyan
    Write-Status '  ================================================================' Cyan
    Write-Status ('  {0}  |  Debug  |  {1} proyectos' -f $mode, $Count) White
    Write-Status ''
}

function Write-ProjectResult($Project, [int]$Index, [int]$Total) {
    $label = 'OK'
    if ($Project.State -eq 'Error') { $label = 'ERROR' }
    elseif ($Project.State -eq 'Bloqueado') { $label = 'BLOQUEADO' }
    $percent = [int][Math]::Floor(100 * $Index / $Total)
    Write-Status ('[{0}] {1,3}/{2} {3,3}%  {4} > {5}  ({6:N1}s)' -f
        $label, $Index, $Total, $percent, $Project.Group, $Project.Name, $Project.Duration)
}

function Write-BuildSummary($Plan, [string]$Directory) {
    $ok = @($Plan | Where-Object State -eq 'Correcto').Count
    $errors = @($Plan | Where-Object State -eq 'Error').Count
    $blocked = @($Plan | Where-Object State -eq 'Bloqueado').Count
    Write-Status ''
    Write-Status '  ================================================================' Cyan
    Write-Status ('  RESULTADO   {0}/{1} correctos | {2} errores | {3} bloqueados' -f $ok, $Plan.Count, $errors, $blocked) White
    Write-Status ('  DURACION    {0:00}:{1:00}:{2:00}' -f [Math]::Floor($script:timer.Elapsed.TotalHours), $script:timer.Elapsed.Minutes, $script:timer.Elapsed.Seconds) White
    Write-Status '  ----------------------------------------------------------------' DarkGray
    Write-Status ('  {0,-27} {1,6} {2,7} {3,10}' -f 'COMPONENTE / MODULO', 'OK', 'ERROR', 'BLOQUEADO')
    foreach ($group in $Plan | Group-Object Group | Sort-Object Name) {
        $correct = @($group.Group | Where-Object State -eq 'Correcto').Count
        $failed = @($group.Group | Where-Object State -eq 'Error').Count
        $waiting = @($group.Group | Where-Object State -eq 'Bloqueado').Count
        Write-Status ('  {0,-27} {1,6} {2,7} {3,10}' -f $group.Name, $correct, $failed, $waiting)
    }
    Write-Status '  ================================================================' Cyan
    Write-Status ''
    Write-Status "Registros: $Directory"
    Write-Status 'Detalle: resultados.csv | resumen_modulos.csv | fallidos.txt'
}

function Get-SourceProjects([string]$Directory) {
    # Podar paquetes y salidas antes de recorrerlos; no seguir enlaces.
    $queue = New-Object 'System.Collections.Generic.Queue[string]'
    $queue.Enqueue($Directory)
    while ($queue.Count -gt 0) {
        foreach ($entry in Get-ChildItem -LiteralPath $queue.Dequeue()) {
            if ($entry.Attributes -band [IO.FileAttributes]::ReparsePoint) { continue }
            if ($entry.PSIsContainer) {
                if ($entry.Name -notmatch '^(bin|obj|packages|logs|node_modules|\.git|\.vs)$') {
                    $queue.Enqueue($entry.FullName)
                }
            } elseif ($entry.Name -like '*.csproj') { $entry }
            elseif ($entry.Name -match '^Directory\.Build\.(props|targets)$') { $script:restoreForImports = $true }
        }
    }
}

function Expand-ProjectPath([string]$Path, [string]$Directory) {
    return $Path.Replace('$(Configuration)', 'Debug').Replace('$(Platform)', 'AnyCPU').Replace('$(MSBuildProjectDirectory)', $Directory)
}

function Get-ProjectInfo([IO.FileInfo]$File) {
    [xml]$xml = [IO.File]::ReadAllText($File.FullName)
    $assembly = $xml.SelectSingleNode("/*[local-name()='Project']/*[local-name()='PropertyGroup']/*[local-name()='AssemblyName']")
    $output = $xml.SelectSingleNode("/*[local-name()='Project']/*[local-name()='PropertyGroup']/*[local-name()='OutputType']")
    $framework = $xml.SelectSingleNode("/*[local-name()='Project']/*[local-name()='PropertyGroup']/*[local-name()='TargetFramework']")
    $assemblyName = $File.BaseName
    if ($assembly) { $assemblyName = $assembly.InnerText.Trim() }
    $type = 'Library'
    if ($output) { $type = $output.InnerText.Trim() }
    elseif ($xml.DocumentElement.GetAttribute('Sdk') -match '\.Web$') { $type = 'Exe' }
    $rank = 2
    if ($File.BaseName -like 'CapaModelo_*') { $rank = 0 }
    elseif ($File.BaseName -like 'CapaControlador_*') { $rank = 1 }
    elseif ($File.BaseName -like 'CapaVista_*') { $rank = 3 }
    if ($type -match '^(WinExe|Exe)$') { $rank = 4 }
    $isSdk = $xml.DocumentElement.HasAttribute('Sdk') -or
        $null -ne $xml.SelectSingleNode("//*[local-name()='Sdk' or (local-name()='Import' and @Sdk)]")
    $needsRestore = (Test-Path -LiteralPath (Join-Path $File.DirectoryName 'packages.config')) -or
        $isSdk -or $null -ne $xml.SelectSingleNode("//*[local-name()='PackageReference']")
    foreach ($import in $xml.SelectNodes("//*[local-name()='Import' and @Project]")) {
        # Los demas imports pueden introducir PackageReference indirectamente.
        $path = $import.GetAttribute('Project')
        $known = $path -match '^\$\(MSBuild(ExtensionsPath[^)]*|ToolsPath|BinPath|FrameworkToolsPath)\)[\\/]' -and
            $path -match '[\\/]Microsoft\.(Common\.props|CSharp\.targets)$'
        if (-not $known) { $needsRestore = $true }
    }
    $extension = '.dll'
    if ($type -match '^(WinExe|Exe)$' -and -not $isSdk) { $extension = '.exe' }
    $outputDirectory = Join-Path $File.DirectoryName 'bin\Debug'
    if ($isSdk -and $framework) { $outputDirectory = Join-Path $outputDirectory $framework.InnerText.Trim() }
    [pscustomobject]@{
        Path = $File.FullName; Directory = $File.DirectoryName
        Relative = $File.FullName.Substring($repo.Length + 1)
        Name = $File.BaseName; Assembly = $assemblyName; Rank = $rank; Xml = $xml
        Output = Join-Path $outputDirectory ($assemblyName + $extension)
        Group = (($File.FullName.Substring($repo.Length + 1) -split '[\\/]')[1..2] -join '\')
        LastErrors = @(); ReferenceErrors = New-Object 'System.Collections.Generic.List[string]'
        State = 'Pendiente'; Attempts = 0; Duration = 0.0; Log = ''
        SharedOutput = $false; NeedsRestore = [bool]$needsRestore
        Dependencies = New-Object 'System.Collections.Generic.HashSet[string]' ([StringComparer]::OrdinalIgnoreCase)
    }
}

function Get-BuildPlan($Projects) {
    $byPath = @{}
    $byAssembly = @{}
    foreach ($project in $Projects) {
        $byPath[$project.Path] = $project
        if (-not $byAssembly.ContainsKey($project.Assembly)) { $byAssembly[$project.Assembly] = @() }
        $byAssembly[$project.Assembly] += $project
    }
    foreach ($project in $Projects) {
        foreach ($reference in $project.Xml.SelectNodes("//*[local-name()='ProjectReference']")) {
            $include = Expand-ProjectPath $reference.GetAttribute('Include') $project.Directory
            if ($include -match '\$\(') {
                $project.ReferenceErrors.Add("ProjectReference con propiedades no resueltas: $include")
                continue
            }
            $path = [IO.Path]::GetFullPath([IO.Path]::Combine($project.Directory, $include))
            if ($byPath.ContainsKey($path) -and $path -ne $project.Path) { [void]$project.Dependencies.Add($path) }
            else { $project.ReferenceErrors.Add("ProjectReference fuera del plan: $include") }
        }
        foreach ($reference in $project.Xml.SelectNodes("//*[local-name()='Reference']")) {
            $name = $reference.GetAttribute('Include').Split(',')[0].Trim()
            if (-not $byAssembly.ContainsKey($name)) { continue }
            $candidates = @($byAssembly[$name] | Where-Object { $_.Path -ne $project.Path })
            $producers = @()
            $hint = $reference.SelectSingleNode("*[local-name()='HintPath']")
            if ($hint) {
                $hintText = Expand-ProjectPath $hint.InnerText $project.Directory
                if ($hintText -notmatch '\$\(') {
                    $hintPath = [IO.Path]::GetFullPath([IO.Path]::Combine($project.Directory, $hintText))
                    $producers = @($candidates | Where-Object {
                        $hintPath.StartsWith($_.Directory + '\bin\', [StringComparison]::OrdinalIgnoreCase)
                    })
                }
            }
            if ($producers.Count -eq 0 -and $candidates.Count -gt 0) {
                if (@($candidates.Output | Sort-Object -Unique).Count -eq 1) { $producers = $candidates }
            }
            if ($producers.Count -eq 0) {
                $project.ReferenceErrors.Add("Referencia interna ambigua: $name. Revise HintPath.")
                continue
            }
            foreach ($dependency in $producers) { [void]$project.Dependencies.Add($dependency.Path) }
            $script:referenceOverrides.Add([pscustomobject]@{
                Project = $project.Path; Identity = $reference.GetAttribute('Include'); Output = $producers[0].Output
            })
        }
    }
    foreach ($group in $Projects | Group-Object Output | Where-Object Count -gt 1) {
        foreach ($project in $group.Group) { $project.SharedOutput = $true }
    }
    $pending = @{}
    foreach ($project in $Projects) { $pending[$project.Path] = $project }
    $ordered = New-Object 'System.Collections.Generic.List[object]'
    $warned = $false
    while ($pending.Count -gt 0) {
        $ready = @($pending.Values | Where-Object {
            $blocked = $false
            foreach ($dependency in $_.Dependencies) {
                if ($pending.ContainsKey($dependency)) { $blocked = $true; break }
            }
            -not $blocked
        } | Sort-Object Rank, Relative)
        if ($ready.Count -eq 0) {
            if (-not $warned) { Write-Status '[AVISO] Dependencias circulares: se registraran los proyectos bloqueados.'; $warned = $true }
            $ready = @($pending.Values | Sort-Object Rank, Relative)
        }
        $project = $ready[0]
        $ordered.Add($project)
        $pending.Remove($project.Path)
    }
    return $ordered.ToArray()
}

function Find-Tool([string]$Kind) {
    $manual = [Environment]::GetEnvironmentVariable($Kind + '_MANUAL')
    if ($manual) {
        if (-not (Test-Path -LiteralPath $manual -PathType Leaf)) { throw "La ruta $($Kind)_MANUAL no existe: $manual" }
        return [IO.Path]::GetFullPath($manual)
    }
    $exe = 'MSBuild.exe'
    if ($Kind -eq 'HHC') { $exe = 'hhc.exe' }
    $command = Get-Command $exe -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
    $programDirs = @([Environment]::GetFolderPath('ProgramFilesX86'), [Environment]::GetFolderPath('ProgramFiles')) |
        Where-Object { $_ } | Select-Object -Unique
    if ($Kind -eq 'HHC') {
        if ($command) { return $command.Source }
        foreach ($directory in $programDirs) {
            $candidate = Join-Path $directory 'HTML Help Workshop\hhc.exe'
            if (Test-Path -LiteralPath $candidate -PathType Leaf) { return $candidate }
        }
        throw 'No se encontro hhc.exe. Instala HTML Help Workshop o define HHC_MANUAL para usar /con-ayudas.'
    }
    foreach ($directory in $programDirs) {
        $vswhere = Join-Path $directory 'Microsoft Visual Studio\Installer\vswhere.exe'
        if (Test-Path -LiteralPath $vswhere -PathType Leaf) {
            $candidates = @(& $vswhere -latest -products '*' -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe')
            foreach ($candidate in $candidates) {
                if (Test-Path -LiteralPath $candidate -PathType Leaf) { return $candidate }
            }
        }
    }
    if ($command) { return $command.Source }
    foreach ($directory in $programDirs) {
        foreach ($edition in @('Community', 'Professional', 'Enterprise', 'BuildTools')) {
            $candidate = Join-Path $directory "Microsoft Visual Studio\2022\$edition\MSBuild\Current\Bin\MSBuild.exe"
            if (Test-Path -LiteralPath $candidate -PathType Leaf) { return $candidate }
        }
    }
    throw 'No se encontro MSBuild. Instala Visual Studio 2022/Build Tools con escritorio .NET y .NET Framework 4.8, o define MSBUILD_MANUAL. Blazor necesita SDK .NET 8 o posterior compatible.'
}

function Initialize-CompileDrive {
    # Alias temporal para evitar MAX_PATH en NuGet packages.config.
    if (Test-Path -LiteralPath $script:substPath -PathType Leaf) {
        foreach ($letter in 90..68) {
            $drive = ([char]$letter).ToString() + ':'
            if (Test-Path -LiteralPath ($drive + '\')) { continue }
            $saved = $ErrorActionPreference
            try {
                $ErrorActionPreference = 'Continue'
                $global:LASTEXITCODE = $null
                & $script:substPath $drive $repo 2>&1 | Out-Null
                $code = $LASTEXITCODE
            } catch { $code = 1 }
            finally { $ErrorActionPreference = $saved }
            if ($code -eq 0) {
                $script:compileDrive = $drive
                Write-Status "Ruta corta temporal de compilacion: $drive\ -> $repo"
                return
            }
        }
    }
    Write-Status '[AVISO] No se pudo crear el alias SUBST. Se usaran las rutas originales; NuGet podria informar rutas largas.'
}

function Get-CompilePath([string]$Path) {
    if ($script:compileDrive) {
        if ($Path.Equals($repo, [StringComparison]::OrdinalIgnoreCase)) { return $script:compileDrive + '\' }
        if ($Path.StartsWith($repo + '\', [StringComparison]::OrdinalIgnoreCase) -or
            $Path.StartsWith($repo + '/', [StringComparison]::OrdinalIgnoreCase)) {
            return $script:compileDrive + $Path.Substring($repo.Length)
        }
    }
    return $Path
}

function ConvertTo-MSBuildLiteral([string]$Value) {
    return [regex]::Replace($Value, '[%$@''();?*]', {
        param($match)
        '%' + ([int][char]$match.Value).ToString('X2')
    })
}

function New-LegacyBuildTargets([string]$LogDirectory) {
    $content = @'
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Target Name="SeguridadExcluirClass1Ausente" BeforeTargets="CoreCompile">
    <ItemGroup>
      <Compile Remove="Class1.cs" Condition="!Exists('$(MSBuildProjectDirectory)\Class1.cs')" />
    </ItemGroup>
  </Target>
</Project>
'@
    [xml]$document = $content
    $ns = $document.DocumentElement.NamespaceURI
    $target = $document.CreateElement('Target', $ns)
    $target.SetAttribute('Name', 'SeguridadResolverReferenciasInternas')
    $target.SetAttribute('BeforeTargets', 'ResolveAssemblyReferences')
    [void]$document.DocumentElement.AppendChild($target)
    $items = $document.CreateElement('ItemGroup', $ns)
    [void]$target.AppendChild($items)
    foreach ($reference in $script:referenceOverrides) {
        $node = $document.CreateElement('Reference', $ns)
        $projectPath = ConvertTo-MSBuildLiteral (Get-CompilePath $reference.Project)
        $identity = ConvertTo-MSBuildLiteral $reference.Identity
        $node.SetAttribute('Condition', ("'{0}' == '{1}' And '{2}' == '{3}'" -f
            '$(MSBuildProjectFullPath)', $projectPath, '%(Reference.Identity)', $identity))
        foreach ($pair in @(@('HintPath', (ConvertTo-MSBuildLiteral (Get-CompilePath $reference.Output))), @('SpecificVersion', 'False'))) {
            $metadata = $document.CreateElement($pair[0], $ns)
            $metadata.InnerText = $pair[1]
            [void]$node.AppendChild($metadata)
        }
        [void]$items.AppendChild($node)
    }
    $content = $document.OuterXml
    $hasher = [Security.Cryptography.SHA256]::Create()
    try { $key = ([BitConverter]::ToString($hasher.ComputeHash($utf8.GetBytes($content)))).Replace('-', '').Substring(0, 16) }
    finally { $hasher.Dispose() }
    $cache = Join-Path $repo 'logs\build_seguridad_global\cache'
    [void][IO.Directory]::CreateDirectory($cache)
    $path = Join-Path $cache ('ajustes_' + $key + '.targets')
    if (-not (Test-Path -LiteralPath $path) -or [IO.File]::ReadAllText($path) -cne $content) {
        [IO.File]::WriteAllText($path, $content, $utf8)
    }
    Copy-Item -LiteralPath $path -Destination (Join-Path $LogDirectory 'ajustes_referencias.targets') -Force
    return $path
}

function Get-SolutionDirectory([string]$Directory) {
    $cursor = Get-Item -LiteralPath $Directory
    while ($cursor -and ($cursor.FullName -eq $repo -or
            $cursor.FullName.StartsWith($repo + '\', [StringComparison]::OrdinalIgnoreCase))) {
        if (Get-ChildItem -LiteralPath $cursor.FullName -Filter '*.sln' -File | Select-Object -First 1) { return $cursor.FullName }
        if ($cursor.FullName -eq $repo) { break }
        $cursor = $cursor.Parent
    }
    return $Directory
}

function Invoke-Logged([string]$Executable, [string[]]$Arguments, [string]$LogPath) {
    $saved = $ErrorActionPreference
    try {
        $ErrorActionPreference = 'Continue'
        $global:LASTEXITCODE = $null
        & $Executable @Arguments 2>&1 | Out-File -LiteralPath $LogPath -Encoding utf8 -ErrorAction Stop
        $code = $global:LASTEXITCODE
    } finally { $ErrorActionPreference = $saved }
    [IO.File]::AppendAllText($script:globalLog, [IO.File]::ReadAllText($LogPath) + [Environment]::NewLine, $utf8)
    if ($null -eq $code) { throw "No se pudo iniciar: $Executable. Log: $LogPath" }
    return $code
}

function Copy-LegacyDlls($Project) {
    $debug = Join-Path $Project.Directory 'bin\Debug'
    if (-not (Test-Path -LiteralPath $debug -PathType Container)) { return }
    $dlls = @(Get-ChildItem -LiteralPath $debug -Filter '*.dll' -File)
    if ($dlls.Count -eq 0) { return }
    $release = Join-Path $Project.Directory 'bin\Release'
    [void][IO.Directory]::CreateDirectory($release)
    foreach ($dll in $dlls) {
        $destination = Join-Path $release $dll.Name
        if (Test-Path -LiteralPath $destination -PathType Leaf) {
            $existing = Get-Item -LiteralPath $destination
            if ($existing.Length -eq $dll.Length -and $existing.LastWriteTimeUtc -eq $dll.LastWriteTimeUtc) { continue }
        }
        Copy-Item -LiteralPath $dll.FullName -Destination $destination -Force
    }
}

function Get-HelpInfo([IO.FileInfo]$File) {
    $encoding = [Text.Encoding]::Default
    $bytes = [IO.File]::ReadAllBytes($File.FullName)
    if ($bytes.Length -ge 3 -and $bytes[0] -eq 239 -and $bytes[1] -eq 187 -and $bytes[2] -eq 191) { $encoding = New-Object Text.UTF8Encoding($true) }
    elseif ($bytes.Length -ge 2 -and $bytes[0] -eq 255 -and $bytes[1] -eq 254) { $encoding = [Text.Encoding]::Unicode }
    elseif ($bytes.Length -ge 2 -and $bytes[0] -eq 254 -and $bytes[1] -eq 255) { $encoding = [Text.Encoding]::BigEndianUnicode }
    $content = [IO.File]::ReadAllText($File.FullName, $encoding)
    $match = [regex]::Match($content, '(?im)^\s*Compiled file\s*=([^\r\n]+)')
    if (-not $match.Success) { throw "El HHP no define Compiled file: $($File.FullName)" }
    $destination = [IO.Path]::GetFullPath([IO.Path]::Combine($File.DirectoryName, $match.Groups[1].Value.Trim().Trim('"')))
    if (-not $destination.StartsWith($repo + '\', [StringComparison]::OrdinalIgnoreCase) -or
        [IO.Path]::GetExtension($destination) -ine '.chm') { throw "Salida CHM fuera del repositorio o no valida: $destination" }
    [pscustomobject]@{ Path = $File.FullName; Directory = $File.DirectoryName; Destination = $destination; Content = $content; Encoding = $encoding }
}

function Assert-Chm([string]$Path) {
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Falta el archivo de ayuda: $Path" }
    $stream = [IO.File]::OpenRead($Path)
    try {
        $header = New-Object byte[] 4
        if ($stream.Read($header, 0, 4) -ne 4 -or [Text.Encoding]::ASCII.GetString($header) -ne 'ITSF') {
            throw "El archivo no tiene un encabezado CHM valido (ITSF): $Path"
        }
    } finally { $stream.Dispose() }
}

function Build-Help($Help, [string]$Compiler, [int]$Index, [string]$LogDirectory) {
    $id = [guid]::NewGuid().ToString('N')
    $temporaryProject = Join-Path $Help.Directory ('.compilar-' + $id + '.hhp')
    $temporaryChm = Join-Path $LogDirectory ('temporal-' + $id + '.chm')
    $log = Join-Path $LogDirectory ('ayuda_{0:D3}.log' -f $Index)
    $content = [regex]::Replace($Help.Content, '(?im)^\s*Compiled file\s*=[^\r\n]*',
        [Text.RegularExpressions.MatchEvaluator]{ param($match) 'Compiled file=' + $temporaryChm })
    try {
        [IO.File]::WriteAllText($temporaryProject, $content, $Help.Encoding)
        Push-Location -LiteralPath $Help.Directory
        try { $code = Invoke-Logged $Compiler @($temporaryProject) $log }
        finally { Pop-Location }
        if ($code -notin @(0, 1) -or [IO.File]::ReadAllText($log) -match '(?im)\bHHC\d+\s*:\s*Error\b') {
            throw "Fallo al compilar la ayuda; se conserva su CHM anterior. Log: $log"
        }
        Assert-Chm $temporaryChm
        Copy-Item -LiteralPath $temporaryChm -Destination $Help.Destination -Force
        Write-Status "[OK] Ayuda: $($Help.Destination)"
    } finally {
        foreach ($path in @($temporaryProject, $temporaryChm)) {
            if (Test-Path -LiteralPath $path) { Remove-Item -LiteralPath $path -Force }
        }
    }
}

function Write-Reports($Plan, [string]$Directory) {
    $Plan | ForEach-Object {
        [pscustomobject]@{
            Area = $_.Group; Proyecto = $_.Relative; Ensamblado = $_.Assembly
            Estado = $_.State; Intentos = $_.Attempts; Registro = $_.Log
            Segundos = [Math]::Round($_.Duration, 2)
            Modo = $(if ($RecompilarTodo -or $_.SharedOutput) { 'Rebuild' } else { 'Build' })
            Detalle = $_.LastErrors -join ' | '
        }
    } | Export-Csv -LiteralPath (Join-Path $Directory 'resultados.csv') -NoTypeInformation -Encoding UTF8
    $Plan | Group-Object Group | Sort-Object Name | ForEach-Object {
        [pscustomobject]@{
            Area = $_.Name; Total = $_.Count
            Correctos = @($_.Group | Where-Object State -eq 'Correcto').Count
            Errores = @($_.Group | Where-Object State -eq 'Error').Count
            Bloqueados = @($_.Group | Where-Object State -eq 'Bloqueado').Count
            Pendientes = @($_.Group | Where-Object State -eq 'Pendiente').Count
        }
    } | Export-Csv -LiteralPath (Join-Path $Directory 'resumen_modulos.csv') -NoTypeInformation -Encoding UTF8
    [IO.File]::WriteAllLines($script:failedFile, [string[]]@($Plan | Where-Object State -ne 'Correcto' |
        ForEach-Object { '[' + $_.State + '] ' + $_.Relative }), $utf8)
    $duplicates = @($Plan | Group-Object Assembly | Where-Object Count -gt 1 | ForEach-Object { $_.Group })
    if ($duplicates.Count -gt 0) {
        $duplicates | Select-Object @{Name='Ensamblado';Expression={$_.Assembly}},
            @{Name='Proyecto';Expression={$_.Relative}}, @{Name='SalidaCompartida';Expression={$_.SharedOutput}} |
            Export-Csv -LiteralPath (Join-Path $Directory 'duplicados.csv') -NoTypeInformation -Encoding UTF8
    }
}

try {
    $repo = Get-RepositoryRoot $RepoRoot
    $codeDirectory = Join-Path $repo 'codigo'
    $helpDirectory = Join-Path $repo 'ayuda'
    $projects = @(Get-SourceProjects $codeDirectory | Sort-Object FullName | ForEach-Object { Get-ProjectInfo $_ })
    if ($projects.Count -eq 0) { throw 'No se encontraron proyectos C# dentro de codigo.' }
    # Directory.Build.* tambien se hereda desde carpetas superiores al clon.
    $cursor = Get-Item -LiteralPath $repo
    while ($cursor) {
        foreach ($name in @('Directory.Build.props', 'Directory.Build.targets')) {
            if (Test-Path -LiteralPath (Join-Path $cursor.FullName $name)) { $script:restoreForImports = $true }
        }
        $cursor = $cursor.Parent
    }
    $plan = @(Get-BuildPlan $projects)
    Write-Header $plan.Count
    Write-Status "Repositorio: $repo"
    $help = @()
    if ($CompilarAyudas -and (Test-Path -LiteralPath $helpDirectory -PathType Container)) {
        $help = @(Get-ChildItem -LiteralPath $helpDirectory -Filter '*.hhp' -Recurse -File |
            Where-Object Name -notlike '.compilar-*' | Sort-Object FullName)
    }
    Write-Status "Plan: $($help.Count) proyectos de ayuda y $($plan.Count) proyectos C#."
    $duplicates = @($projects | Group-Object Assembly | Where-Object Count -gt 1)
    if ($duplicates.Count -gt 0) {
        $shared = @($projects | Where-Object SharedOutput).Count
        Write-Status "[AVISO] $($duplicates.Count) nombres de DLL compartidos; $shared proyectos se recompilan por compartir salida."
    }
    foreach ($root in @('componentes', 'modulos')) {
        foreach ($area in Get-ChildItem -LiteralPath (Join-Path $codeDirectory $root) -Directory | Sort-Object Name) {
            $group = $root + '\' + $area.Name
            $count = @($projects | Where-Object Group -eq $group).Count
            if ($count -eq 0) { Write-Status "[SIN PROYECTOS] $group no contiene archivos .csproj." }
            else { Write-Status "[AREA] $($group): $count proyectos." }
        }
    }
    if ($SoloListar) {
        foreach ($item in $help) { Write-Status "AYUDA $($item.FullName)" }
        for ($index = 0; $index -lt $plan.Count; $index++) { Write-Status ('{0:D3} {1}' -f ($index + 1), $plan[$index].Relative) }
        foreach ($project in $plan) {
            foreach ($message in $project.ReferenceErrors) { Write-Status "[ERROR] $($project.Relative): $message" }
        }
        if (@($plan | Where-Object { $_.ReferenceErrors.Count -gt 0 }).Count -gt 0) { exit 1 }
        exit 0
    }
    $runId = (Get-Date -Format 'yyyyMMdd-HHmmss') + '-' + [guid]::NewGuid().ToString('N').Substring(0, 6)
    $logDirectory = Join-Path $repo ('logs\build_seguridad_global\' + $runId)
    [void][IO.Directory]::CreateDirectory($logDirectory)
    $script:globalLog = Join-Path $logDirectory 'build_log.txt'
    $script:failedFile = Join-Path $logDirectory 'fallidos.txt'
    [IO.File]::WriteAllText($script:globalLog, 'Inicio: ' + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss') + [Environment]::NewLine, $utf8)
    [IO.File]::WriteAllText($script:failedFile, '', $utf8)
    Write-Status "Repositorio: $repo"
    Write-Status "Proyectos: $($plan.Count). Registros: $logDirectory"
    Write-Status ('Modo: {0}' -f $(if ($RecompilarTodo) { 'Rebuild' } else { 'Build incremental' }))
    Write-Reports $plan $logDirectory
    $oldBats = @('Estos BAT no son invocados por el compilador global. Revisar sus opciones antes de retirarlos.', '')
    $oldBats += @(Get-ChildItem -LiteralPath $repo -Filter '*.bat' -Recurse -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj|packages|logs|\.git)[\\/]' -and $_.FullName -ne $env:SEGURIDAD_SELF } |
        Sort-Object FullName | ForEach-Object { $_.FullName.Substring($repo.Length + 1) })
    [IO.File]::WriteAllLines((Join-Path $logDirectory 'bat_anteriores.txt'), [string[]]$oldBats, $utf8)
    $msbuild = Find-Tool 'MSBUILD'
    Write-Status "MSBuild: $msbuild"
    Initialize-CompileDrive
    $legacyTargets = New-LegacyBuildTargets $logDirectory
    Write-Status "Referencias internas resueltas: $($script:referenceOverrides.Count)."
    $helpErrors = New-Object 'System.Collections.Generic.List[string]'
    if ($CompilarAyudas) {
        try {
            if ($help.Count -eq 0) { throw 'No se encontraron proyectos HHP de ayuda.' }
            $compiler = Find-Tool 'HHC'
            for ($index = 0; $index -lt $help.Count; $index++) {
                try {
                    Write-Status "Compilando ayuda: $($help[$index].FullName)"
                    Build-Help (Get-HelpInfo $help[$index]) $compiler ($index + 1) $logDirectory
                } catch {
                    $helpErrors.Add($_.Exception.Message)
                    Write-Status "[ERROR AYUDA] $($_.Exception.Message)"
                }
            }
        } catch {
            $helpErrors.Add($_.Exception.Message)
            Write-Status "[ERROR AYUDA] $($_.Exception.Message)"
        }
    } else { Write-Status 'Se conservan las ayudas existentes. Use /con-ayudas para recompilarlas.' }
    Write-Status '[AVISO] Se compila Debug. Las copias a bin\Release para referencias heredadas siguen siendo Debug.'
    $pending = $plan
    $byPath = @{}
    foreach ($project in $plan) { $byPath[$project.Path] = $project }
    for ($round = 1; $round -le 3 -and $pending.Count -gt 0; $round++) {
        Write-Status "Ronda $round de 3: $($pending.Count) proyectos pendientes."
        $failed = New-Object 'System.Collections.Generic.List[object]'
        foreach ($project in $pending) {
            $index = [array]::IndexOf($plan, $project) + 1
            $safeName = $project.Name -replace '[^a-zA-Z0-9_.-]', '_'
            $log = Join-Path $logDirectory ('ronda_{0}_proyecto_{1:D3}_{2}.log' -f $round, $index, $safeName)
            [IO.File]::AppendAllText($script:globalLog, "[$index/$($plan.Count)] $($project.Relative)" + [Environment]::NewLine, $utf8)
            if ($script:useColor) {
                Write-Progress -Activity 'Compilando componentes y modulos' -Status ($project.Group + ' > ' + $project.Name) -PercentComplete ([int](100 * ($index - 1) / $plan.Count))
            }
            $project.Log = $log
            $unavailable = @($project.Dependencies | Where-Object { $byPath[$_].State -ne 'Correcto' })
            if ($unavailable.Count -gt 0) {
                $project.State = 'Bloqueado'
                $project.LastErrors = @($unavailable | ForEach-Object { 'Dependencia sin compilar: ' + $byPath[$_].Relative })
                [IO.File]::WriteAllLines($log, [string[]]$project.LastErrors, $utf8)
                $failed.Add($project)
                Write-ProjectResult $project $index $plan.Count
                continue
            }
            $attemptTimer = [Diagnostics.Stopwatch]::StartNew()
            try {
                $project.LastErrors = @()
                if ($project.ReferenceErrors.Count -gt 0) { throw ($project.ReferenceErrors -join ' ') }
                $project.Attempts++
                $solution = (Get-SolutionDirectory $project.Directory).TrimEnd('\', '/') + '/'
                $target = 'Build'
                if ($RecompilarTodo -or $project.SharedOutput) { $target = 'Rebuild' }
                $arguments = @((Get-CompilePath $project.Path), ('/p:SolutionDir=' + (ConvertTo-MSBuildLiteral (Get-CompilePath $solution))),
                    ('/p:CustomAfterMicrosoftCommonTargets=' + (ConvertTo-MSBuildLiteral (Get-CompilePath $legacyTargets))),
                    ('/t:' + $target), '/p:Configuration=Debug', '/p:BuildProjectReferences=false',
                    '/p:UseSharedCompilation=false', '/warnAsError:MSB3245,MSB3243', '/nr:false', '/nologo', '/verbosity:minimal')
                if ($RecompilarTodo -or $project.NeedsRestore -or $script:restoreForImports) {
                    $arguments += @('/restore', '/p:RestorePackagesConfig=true')
                }
                if ($project.Xml.SelectSingleNode("//*[local-name()='Compile' and @Include='Class1.cs']") -and
                    -not (Test-Path -LiteralPath (Join-Path $project.Directory 'Class1.cs'))) {
                    Write-Status '[AJUSTE] Se omite la referencia a Class1.cs ausente sin modificar el proyecto.'
                }
                Push-Location -LiteralPath (Get-CompilePath $project.Directory)
                try { $code = Invoke-Logged $msbuild $arguments $log }
                finally { Pop-Location }
                if ($code -ne 0) { throw "MSBuild termino con codigo $code. Log: $log" }
                Copy-LegacyDlls $project
                $project.State = 'Correcto'
            } catch {
                $project.State = 'Error'
                $failed.Add($project)
                $project.LastErrors = @($_.Exception.Message)
                if (Test-Path -LiteralPath $log) {
                    $diagnostics = @(Get-Content -LiteralPath $log | Where-Object { $_ -match '(?i)\berror\b' } | Select-Object -First 12)
                    if ($diagnostics.Count -eq 0) {
                        $diagnostics = @(Get-Content -LiteralPath $log | Where-Object { $_ -match 'MSB3245|MSB3243' } | Select-Object -First 12)
                    }
                    $project.LastErrors += $diagnostics
                } else { [IO.File]::WriteAllLines($log, [string[]]$project.LastErrors, $utf8) }
            } finally {
                $attemptTimer.Stop()
                $project.Duration += $attemptTimer.Elapsed.TotalSeconds
            }
            Write-ProjectResult $project $index $plan.Count
            if ($project.State -eq 'Error') {
                $project.LastErrors | Select-Object -First 3 | ForEach-Object { Write-Status ('  ' + $_) }
            }
        }
        if ($script:useColor) { Write-Progress -Activity 'Compilando componentes y modulos' -Completed }
        Write-Reports $plan $logDirectory
        if ($failed.Count -eq 0) { $pending = @(); break }
        if ($failed.Count -eq $pending.Count) {
            Write-Status 'No hubo progreso en esta ronda; se detienen los reintentos.'
            $pending = $failed.ToArray()
            break
        }
        $pending = $failed.ToArray()
    }
    Write-Reports $plan $logDirectory
    if ($helpErrors.Count -gt 0) {
        [IO.File]::AppendAllText($script:failedFile, '[AYUDAS] ' + ($helpErrors -join [Environment]::NewLine) + [Environment]::NewLine, $utf8)
    }
    Write-BuildSummary $plan $logDirectory
    if ($pending.Count -gt 0) {
        foreach ($project in $pending) {
            Write-Status "PENDIENTE $($project.Relative)"
            $project.LastErrors | Select-Object -First 3 | ForEach-Object { Write-Status ('  ' + $_) }
        }
        exit 1
    }
    if ($helpErrors.Count -gt 0) { exit 1 }
    exit 0
} catch {
    $message = '[ERROR] ' + $_.Exception.Message
    Write-Host $message
    if ($script:globalLog) { try { [IO.File]::AppendAllText($script:globalLog, $message + [Environment]::NewLine, $utf8) } catch {} }
    if ($script:failedFile) { try { [IO.File]::AppendAllText($script:failedFile, $message + [Environment]::NewLine, $utf8) } catch {} }
    exit 1
} finally {
    if ($script:useColor) { Write-Progress -Activity 'Compilando componentes y modulos' -Completed }
    if ($script:compileDrive) {
        try {
            & $script:substPath $script:compileDrive /D 2>&1 | Out-Null
            if ($LASTEXITCODE -ne 0) { Write-Warning "No se pudo quitar el alias temporal $script:compileDrive" }
        } catch { Write-Warning "No se pudo quitar el alias temporal $script:compileDrive" }
    }
}
