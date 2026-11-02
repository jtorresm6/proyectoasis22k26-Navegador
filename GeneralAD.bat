@echo off
setlocal EnableExtensions DisableDelayedExpansion
:: Ejecutar desde cualquier carpeta. Doble clic: compilar y conservar la ventana.
:: /auto: no pausar. /sin-ayudas: usar los CHM existentes. /listar: ver el plan.
:: Incluye su compilador PowerShell: no requiere archivos auxiliares.
set "GENERALAD_ROOT=%~dp0"
set "GENERALAD_SELF=%~f0"
set "AUTO=0"
set "OPCIONES_PS="

:ARGUMENTOS
if "%~1"=="" goto COMPILAR
if /i "%~1"=="/auto" (
    set "AUTO=1"
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/sin-ayudas" (
    set "OPCIONES_PS=%OPCIONES_PS% -UsarAyudasExistentes"
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/listar" (
    set "OPCIONES_PS=%OPCIONES_PS% -SoloListar"
    set "AUTO=1"
    shift
    goto ARGUMENTOS
)
if /i "%~1"=="/?" goto AYUDA
echo [ERROR] Opcion desconocida: "%~1"
set "EXIT_CODE=1"
goto FIN

:COMPILAR
powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -Command "$texto = [IO.File]::ReadAllText($env:GENERALAD_SELF); $marca = [regex]::Match($texto, '(?m)^:__GENERALAD_POWERSHELL__\r?$'); if (-not $marca.Success) { Write-Error 'Falta el compilador incluido en este BAT.'; exit 1 }; & ([scriptblock]::Create($texto.Substring($marca.Index + $marca.Length))) -RepoRoot $env:GENERALAD_ROOT %OPCIONES_PS%"
set "EXIT_CODE=%ERRORLEVEL%"
goto FIN

:AYUDA
echo Uso: %~nx0 [/auto] [/sin-ayudas] [/listar]
echo.
echo Compila las ayudas y todos los proyectos de codigo, incluido Navegador.
echo /auto        Termina sin pausar. Codigo 0: correcto; 1: error.
echo /sin-ayudas  Usa los CHM existentes sin recompilar los archivos HHP.
echo /listar      Muestra el orden de compilacion sin ejecutar herramientas.
echo.
echo Requiere Visual Studio o Build Tools con desarrollo de escritorio .NET,
echo .NET Framework 4.8, SDK .NET 8 o posterior y HTML Help Workshop.
echo Rutas alternativas: variables MSBUILD_MANUAL y HHC_MANUAL.
set "EXIT_CODE=0"
set "AUTO=1"

:FIN
if "%AUTO%"=="0" pause
endlocal & exit /b %EXIT_CODE%

:__GENERALAD_POWERSHELL__
param(
    [switch]$UsarAyudasExistentes,
    [switch]$SoloListar,
    [string]$RepoRoot
)

