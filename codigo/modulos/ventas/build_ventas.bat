@echo off
setlocal EnableExtensions DisableDelayedExpansion
:: Compila Ventas, sus componentes y todas las soluciones dentro de codigo.
:: Uso: build_ventas.bat [Debug|Release] [/sinpausa]
:: Las rutas se calculan desde este archivo; no dependen de la carpeta del equipo.

set "VENTAS_ROOT=%~dp0"
set "VENTAS_BUILD=%VENTAS_ROOT%Compilacion_Ventas\"
set "VENTAS_BIBLIOTECAS=%VENTAS_ROOT%Biblioteca de clase Ventas Y CxC\"
set "VENTAS_SELF=%~f0"
if not defined VENTAS_MSBUILD_NODOS set "VENTAS_MSBUILD_NODOS=%NUMBER_OF_PROCESSORS%"
if not defined VENTAS_MSBUILD_NODOS set "VENTAS_MSBUILD_NODOS=1"
set "VENTAS_CONFIG=%~1"
set "VENTAS_SINPAUSA=0"
if /i "%~2"=="/sinpausa" set "VENTAS_SINPAUSA=1"
if /i "%~2"=="--no-pause" set "VENTAS_SINPAUSA=1"
if not defined VENTAS_CONFIG set "VENTAS_CONFIG=Debug"
if /i "%VENTAS_CONFIG%"=="Debug" set "VENTAS_CONFIG=Debug"
if /i "%VENTAS_CONFIG%"=="Release" set "VENTAS_CONFIG=Release"
if not "%VENTAS_CONFIG%"=="Debug" if not "%VENTAS_CONFIG%"=="Release" (
    echo.
    echo   [!] Perfil no valido. Use Debug o Release.
    echo   Uso: build_ventas.bat [Debug^|Release] [/sinpausa]
    exit /b 2
)

:: Colores: componentes en azul y titulos de modulos en morado.
:: Sin consola compatible se utiliza texto normal.
set "VENTAS_ESC="
set "VENTAS_PANEL="
set "VENTAS_TITULO="
set "VENTAS_AZUL="
set "VENTAS_MORADO="
set "VENTAS_DORADO="
set "VENTAS_VERDE="
set "VENTAS_ROJO="
set "VENTAS_GRIS="
set "VENTAS_RESET="
if "%VENTAS_SINPAUSA%"=="0" title VentasYCxC - Centro de compilacion
:: Habilitar ANSI solamente si existe una consola real, sin modificar el registro.
for /f "usebackq delims=" %%E in (`powershell.exe -NoLogo -NoProfile -NonInteractive -Command "$ErrorActionPreference='Stop'; try { $q=[char]34; $d='[System.Runtime.InteropServices.DllImport('+$q+'kernel32.dll'+$q+')] public static extern System.IntPtr CreateFile(string n, uint a, uint s, System.IntPtr p, uint c, uint f, System.IntPtr t); [System.Runtime.InteropServices.DllImport('+$q+'kernel32.dll'+$q+')] public static extern bool GetConsoleMode(System.IntPtr h, out uint m); [System.Runtime.InteropServices.DllImport('+$q+'kernel32.dll'+$q+')] public static extern bool SetConsoleMode(System.IntPtr h, uint m); [System.Runtime.InteropServices.DllImport(' + $q + 'kernel32.dll' + $q + ')] public static extern bool CloseHandle(System.IntPtr h);'; $t=Add-Type -Namespace VentasBuild -Name ConsoleNative -MemberDefinition $d -PassThru; $h=$t::CreateFile('CONOUT$',3221225472,3,[IntPtr]::Zero,3,0,[IntPtr]::Zero); [uint32]$m=0; if($t::GetConsoleMode($h,[ref]$m)) { if($t::SetConsoleMode($h,($m -bor 4))) { [Console]::Write([char]27) } }; [void]$t::CloseHandle($h) } catch {}" 2^>nul`) do set "VENTAS_ESC=%%E"
if not defined VENTAS_ESC goto COMPILAR
set "VENTAS_PANEL=%VENTAS_ESC%[48;2;59;29;74m%VENTAS_ESC%[38;2;228;212;234m"
set "VENTAS_TITULO=%VENTAS_ESC%[48;2;59;29;74m%VENTAS_ESC%[38;2;201;162;74m%VENTAS_ESC%[1m"
set "VENTAS_AZUL=%VENTAS_ESC%[38;2;86;156;214m"
set "VENTAS_MORADO=%VENTAS_ESC%[38;2;183;133;205m"
set "VENTAS_DORADO=%VENTAS_ESC%[38;2;201;162;74m"
set "VENTAS_VERDE=%VENTAS_ESC%[38;2;112;207;155m"
set "VENTAS_ROJO=%VENTAS_ESC%[38;2;244;126;139m"
set "VENTAS_GRIS=%VENTAS_ESC%[38;2;164;164;178m"
set "VENTAS_RESET=%VENTAS_ESC%[0m"

:COMPILAR
set "VENTAS_RESULT=2"
set "VENTAS_DURACION=No iniciada"
set "VENTAS_SALIDA=%VENTAS_ROOT%Ejecutable_Ventas\Ejecutable_Ventas\bin\%VENTAS_CONFIG%"
set "VENTAS_EJECUTABLE=%VENTAS_SALIDA%\Ejecucion_Ventas.exe"
call :ENCABEZADO
echo   %VENTAS_DORADO%PERFIL%VENTAS_RESET%  %VENTAS_CONFIG%       %VENTAS_DORADO%MODO%VENTAS_RESET%  Paralelo + incremental
echo.
echo   %VENTAS_GRIS%Consultas / Navegador / Seguridad / Reporteador + sus 8 botones
echo.
echo   Facturas                 Listado de precios        Cotizaciones
echo   Movimientos y pagos      Cierre de movimientos     Reportes
echo   Mantenimientos           Comisiones                Pedidos%VENTAS_RESET%
echo.
echo   %VENTAS_DORADO%[1/3]%VENTAS_RESET% Verificando herramientas...
call :BUSCAR_MSBUILD
if errorlevel 1 goto SIN_MSBUILD
if not exist "%VENTAS_BUILD%build_ventas.proj" goto SIN_PROYECTO
if not exist "%VENTAS_BUILD%build_componentes_ventas.proj" goto SIN_PROYECTO
if not exist "%VENTAS_BUILD%build_componentes_ventas.targets" goto SIN_PROYECTO
if not exist "%VENTAS_BIBLIOTECAS%VentasYCxC_Ventas\CapaModelo_Ventas\CapaModelo_Ventas.csproj" goto SIN_BIBLIOTECAS
echo         MSBuild disponible.
echo   %VENTAS_DORADO%[2/3]%VENTAS_RESET% Compilando las soluciones de cada modulo y componente...
echo         %VENTAS_GRIS%No restaura NuGet ni genera paquetes .nupkg.%VENTAS_RESET%
echo.
set "VENTAS_INICIO=%TIME: =0%"
set "VENTAS_RESULT=0"
call :COMPILAR_TODO_EL_PROYECTO
set "VENTAS_SOLUCIONES_RESULT=%ERRORLEVEL%"
if not "%VENTAS_SOLUCIONES_RESULT%"=="0" set "VENTAS_RESULT=1"
if not "%VENTAS_SOLUCIONES_RESULT%"=="0" goto FIN_COMPILACION
echo.
echo   %VENTAS_DORADO%[3/3]%VENTAS_RESET% Integrando Ventas y reuniendo sus DLL...
pushd "%VENTAS_BUILD%"
if errorlevel 1 goto SIN_DIRECTORIO
"%VENTAS_MSBUILD%" "%VENTAS_BUILD%build_ventas.proj" /t:ReunirBibliotecas /p:Configuration=%VENTAS_CONFIG% /p:GeneratePackageOnBuild=false /p:PackageOnBuild=false /nologo /v:minimal /clp:Summary
set "VENTAS_VENTAS_RESULT=%ERRORLEVEL%"
popd
if not "%VENTAS_VENTAS_RESULT%"=="0" set "VENTAS_RESULT=1"
:FIN_COMPILACION
call :CALCULAR_DURACION
goto RESUMEN