# Compatible con Windows PowerShell 5.1. Las rutas parten de este archivo.
$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($RepoRoot)) {
    $repo = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
} else {
    $repo = [IO.Path]::GetFullPath($RepoRoot).TrimEnd('\', '/')
}
$script:globalLog = $null
$script:failedFile = $null
$utf8 = New-Object Text.UTF8Encoding($false)

function Write-Status([string]$Message) {
    Write-Host $Message
    if ($script:globalLog) {
        [IO.File]::AppendAllText($script:globalLog, $Message + [Environment]::NewLine, $utf8)
    }
}

function Get-ProjectInfo([IO.FileInfo]$File) {
    [xml]$xml = [IO.File]::ReadAllText($File.FullName)
    $assembly = $xml.SelectSingleNode("/*[local-name()='Project']/*[local-name()='PropertyGroup']/*[local-name()='AssemblyName']")
    $output = $xml.SelectSingleNode("/*[local-name()='Project']/*[local-name()='PropertyGroup']/*[local-name()='OutputType']")
    $name = $File.BaseName
    $assemblyName = $name
    if ($assembly) { $assemblyName = $assembly.InnerText.Trim() }
    $rank = 2
    if ($name -like 'CapaModelo_*') { $rank = 0 }
    elseif ($name -like 'CapaControlador_*') { $rank = 1 }
    elseif ($name -like 'CapaVista_*') { $rank = 3 }
    if ($output -and $output.InnerText -match '^(WinExe|Exe)$') { $rank = 4 }
    [pscustomobject]@{
        Path = $File.FullName
        Directory = $File.DirectoryName
        Relative = $File.FullName.Substring($repo.Length + 1)
        Name = $name
        Assembly = $assemblyName
        Rank = $rank
        Xml = $xml
        LastErrors = @()
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
            $include = $reference.GetAttribute('Include')
            if ($include -match '\$\(') { continue }
            $path = [IO.Path]::GetFullPath([IO.Path]::Combine($project.Directory, $include))
            if ($byPath.ContainsKey($path) -and $path -ne $project.Path) {
                [void]$project.Dependencies.Add($path)
            }
        }
        foreach ($reference in $project.Xml.SelectNodes("//*[local-name()='Reference']")) {
            $hint = $reference.SelectSingleNode("*[local-name()='HintPath']")
            if (-not $hint -or $hint.InnerText -notmatch '[\\/]bin[\\/]') { continue }
            $name = $reference.GetAttribute('Include').Split(',')[0].Trim()
            if ($byAssembly.ContainsKey($name)) {
                foreach ($dependency in $byAssembly[$name]) {
                    if ($dependency.Path -ne $project.Path) { [void]$project.Dependencies.Add($dependency.Path) }
                }
            }
        }
    }
    foreach ($name in @($byAssembly.Keys | Sort-Object)) {
        if ($byAssembly[$name].Count -gt 1) {
            Write-Status "[AVISO] Ensamblado duplicado '$name': se conservan los $($byAssembly[$name].Count) proyectos."
        }
    }
    $pending = @{}
    foreach ($project in $Projects) { $pending[$project.Path] = $project }
    $ordered = New-Object 'System.Collections.Generic.List[object]'
    while ($pending.Count -gt 0) {
        $ready = @($pending.Values | Where-Object {
            $blocked = $false
            foreach ($dependency in $_.Dependencies) {
                if ($pending.ContainsKey($dependency)) { $blocked = $true; break }
            }
            -not $blocked
        } | Sort-Object Rank, Relative)
        if ($ready.Count -eq 0) {
            Write-Status '[AVISO] Hay dependencias circulares. Los proyectos restantes se ordenan por capas y se reintentan si hay progreso.'
            $ready = @($pending.Values | Sort-Object Rank, Relative)
        }
        foreach ($project in $ready) {
            $ordered.Add($project)
            $pending.Remove($project.Path)
        }
    }
    return $ordered.ToArray()
}

function Get-HelpInfo([IO.FileInfo]$File) {
    # Los HHP historicos son ANSI. ReadAllText tambien reconoce BOM UTF-8/UTF-16.
    $encoding = [Text.Encoding]::Default
    $bytes = [IO.File]::ReadAllBytes($File.FullName)
    if ($bytes.Length -ge 3 -and $bytes[0] -eq 239 -and $bytes[1] -eq 187 -and $bytes[2] -eq 191) {
        $encoding = New-Object Text.UTF8Encoding($true)
    } elseif ($bytes.Length -ge 2 -and $bytes[0] -eq 255 -and $bytes[1] -eq 254) {
        $encoding = [Text.Encoding]::Unicode
    } elseif ($bytes.Length -ge 2 -and $bytes[0] -eq 254 -and $bytes[1] -eq 255) {
        $encoding = [Text.Encoding]::BigEndianUnicode
    }
    $content = [IO.File]::ReadAllText($File.FullName, $encoding)
    $match = [regex]::Match($content, '(?im)^\s*Compiled file\s*=([^\r\n]+)')
    if (-not $match.Success) { throw "El HHP no define Compiled file: $($File.FullName)" }
    $destination = [IO.Path]::GetFullPath([IO.Path]::Combine($File.DirectoryName, $match.Groups[1].Value.Trim().Trim('"')))
    if (-not $destination.StartsWith($repo + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase) -or [IO.Path]::GetExtension($destination) -ne '.chm') {
        throw "Salida CHM fuera del repositorio o extension incorrecta: $destination"
    }
    [pscustomobject]@{ Path = $File.FullName; Directory = $File.DirectoryName; Destination = $destination; Content = $content; Encoding = $encoding }
}