:SIN_MSBUILD
echo.
echo   %VENTAS_ROJO%[!] No se encontro MSBuild de Visual Studio.%VENTAS_RESET%
echo       Instale las herramientas de compilacion o defina VENTAS_MSBUILD
echo       con la ruta de MSBuild.exe.
goto RESUMEN

:SIN_PROYECTO
echo.
echo   %VENTAS_ROJO%[!] Falta un archivo del compilador de Ventas. Copie el repositorio completo.%VENTAS_RESET%
goto RESUMEN

:SIN_BIBLIOTECAS
echo.
echo   %VENTAS_ROJO%[!] No se encontraron las bibliotecas de Ventas.%VENTAS_RESET%
echo       Ruta esperada: "%VENTAS_BIBLIOTECAS%"
goto RESUMEN

:SIN_DIRECTORIO
echo.
echo   %VENTAS_ROJO%[!] No se pudo abrir la carpeta de Ventas.%VENTAS_RESET%
set "VENTAS_RESULT=1"
goto RESUMEN

:RESUMEN
echo.
echo   %VENTAS_GRIS%------------------------------------------------------------------------%VENTAS_RESET%
if "%VENTAS_RESULT%"=="0" (
    echo   %VENTAS_VERDE%[OK] COMPILACION COMPLETADA%VENTAS_RESET%
    echo        Todos los modulos y componentes con soluciones quedaron compilados.
) else (
    echo   %VENTAS_ROJO%[!] COMPILACION DETENIDA%VENTAS_RESET%
    echo        Revise el mensaje anterior. Codigo de salida: %VENTAS_RESULT%
)
echo.
echo   Perfil:  %VENTAS_CONFIG%        Tiempo: %VENTAS_DURACION%
echo.
echo   %VENTAS_DORADO%APLICACION%VENTAS_RESET%
echo   Ejecutable_Ventas\Ejecutable_Ventas\bin\%VENTAS_CONFIG%\Ejecucion_Ventas.exe
echo   Inicio: login de Seguridad con seleccion de modulo habilitada.
echo.
echo   Los mensajes de compilacion se muestran solo en esta consola.
echo   %VENTAS_GRIS%------------------------------------------------------------------------%VENTAS_RESET%
if "%VENTAS_SINPAUSA%"=="1" goto SALIR

:MENU
echo.
echo   %VENTAS_DORADO%[R]%VENTAS_RESET% Repetir compilacion     %VENTAS_DORADO%[C]%VENTAS_RESET% Cambiar Debug / Release
if "%VENTAS_RESULT%"=="0" echo   %VENTAS_DORADO%[A]%VENTAS_RESET% Abrir Ventas (login de Seguridad)
echo   %VENTAS_DORADO%[S]%VENTAS_RESET% Salir
echo.
choice /c RCAS /n /m "  Elige una opcion: "
if errorlevel 255 goto SALIR
if errorlevel 4 goto SALIR
if errorlevel 3 goto ABRIR_APLICACION
if errorlevel 2 goto CAMBIAR_PERFIL
if errorlevel 1 goto COMPILAR
goto SALIR

:CAMBIAR_PERFIL
echo.
echo   %VENTAS_DORADO%[D]%VENTAS_RESET% Debug    %VENTAS_DORADO%[R]%VENTAS_RESET% Release    %VENTAS_DORADO%[S]%VENTAS_RESET% Volver
choice /c DRS /n /m "  Perfil: "
if errorlevel 255 goto SALIR
if errorlevel 3 goto MENU
if errorlevel 2 (
    set "VENTAS_CONFIG=Release"
) else (
    set "VENTAS_CONFIG=Debug"
)
goto COMPILAR

:ABRIR_APLICACION
if not "%VENTAS_RESULT%"=="0" (
    echo.
    echo   %VENTAS_ROJO%Primero complete una compilacion correctamente.%VENTAS_RESET%
    goto MENU
)
if not exist "%VENTAS_EJECUTABLE%" (
    echo.
    echo   %VENTAS_ROJO%No se encontro Ejecucion_Ventas.exe. Vuelva a compilar.%VENTAS_RESET%
    goto MENU
)
start "" /d "%VENTAS_SALIDA%" "%VENTAS_EJECUTABLE%"
goto MENU

:SALIR
if defined VENTAS_RESET echo %VENTAS_RESET%
endlocal & exit /b %VENTAS_RESULT%

:ENCABEZADO
if "%VENTAS_SINPAUSA%"=="0" cls
echo.
echo   %VENTAS_PANEL%+----------------------------------------------------------------------+%VENTAS_RESET%
echo   %VENTAS_TITULO%^|                       V E N T A S   /   C x C                        ^|%VENTAS_RESET%
echo   %VENTAS_PANEL%^|                        CENTRO DE COMPILACION                         ^|%VENTAS_RESET%
echo   %VENTAS_PANEL%+----------------------------------------------------------------------+%VENTAS_RESET%
echo.
exit /b 0