function Assert-Chm([string]$Path) {
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Falta el archivo de ayuda: $Path" }
    $stream = [IO.File]::OpenRead($Path)
    try {
        $header = New-Object byte[] 4
        if ($stream.Read($header, 0, 4) -ne 4 -or [Text.Encoding]::ASCII.GetString($header) -ne 'ITSF') {
            throw "El archivo de ayuda no tiene un encabezado CHM valido (ITSF): $Path"
        }
    } finally { $stream.Dispose() }
}

function Find-Tool([string]$Kind) {
    $manual = [Environment]::GetEnvironmentVariable($Kind + '_MANUAL')
    if ($manual) {
        if (-not (Test-Path -LiteralPath $manual -PathType Leaf)) { throw "La ruta ${Kind}_MANUAL no existe: $manual" }
        return [IO.Path]::GetFullPath($manual)
    }
    $exe = 'MSBuild.exe'
    if ($Kind -eq 'HHC') { $exe = 'hhc.exe' }
    $command = Get-Command $exe -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
    $programDirs = @(${env:ProgramFiles(x86)}, $env:ProgramFiles) | Where-Object { $_ } | Select-Object -Unique
    if ($Kind -eq 'HHC') {
        if ($command) { return $command.Source }
        foreach ($directory in $programDirs) {
            $candidate = Join-Path $directory 'HTML Help Workshop\hhc.exe'
            if (Test-Path -LiteralPath $candidate -PathType Leaf) { return $candidate }
        }
        throw 'No se encontro hhc.exe. Instala HTML Help Workshop o define HHC_MANUAL. Para conservar los CHM actuales, ejecuta GeneralAD.bat /sin-ayudas.'
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
        foreach ($version in @('2022', '2019')) {
            foreach ($edition in @('Community', 'Professional', 'Enterprise', 'BuildTools')) {
                $candidate = Join-Path $directory "Microsoft Visual Studio\$version\$edition\MSBuild\Current\Bin\MSBuild.exe"
                if (Test-Path -LiteralPath $candidate -PathType Leaf) { return $candidate }
            }
        }
    }
    throw 'No se encontro MSBuild.exe. Instala Visual Studio/Build Tools con desarrollo de escritorio .NET y .NET Framework 4.8, o define MSBUILD_MANUAL. El proyecto Blazor necesita el SDK .NET 8.'
}

function Invoke-Logged([string]$Executable, [string[]]$Arguments, [string]$LogPath) {
    $savedPreference = $ErrorActionPreference
    try {
        # PS 5.1 convierte stderr nativo en ErrorRecord; el codigo del proceso
        # es el que decide el resultado. Se conserva toda la salida en el log.
        $ErrorActionPreference = 'Continue'
        $global:LASTEXITCODE = $null
        & $Executable @Arguments 2>&1 | Out-File -LiteralPath $LogPath -Encoding utf8 -ErrorAction Stop
        $code = $global:LASTEXITCODE
    } finally { $ErrorActionPreference = $savedPreference }
    [IO.File]::AppendAllText($script:globalLog, [IO.File]::ReadAllText($LogPath) + [Environment]::NewLine, $utf8)
    if ($null -eq $code) { throw "No se pudo iniciar el proceso: $Executable. Log: $LogPath" }
    return $code
}