:BUSCAR_MSBUILD
if defined VENTAS_MSBUILD goto VALIDAR_MSBUILD
set "VENTAS_VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VENTAS_VSWHERE%" goto MSBUILD_ALTERNATIVO
for /f "usebackq delims=" %%I in (`"%VENTAS_VSWHERE%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "VENTAS_MSBUILD=%%I"
:MSBUILD_ALTERNATIVO
if not defined VENTAS_MSBUILD set "VENTAS_MSBUILD=%ProgramFiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
:VALIDAR_MSBUILD
if not exist "%VENTAS_MSBUILD%" exit /b 1
exit /b 0

:COMPILAR_TODO_EL_PROYECTO
for %%R in ("%VENTAS_ROOT%..\..\..") do set "VENTAS_REPO_ROOT=%%~fR"
powershell.exe -NoLogo -NoProfile -ExecutionPolicy Bypass -Command "$text=[IO.File]::ReadAllText($env:VENTAS_SELF); $marker=[regex]::Match($text,'(?m)^:__VENTAS_POWERSHELL__\r?$'); if(-not $marker.Success){Write-Error 'Falta el compilador incluido en este BAT.'; exit 1}; & ([scriptblock]::Create($text.Substring($marker.Index+$marker.Length)))"
exit /b %ERRORLEVEL%

:CALCULAR_DURACION
set "VENTAS_FIN=%TIME: =0%"
set /a "VENTAS_INICIO_CS=((1%VENTAS_INICIO:~0,2%-100)*3600+(1%VENTAS_INICIO:~3,2%-100)*60+(1%VENTAS_INICIO:~6,2%-100))*100+(1%VENTAS_INICIO:~9,2%-100)" >nul
set /a "VENTAS_FIN_CS=((1%VENTAS_FIN:~0,2%-100)*3600+(1%VENTAS_FIN:~3,2%-100)*60+(1%VENTAS_FIN:~6,2%-100))*100+(1%VENTAS_FIN:~9,2%-100)" >nul
set /a "VENTAS_TOTAL_CS=VENTAS_FIN_CS-VENTAS_INICIO_CS" >nul
if %VENTAS_TOTAL_CS% lss 0 set /a "VENTAS_TOTAL_CS+=8640000" >nul
set /a "VENTAS_SEGUNDOS=VENTAS_TOTAL_CS/100, VENTAS_DECIMAS=(VENTAS_TOTAL_CS%%100)/10" >nul
set "VENTAS_DURACION=%VENTAS_SEGUNDOS%.%VENTAS_DECIMAS% s"
exit /b 0

:__VENTAS_POWERSHELL__
$ErrorActionPreference = 'Stop'
$repo = $env:VENTAS_REPO_ROOT
$msbuild = $env:VENTAS_MSBUILD
$configuration = $env:VENTAS_CONFIG
$nodes = $env:VENTAS_MSBUILD_NODOS
$codeRoot = (Get-Item -LiteralPath (Join-Path $repo 'codigo')).FullName
$allSolutions = @(Get-ChildItem -LiteralPath $codeRoot -Filter '*.sln' -File -Recurse |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj|packages|\.git|\.vs)[\\/]' } |
    Sort-Object FullName)
if ($allSolutions.Count -eq 0) {
    Write-Host '[ERROR] No se encontraron soluciones .sln dentro de codigo.'
    exit 1
}

$records = New-Object 'System.Collections.Generic.List[object]'
foreach ($solution in $allSolutions) {
    $relative = $solution.FullName.Substring($codeRoot.Length).TrimStart([char[]]@('\', '/'))
    $parts = $relative -split '[\\/]'
    if ($parts.Count -lt 3) { continue }
    $family = $parts[0]
    $area = $parts[1]
    $isExecution = ($solution.BaseName -match '(?i)^Ejecucion_' -or $solution.Directory.Name -match '(?i)^Ejecucion_')
    $isSecurity = ($family -ieq 'componentes' -and $area -ieq 'seguridad')
    $projectFiles = @([regex]::Matches([IO.File]::ReadAllText($solution.FullName), '(?im)^Project\("\{[^}]+\}"\)\s*=\s*"[^"]+"\s*,\s*"([^"]+\.csproj)"') | ForEach-Object {
        $path = [IO.Path]::GetFullPath([IO.Path]::Combine($solution.DirectoryName, $_.Groups[1].Value.Replace('/', '\')))
        if (Test-Path -LiteralPath $path -PathType Leaf) { $path }
    })
    $records.Add([pscustomobject]@{
        Path = $solution.FullName
        DirectoryName = $solution.DirectoryName
        Name = $solution.Name
        Family = $family
        Area = $area
        ProjectFiles = $projectFiles
        Dependencies = New-Object 'System.Collections.Generic.HashSet[string]' ([StringComparer]::OrdinalIgnoreCase)
        IsExecution = $isExecution
        IsSecurity = $isSecurity
        State = 'Pendiente'
        LastCode = 0
    })
}

$projectOwners = @{}
$assemblyOwners = @{}
foreach ($record in $records) {
    foreach ($projectPath in $record.ProjectFiles) {
        $projectOwners[$projectPath] = $record
        try {
            [xml]$projectXml = [IO.File]::ReadAllText($projectPath)
            $node = $projectXml.SelectSingleNode("/*[local-name()='Project']/*[local-name()='PropertyGroup']/*[local-name()='AssemblyName']")
            $assembly = [IO.Path]::GetFileNameWithoutExtension($projectPath)
            if ($node) { $assembly = $node.InnerText.Trim() }
            if (-not $assemblyOwners.ContainsKey($assembly)) { $assemblyOwners[$assembly] = New-Object 'System.Collections.Generic.List[object]' }
            $assemblyOwners[$assembly].Add([pscustomobject]@{ Path=$projectPath; Directory=[IO.Path]::GetDirectoryName($projectPath); Record=$record })
        } catch { }
    }
}
foreach ($record in $records) {
    foreach ($projectPath in $record.ProjectFiles) {
        try { [xml]$projectXml = [IO.File]::ReadAllText($projectPath) } catch { continue }
        foreach ($reference in $projectXml.SelectNodes("//*[local-name()='ProjectReference']")) {
            $include = $reference.GetAttribute('Include').Replace('$(Configuration)', $configuration).Replace('$(Platform)', 'AnyCPU')
            if ($include -match '\$\(') { continue }
            $path = [IO.Path]::GetFullPath([IO.Path]::Combine([IO.Path]::GetDirectoryName($projectPath), $include))
            if ($projectOwners.ContainsKey($path) -and $projectOwners[$path].Path -ne $record.Path) { [void]$record.Dependencies.Add($projectOwners[$path].Path) }
        }
        foreach ($reference in $projectXml.SelectNodes("//*[local-name()='Reference']")) {
            $assembly = $reference.GetAttribute('Include').Split(',')[0].Trim()
            if (-not $assemblyOwners.ContainsKey($assembly)) { continue }
            $candidates = @($assemblyOwners[$assembly] | Where-Object { $_.Path -ne $projectPath })
            $producers = @()
            $hint = $reference.SelectSingleNode("*[local-name()='HintPath']")
            if ($hint) {
                $hintText = $hint.InnerText.Replace('$(Configuration)', $configuration).Replace('$(Platform)', 'AnyCPU')
                if ($hintText -notmatch '\$\(') {
                    $hintPath = [IO.Path]::GetFullPath([IO.Path]::Combine([IO.Path]::GetDirectoryName($projectPath), $hintText))
                    $producers = @($candidates | Where-Object { $hintPath.StartsWith($_.Directory + '\bin\', [StringComparison]::OrdinalIgnoreCase) })
                }
            }
            if ($producers.Count -eq 0 -and $candidates.Count -eq 1) { $producers = $candidates }
            foreach ($producer in $producers) { if ($producer.Record.Path -ne $record.Path) { [void]$record.Dependencies.Add($producer.Record.Path) } }
        }
    }
}

function Get-OrderedSolutions([object[]]$Items) {
    $pending = @{}
    foreach ($item in $Items) { $pending[$item.Path] = $item }
    $ordered = New-Object 'System.Collections.Generic.List[object]'
    while ($pending.Count -gt 0) {
        $ready = @($pending.Values | Where-Object {
            $blocked = $false
            foreach ($dependency in $_.Dependencies) { if ($pending.ContainsKey($dependency)) { $blocked = $true; break } }
            -not $blocked
        } | Sort-Object Path)
        if ($ready.Count -eq 0) {
            $ready = @(@($pending.Values | Sort-Object Path)[0])
            Write-Host ('[AVISO] Ciclo de dependencias; se intenta una vez: {0}' -f $ready[0].Name)
        }
        $ordered.Add($ready[0])
        $pending.Remove($ready[0].Path)
    }
    return $ordered.ToArray()
}

function Invoke-SolutionPhase {
    param(
        [object[]]$Items,
        [string]$Phase
    )

    $pending = @($Items | Where-Object State -ne 'Correcto')
    if ($pending.Count -eq 0) { return }
    $orderedItems = @(Get-OrderedSolutions $pending)
    Write-Host ('===== {0}: {1} soluciones =====' -f $Phase, $orderedItems.Count)
    $currentArea = ''
    foreach ($item in $orderedItems) {
        $areaKey = $item.Family + '|' + $item.Area
        if ($areaKey -ne $currentArea) {
            $currentArea = $areaKey
            if ($item.Family -ieq 'componentes') { $headingColor = $env:VENTAS_AZUL; $heading = '[COMPONENTE] ' + $item.Area }
            else { $headingColor = $env:VENTAS_MORADO; $heading = '[MODULO] ' + $item.Area }
            Write-Host ($headingColor + $heading + $env:VENTAS_RESET)
        }
        Write-Host ('  [COMPILAR] {0}' -f $item.Name)
            $arguments = @(
                $item.Path,
                ('/m:' + $nodes),
                '/t:Build',
                ('/p:Configuration=' + $configuration),
                '/p:GeneratePackageOnBuild=false',
                '/p:PackageOnBuild=false',
                '/p:RestoreDuringBuild=false',
                '/nologo',
                '/v:q'
            )
            if ($item.IsSecurity -or $item.Name -match '(?i)(PagosProveedores|EstadosFinancieros)') {
                $moduleViews = @(Get-ChildItem -LiteralPath (Join-Path $codeRoot 'modulos') -Filter 'CapaVista_*.csproj' -File -Recurse |
                    ForEach-Object { Join-Path $_.DirectoryName ('bin\' + $configuration) } |
                    Sort-Object -Unique)
                $reportViewer = Join-Path $env:USERPROFILE '.nuget\packages\microsoft.reportingservices.reportviewercontrol.winforms\150.1652.0\lib\net40'
                if (Test-Path -LiteralPath $reportViewer -PathType Container) { $moduleViews += $reportViewer }
                $referenceDirectories = $moduleViews -join '%3B'
                $ventasTargets = Join-Path $repo 'codigo\modulos\ventas\Compilacion_Ventas\build_componentes_ventas.targets'
                $arguments += ('/p:CustomBeforeMicrosoftCommonTargets=' + $ventasTargets)
                $arguments += ('/p:VentasDllCompartidas=' + $referenceDirectories)
            }
            & $msbuild @arguments 2>&1 | ForEach-Object { Write-Host $_ }
            $exitCode = $LASTEXITCODE
            if ($exitCode -eq 0) {
                $item.State = 'Correcto'
                $item.LastCode = 0
                Write-Host ('  [OK] {0}' -f $item.Name)
            } else {
                $item.State = 'Error'
                $item.LastCode = $exitCode
                Write-Host ('  [ERROR {0}] {1}' -f $exitCode, $item.Name)
            }
    }
}

function Get-FileFingerprint([string]$Path) {
    $sha = [System.Security.Cryptography.SHA256]::Create()
    $stream = [IO.File]::OpenRead($Path)
    try { return [BitConverter]::ToString($sha.ComputeHash($stream)) }
    finally { $stream.Dispose(); $sha.Dispose() }
}

function New-ProvisionalRrhhView {
    $project = Join-Path $codeRoot 'modulos\rrhh\Bibliotecas de clase RRHH\Recursos Humanos\CapaVista_Rrhh\CapaVista_Rrhh.csproj'
    $dll = Join-Path (Split-Path -Parent $project) ('bin\' + $configuration + '\CapaVista_Rrhh.dll')
    $marker = $dll + '.ventas-provisional'
    if (Test-Path -LiteralPath $marker -PathType Leaf) {
        Remove-Item -LiteralPath $marker -Force
        Remove-Item -LiteralPath $dll -Force -ErrorAction SilentlyContinue
    }
    if (Test-Path -LiteralPath $dll -PathType Leaf) { return }
    [void][IO.Directory]::CreateDirectory((Split-Path -Parent $dll))
    $source = @'
using System.Reflection;
[assembly: AssemblyVersion("1.0.0.0")]
namespace CapaVista_Rrhh.Formularios { public class FrmMDIRrhh : System.Windows.Forms.Form { public void RrhhMetActualizarInfoUsuario(string Nombre, string Rol) { } } }
namespace CapaVista_Rrhh { internal class __ReferenciaCompilacionTemporal { } }
'@
    $formsAssembly = [System.Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms')
    if (-not $formsAssembly) { throw 'No se pudo cargar System.Windows.Forms para resolver el ciclo de RRHH.' }
    Add-Type -TypeDefinition $source -ReferencedAssemblies @($formsAssembly.Location) -OutputAssembly $dll -OutputType Library
    $script:provisionalRrhhDll = $dll
    $script:provisionalRrhhMarker = $marker
    [IO.File]::WriteAllText($marker, 'Referencia temporal generada por build_ventas.bat')
    $script:provisionalRrhhHash = Get-FileFingerprint $dll
    Write-Host '[AJUSTE] Referencia temporal RRHH para compilar Seguridad sin repetir soluciones.'
}

function Remove-ProvisionalRrhhView {
    if (-not $script:provisionalRrhhDll) { return }
    Get-ChildItem -LiteralPath (Join-Path $codeRoot 'componentes\seguridad') -Filter 'CapaVista_Rrhh.dll' -File -Recurse -ErrorAction SilentlyContinue | ForEach-Object {
        try { if ((Get-FileFingerprint $_.FullName) -eq $script:provisionalRrhhHash) { Remove-Item -LiteralPath $_.FullName -Force } } catch { }
    }
    Remove-Item -LiteralPath $script:provisionalRrhhDll -Force -ErrorAction SilentlyContinue
    Remove-Item -LiteralPath $script:provisionalRrhhMarker -Force -ErrorAction SilentlyContinue
}

function Copy-RrhhViewIntoSecurityOutput {
    $dll = Join-Path $codeRoot ('modulos\rrhh\Bibliotecas de clase RRHH\Recursos Humanos\CapaVista_Rrhh\bin\' + $configuration + '\CapaVista_Rrhh.dll')
    $securityOutput = Join-Path $codeRoot ('componentes\seguridad\Seguridad\CapaVista_Seguridad\bin\' + $configuration)
    if ((Test-Path -LiteralPath $dll -PathType Leaf) -and (Test-Path -LiteralPath $securityOutput -PathType Container)) {
        Copy-Item -LiteralPath $dll -Destination (Join-Path $securityOutput 'CapaVista_Rrhh.dll') -Force
    }
}

$componentesBase = @($records | Where-Object { $_.Family -ieq 'componentes' -and -not $_.IsSecurity -and -not $_.IsExecution })
$modulosBase = @($records | Where-Object { $_.Family -ieq 'modulos' -and $_.Area -ine 'rrhh' -and -not $_.IsExecution })
$seguridad = @($records | Where-Object { $_.IsSecurity -and -not $_.IsExecution })
$modulosRrhh = @($records | Where-Object { $_.Family -ieq 'modulos' -and $_.Area -ieq 'rrhh' -and -not $_.IsExecution })
$ejecutables = @($records | Where-Object IsExecution)

Invoke-SolutionPhase $componentesBase 'Componentes base'
Invoke-SolutionPhase $modulosBase 'Modulos excepto RRHH'
if ($seguridad.Count -gt 0) { New-ProvisionalRrhhView }
Invoke-SolutionPhase $seguridad 'Seguridad (despues de compilar los modulos)'
Remove-ProvisionalRrhhView
Invoke-SolutionPhase $modulosRrhh 'Modulo RRHH (despues de Seguridad)'
Copy-RrhhViewIntoSecurityOutput
Invoke-SolutionPhase $ejecutables 'Ejecutables y soluciones de ejecucion'

Write-Host ''
Write-Host '===== RESUMEN POR MODULO Y COMPONENTE ====='
foreach ($family in @('componentes', 'modulos')) {
    $familyRoot = Join-Path $codeRoot $family
    if (-not (Test-Path -LiteralPath $familyRoot -PathType Container)) { continue }
    foreach ($area in (Get-ChildItem -LiteralPath $familyRoot -Directory | Sort-Object Name)) {
        $areaRecords = @($records | Where-Object { $_.Family -ieq $family -and $_.Area -ieq $area.Name })
        if ($family -eq 'componentes') {
            $headingColor = $env:VENTAS_AZUL
            $heading = '[COMPONENTE] ' + $area.Name
        } else {
            $headingColor = $env:VENTAS_MORADO
            $heading = '[MODULO] ' + $area.Name
        }
        Write-Host ($headingColor + $heading + $env:VENTAS_RESET)
        if ($areaRecords.Count -eq 0) {
            Write-Host '  [SIN SOLUCIONES]'
            continue
        }
        $compiled = @($areaRecords | Where-Object State -eq 'Correcto').Count
        if ($compiled -eq $areaRecords.Count) {
            Write-Host ('  [OK] {0}/{1} soluciones compiladas' -f $compiled, $areaRecords.Count)
        } else {
            Write-Host ('  [PENDIENTE] {0}/{1} soluciones compiladas' -f $compiled, $areaRecords.Count)
            foreach ($failed in ($areaRecords | Where-Object State -ne 'Correcto')) {
                Write-Host ('  [ERROR {0}] {1}' -f $failed.LastCode, $failed.Name)
            }
        }
    }
}

$totalCompiled = @($records | Where-Object State -eq 'Correcto').Count
$totalFailed = $records.Count - $totalCompiled
if ($totalFailed -gt 0) {
    Write-Host ('[ERROR] Total compilado: {0}/{1}; quedan {2} soluciones pendientes.' -f $totalCompiled, $records.Count, $totalFailed)
    exit 1
}
Write-Host ('[OK] Total: {0}/{1} soluciones de modulos y componentes compiladas.' -f $totalCompiled, $records.Count)
exit 0