function Build-Help($Help, [string]$Compiler, [int]$Index, [string]$LogDirectory) {
    $id = [guid]::NewGuid().ToString('N')
    $temporaryProject = Join-Path $Help.Directory ('.compilar-' + $id + '.hhp')
    $temporaryChm = Join-Path $LogDirectory ('temporal-' + $id + '.chm')
    $log = Join-Path $LogDirectory ('ayuda_{0:D3}.log' -f $Index)
    $content = [regex]::Replace($Help.Content, '(?im)^\s*Compiled file\s*=[^\r\n]*', [Text.RegularExpressions.MatchEvaluator]{ param($m) 'Compiled file=' + $temporaryChm })
    try {
        [IO.File]::WriteAllText($temporaryProject, $content, $Help.Encoding)
        Push-Location -LiteralPath $Help.Directory
        try { $code = Invoke-Logged $Compiler @($temporaryProject) $log }
        finally { Pop-Location }
        if ($code -notin @(0, 1)) { throw "hhc.exe termino con codigo $code. Log: $log" }
        if ([IO.File]::ReadAllText($log) -match '(?im)\bHHC\d+\s*:\s*Error\b') {
            throw "hhc.exe notifico un error; se conserva el CHM anterior. Log: $log"
        }
        Assert-Chm $temporaryChm
        Copy-Item -LiteralPath $temporaryChm -Destination $Help.Destination -Force
        Write-Status "[OK] Ayuda: $($Help.Destination)"
    } finally {
        foreach ($temporary in @($temporaryProject, $temporaryChm)) {
            if (Test-Path -LiteralPath $temporary) { Remove-Item -LiteralPath $temporary -Force }
        }
    }
}

function Get-SolutionDirectory([string]$Directory) {
    $cursor = Get-Item -LiteralPath $Directory
    while ($cursor -and $cursor.FullName.StartsWith($repo, [StringComparison]::OrdinalIgnoreCase)) {
        if (Get-ChildItem -LiteralPath $cursor.FullName -Filter '*.sln' -File | Select-Object -First 1) { return $cursor.FullName }
        if ($cursor.FullName -eq $repo) { break }
        $cursor = $cursor.Parent
    }
    return $Directory
}

function Copy-LegacyDlls($Project) {
    $debug = Join-Path $Project.Directory 'bin\Debug'
    if (-not (Test-Path -LiteralPath $debug -PathType Container)) { return }
    $dlls = @(Get-ChildItem -LiteralPath $debug -Filter '*.dll' -File)
    if ($dlls.Count -eq 0) { return }
    $release = Join-Path $Project.Directory 'bin\Release'
    [void][IO.Directory]::CreateDirectory($release)
    foreach ($dll in $dlls) { Copy-Item -LiteralPath $dll.FullName -Destination $release -Force }
}

function New-LegacyBuildTargets([string]$LogDirectory) {
    # Ajustes de los BAT de RRHH y Bancos, sin modificar los proyectos originales.
    $path = Join-Path $LogDirectory 'ajustes_rrhh_bancos.targets'
    $content = @'
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Target Name="GeneralADExcluirClass1Ausente" BeforeTargets="CoreCompile">
    <ItemGroup>
      <Compile Remove="Class1.cs" Condition="!Exists('$(MSBuildProjectDirectory)\Class1.cs')" />
    </ItemGroup>
  </Target>
  <Target Name="GeneralADResolverVistaBancos" BeforeTargets="ResolveAssemblyReferences"
          Condition="'$(MSBuildProjectName)' == 'Ejecucion_Bancos' and '$(BancoVistaDll)' != ''">
    <Error Condition="!Exists('$(BancoVistaDll)')" Text="Falta la DLL de CapaVista_Bancos en Bibliotecas de Bancos. Revise su compilacion." />
    <ItemGroup>
      <Reference Remove="CapaVista_Bancos*" />
      <Reference Include="CapaVista_Bancos">
        <HintPath>$(BancoVistaDll)</HintPath>
        <SpecificVersion>False</SpecificVersion>
        <Private>True</Private>
      </Reference>
    </ItemGroup>
  </Target>
</Project>
'@
    [IO.File]::WriteAllText($path, $content, $utf8)
    return $path
}

try {
    $codeDirectory = Join-Path $repo 'codigo'
    $helpDirectory = Join-Path $repo 'ayuda'
    if (-not (Test-Path -LiteralPath $codeDirectory -PathType Container)) { throw "No existe $codeDirectory" }
    if (-not (Test-Path -LiteralPath $helpDirectory -PathType Container)) { throw "No existe $helpDirectory" }
    $projects = @(Get-ChildItem -LiteralPath $codeDirectory -Filter '*.csproj' -Recurse -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj|packages)[\\/]' } |
        Sort-Object FullName | ForEach-Object { Get-ProjectInfo $_ })
    if ($projects.Count -eq 0) { throw 'No se encontraron proyectos C# en codigo.' }
    $plan = @(Get-BuildPlan $projects)
    $help = @(Get-ChildItem -LiteralPath $helpDirectory -Filter '*.hhp' -Recurse -File | Sort-Object FullName | ForEach-Object { Get-HelpInfo $_ })
    Write-Status "Repositorio: $repo"
    Write-Status "Plan: $($help.Count) proyectos de ayuda y $($plan.Count) proyectos C#."
    if ($SoloListar) {
        foreach ($item in $help) { Write-Status "AYUDA $($item.Path) -> $($item.Destination)" }
        foreach ($chm in Get-ChildItem -LiteralPath $helpDirectory -Filter '*.chm' -Recurse -File | Sort-Object FullName) {
            if ($chm.FullName -notin @($help | ForEach-Object { $_.Destination })) { Write-Status "CHM EXISTENTE $($chm.FullName)" }
        }
        for ($i = 0; $i -lt $plan.Count; $i++) { Write-Status ('{0:D3} {1}' -f ($i + 1), $plan[$i].Relative) }
        exit 0
    }
    $logDirectory = Join-Path $repo 'logs\build_proyecto'
    [void][IO.Directory]::CreateDirectory($logDirectory)
    $script:globalLog = Join-Path $logDirectory 'build_log.txt'
    $script:failedFile = Join-Path $logDirectory 'fallidos.txt'
    [IO.File]::WriteAllText($script:globalLog, 'Inicio: ' + (Get-Date -Format 'yyyy-MM-dd HH:mm:ss') + [Environment]::NewLine, $utf8)
    [IO.File]::WriteAllText($script:failedFile, '', $utf8)
    $compiler = $null
    if (-not $UsarAyudasExistentes -and $help.Count -gt 0) { $compiler = Find-Tool 'HHC' }
    $msbuild = Find-Tool 'MSBUILD'
    Write-Status "MSBuild: $msbuild"
    $legacyTargets = New-LegacyBuildTargets $logDirectory
    Write-Status 'Ajustes RRHH/Bancos: Class1.cs ausente y referencia de Ejecucion_Bancos.'
    Write-Status 'Logistica se compila antes de CapaVista_Seguridad y de su ejecutable, segun sus dependencias.'
    if ($UsarAyudasExistentes) {
        Write-Status '[AVISO] Se conservan y validan los CHM existentes; no se recompilan sus fuentes HTML.'
        foreach ($item in $help) { Assert-Chm $item.Destination }
    } else {
        Write-Status "Compilador de ayudas: $compiler"
        for ($i = 0; $i -lt $help.Count; $i++) {
            Write-Status "Compilando ayuda: $($help[$i].Path)"
            Build-Help $help[$i] $compiler ($i + 1) $logDirectory
        }
    }
    foreach ($chm in Get-ChildItem -LiteralPath $helpDirectory -Filter '*.chm' -Recurse -File) { Assert-Chm $chm.FullName }
    Write-Status '[AVISO] Se compila Debug. Se copian DLL a bin\Release para referencias heredadas; estas copias siguen siendo Debug.'
    $pending = $plan
    $completed = 0
    for ($round = 1; $round -le 3 -and $pending.Count -gt 0; $round++) {
        Write-Status "Ronda $round de 3: $($pending.Count) proyectos pendientes."
        $failed = New-Object 'System.Collections.Generic.List[object]'
        foreach ($project in $pending) {
            $index = [array]::IndexOf($plan, $project) + 1
            $safeName = $project.Name -replace '[^a-zA-Z0-9_.-]', '_'
            $log = Join-Path $logDirectory ('ronda_{0}_proyecto_{1:D3}_{2}.log' -f $round, $index, $safeName)
            Write-Status "[$index/$($plan.Count)] $($project.Relative)"
            try {
                $project.LastErrors = @()
                $solution = (Get-SolutionDirectory $project.Directory).TrimEnd('\', '/') + '/'
                $arguments = @($project.Path, '/restore', '/p:RestorePackagesConfig=true', ('/p:SolutionDir=' + $solution), ('/p:CustomAfterMicrosoftCommonTargets=' + $legacyTargets), '/t:Build', '/p:Configuration=Debug', '/nologo', '/verbosity:minimal')
                if ($project.Xml.SelectSingleNode("//*[local-name()='Compile' and @Include='Class1.cs']") -and -not (Test-Path -LiteralPath (Join-Path $project.Directory 'Class1.cs'))) {
                    Write-Status '[AJUSTE RRHH] Se omite la referencia a Class1.cs ausente, sin modificar el .csproj.'
                }
                if ($project.Name -eq 'Ejecucion_Bancos') {
                    $bancoDll = Join-Path $repo 'codigo\modulos\bancos\Bibliotecas de Bancos\Bancos\CapaVista_Bancos\bin\Debug\CapaVista_Bancos.dll'
                    $arguments += '/p:BancoVistaDll=' + $bancoDll
                    Write-Status '[AJUSTE BANCOS] Se usa CapaVista_Bancos desde Bibliotecas de Bancos.'
                }
                # Algunas tareas de .NET Framework resuelven App.config desde
                # el directorio actual, aunque reciban el CSProj por ruta absoluta.
                Push-Location -LiteralPath $project.Directory
                try { $code = Invoke-Logged $msbuild $arguments $log }
                finally { Pop-Location }
                if ($code -ne 0) { throw "MSBuild termino con codigo $code. Log: $log" }
                Copy-LegacyDlls $project
                $completed++
                Write-Status '[OK]'
            } catch {
                $failed.Add($project)
                $project.LastErrors = @($_.Exception.Message)
                Write-Status "[ERROR] $($_.Exception.Message)"
                if (Test-Path -LiteralPath $log) {
                    $diagnostics = @(Get-Content -LiteralPath $log | Where-Object { $_ -match '(?i)\berror\b' } | Select-Object -First 12)
                    if ($diagnostics.Count -eq 0) {
                        $diagnostics = @(Get-Content -LiteralPath $log | Where-Object { $_ -match 'MSB3245|MSB3243' } | Select-Object -First 12)
                    }
                    $diagnostics | ForEach-Object { Write-Host $_ }
                    $project.LastErrors += $diagnostics
                }
            }
        }
        if ($failed.Count -eq 0) { $pending = @(); break }
        if ($failed.Count -eq $pending.Count) {
            Write-Status 'No hubo progreso en esta ronda; se detienen los reintentos.'
            $pending = $failed.ToArray()
            break
        }
        $pending = $failed.ToArray()
    }
    [IO.File]::WriteAllLines($script:failedFile, [string[]]@($pending | ForEach-Object { $_.Relative }), $utf8)
    Write-Status "Resultado: $completed correctos, $($pending.Count) con errores, $($plan.Count) en total."
    Write-Status "Log completo: $script:globalLog"
    if ($pending.Count -gt 0) {
        foreach ($project in $pending) {
            Write-Status "PENDIENTE $($project.Relative)"
            $project.LastErrors | Select-Object -First 3 | ForEach-Object { Write-Status "  $_" }
        }
        exit 1
    }
    exit 0
} catch {
    $errorMessage = '[ERROR] ' + $_.Exception.Message
    Write-Host $errorMessage
    if ($script:globalLog) {
        try { [IO.File]::AppendAllText($script:globalLog, $errorMessage + [Environment]::NewLine, $utf8) } catch {}
    }
    if ($script:failedFile) {
        try { [IO.File]::AppendAllText($script:failedFile, $errorMessage + [Environment]::NewLine, $utf8) } catch {}
    }
    exit 1
}
