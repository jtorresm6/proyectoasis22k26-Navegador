@echo off
setlocal EnableExtensions DisableDelayedExpansion
:: Ventas y componentes: compilacion incremental y paralela por dependencias.
:: Uso: build_ventas.bat [Debug|Release] [/sinpausa]
:: Las rutas se calculan desde este archivo; no dependen de la carpeta del equipo.

set "VENTAS_ROOT=%~dp0"
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

:: Colores propios de Ventas. Sin consola compatible se utiliza texto normal.
:: /sinpausa no cambia el titulo, no limpia la pantalla y no emite codigos ANSI.
set "VENTAS_ESC="
set "VENTAS_PANEL="
set "VENTAS_TITULO="
set "VENTAS_MORADO="
set "VENTAS_DORADO="
set "VENTAS_VERDE="
set "VENTAS_ROJO="
set "VENTAS_GRIS="
set "VENTAS_RESET="
if "%VENTAS_SINPAUSA%"=="1" goto COMPILAR
title VentasYCxC - Centro de compilacion
:: Habilitar ANSI solamente si existe una consola real, sin modificar el registro.
for /f "usebackq delims=" %%E in (`powershell.exe -NoLogo -NoProfile -NonInteractive -Command "$ErrorActionPreference='Stop'; try { $q=[char]34; $d='[System.Runtime.InteropServices.DllImport('+$q+'kernel32.dll'+$q+')] public static extern System.IntPtr CreateFile(string n, uint a, uint s, System.IntPtr p, uint c, uint f, System.IntPtr t); [System.Runtime.InteropServices.DllImport('+$q+'kernel32.dll'+$q+')] public static extern bool GetConsoleMode(System.IntPtr h, out uint m); [System.Runtime.InteropServices.DllImport('+$q+'kernel32.dll'+$q+')] public static extern bool SetConsoleMode(System.IntPtr h, uint m); [System.Runtime.InteropServices.DllImport(' + $q + 'kernel32.dll' + $q + ')] public static extern bool CloseHandle(System.IntPtr h);'; $t=Add-Type -Namespace VentasBuild -Name ConsoleNative -MemberDefinition $d -PassThru; $h=$t::CreateFile('CONOUT$',3221225472,3,[IntPtr]::Zero,3,0,[IntPtr]::Zero); [uint32]$m=0; if($t::GetConsoleMode($h,[ref]$m)) { if($t::SetConsoleMode($h,($m -bor 4))) { [Console]::Write([char]27) } }; [void]$t::CloseHandle($h) } catch {}" 2^>nul`) do set "VENTAS_ESC=%%E"
if not defined VENTAS_ESC goto COMPILAR
set "VENTAS_PANEL=%VENTAS_ESC%[48;2;59;29;74m%VENTAS_ESC%[38;2;228;212;234m"
set "VENTAS_TITULO=%VENTAS_ESC%[48;2;59;29;74m%VENTAS_ESC%[38;2;201;162;74m%VENTAS_ESC%[1m"
set "VENTAS_MORADO=%VENTAS_ESC%[38;2;183;133;205m"
set "VENTAS_DORADO=%VENTAS_ESC%[38;2;201;162;74m"
set "VENTAS_VERDE=%VENTAS_ESC%[38;2;112;207;155m"
set "VENTAS_ROJO=%VENTAS_ESC%[38;2;244;126;139m"
set "VENTAS_GRIS=%VENTAS_ESC%[38;2;164;164;178m"
set "VENTAS_RESET=%VENTAS_ESC%[0m"

:COMPILAR
set "VENTAS_RESULT=2"
set "VENTAS_DURACION=No iniciada"
set "VENTAS_LOG=%VENTAS_ROOT%logs\build_ventas_%VENTAS_CONFIG%.log"
set "VENTAS_SALIDA=%VENTAS_ROOT%..\..\componentes\seguridad\Ejecucion_Seguridad\Ejecucion_Seguridad\bin\%VENTAS_CONFIG%"
set "VENTAS_EJECUTABLE=%VENTAS_SALIDA%\Ejecucion_Seguridad.exe"
call :ENCABEZADO
echo   %VENTAS_DORADO%PERFIL%VENTAS_RESET%  %VENTAS_CONFIG%       %VENTAS_DORADO%MODO%VENTAS_RESET%  Paralelo + incremental
echo.
echo   %VENTAS_GRIS%Consultas / Navegador / Logistica / Seguridad / Reporteador + sus 8 botones
echo.
echo   Facturas                 Listado de precios        Cotizaciones
echo   Movimientos y pagos      Cierre de movimientos     Reportes
echo   Mantenimientos           Comisiones                Pedidos%VENTAS_RESET%
echo.
echo   %VENTAS_MORADO%[1/2]%VENTAS_RESET% Verificando herramientas...
call :BUSCAR_MSBUILD
if errorlevel 1 goto SIN_MSBUILD
if not exist "%VENTAS_ROOT%build_ventas.proj" goto SIN_PROYECTO
if not exist "%VENTAS_ROOT%build_componentes_ventas.proj" goto SIN_PROYECTO
if not exist "%VENTAS_ROOT%build_componentes_ventas.targets" goto SIN_PROYECTO
if not exist "%VENTAS_ROOT%logs" mkdir "%VENTAS_ROOT%logs"
if not exist "%VENTAS_ROOT%logs" goto SIN_LOGS
echo         MSBuild disponible.
echo   %VENTAS_MORADO%[2/2]%VENTAS_RESET% Compilando soluciones y reuniendo las DLL...
echo         %VENTAS_GRIS%Solo se recompilan los proyectos que cambiaron.%VENTAS_RESET%
echo         %VENTAS_GRIS%Paquetes incluidos en el repositorio. Sin descargas de NuGet.%VENTAS_RESET%
echo.
set "VENTAS_INICIO=%TIME: =0%"
pushd "%VENTAS_ROOT%"
if errorlevel 1 goto SIN_DIRECTORIO
"%VENTAS_MSBUILD%" "%VENTAS_ROOT%build_ventas.proj" /m /t:Compilar /p:Configuration=%VENTAS_CONFIG% /nologo /v:minimal /clp:Summary /fl "/flp:logfile=%VENTAS_LOG%;verbosity=normal"
set "VENTAS_RESULT=%ERRORLEVEL%"
popd
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

:SIN_LOGS
echo.
echo   %VENTAS_ROJO%[!] No se pudo preparar la carpeta de logs.%VENTAS_RESET%
goto RESUMEN

:SIN_DIRECTORIO
echo.
echo   %VENTAS_ROJO%[!] No se pudo abrir la carpeta de Ventas.%VENTAS_RESET%
goto RESUMEN

:RESUMEN
echo.
echo   %VENTAS_MORADO%------------------------------------------------------------------------%VENTAS_RESET%
if "%VENTAS_RESULT%"=="0" (
    echo   %VENTAS_VERDE%[OK] COMPILACION COMPLETADA%VENTAS_RESET%
    echo        Ventas y componentes listos. DLL actualizadas para el MDI.
) else (
    echo   %VENTAS_ROJO%[!] COMPILACION DETENIDA%VENTAS_RESET%
    echo        Revise el mensaje anterior. Codigo de salida: %VENTAS_RESULT%
)
echo.
echo   Perfil:  %VENTAS_CONFIG%        Tiempo: %VENTAS_DURACION%
echo.
echo   %VENTAS_DORADO%APLICACION%VENTAS_RESET%
echo   ..\..\componentes\seguridad\Ejecucion_Seguridad\Ejecucion_Seguridad\bin\%VENTAS_CONFIG%\Ejecucion_Seguridad.exe
echo.
echo   %VENTAS_DORADO%LOG DE COMPILACION%VENTAS_RESET%
echo   logs\build_ventas_%VENTAS_CONFIG%.log
echo   %VENTAS_MORADO%------------------------------------------------------------------------%VENTAS_RESET%
if "%VENTAS_SINPAUSA%"=="1" goto SALIR

:MENU
echo.
echo   %VENTAS_DORADO%[R]%VENTAS_RESET% Repetir compilacion     %VENTAS_DORADO%[C]%VENTAS_RESET% Cambiar Debug / Release
if "%VENTAS_RESULT%"=="0" echo   %VENTAS_DORADO%[A]%VENTAS_RESET% Abrir Seguridad
echo   %VENTAS_DORADO%[L]%VENTAS_RESET% Ver log                 %VENTAS_DORADO%[S]%VENTAS_RESET% Salir
echo.
choice /c RCALS /n /m "  Elige una opcion: "
if errorlevel 255 goto SALIR
if errorlevel 5 goto SALIR
if errorlevel 4 goto VER_LOG
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
    echo   %VENTAS_ROJO%No se encontro Ejecucion_Seguridad.exe. Vuelva a compilar.%VENTAS_RESET%
    goto MENU
)
start "" /d "%VENTAS_SALIDA%" "%VENTAS_EJECUTABLE%"
goto MENU

:VER_LOG
if not exist "%VENTAS_LOG%" (
    echo.
    echo   %VENTAS_GRIS%Todavia no hay un log para este perfil.%VENTAS_RESET%
    goto MENU
)
start "" notepad.exe "%VENTAS_LOG%"
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

:CALCULAR_DURACION
set "VENTAS_FIN=%TIME: =0%"
set /a "VENTAS_INICIO_CS=((1%VENTAS_INICIO:~0,2%-100)*3600+(1%VENTAS_INICIO:~3,2%-100)*60+(1%VENTAS_INICIO:~6,2%-100))*100+(1%VENTAS_INICIO:~9,2%-100)" >nul
set /a "VENTAS_FIN_CS=((1%VENTAS_FIN:~0,2%-100)*3600+(1%VENTAS_FIN:~3,2%-100)*60+(1%VENTAS_FIN:~6,2%-100))*100+(1%VENTAS_FIN:~9,2%-100)" >nul
set /a "VENTAS_TOTAL_CS=VENTAS_FIN_CS-VENTAS_INICIO_CS" >nul
if %VENTAS_TOTAL_CS% lss 0 set /a "VENTAS_TOTAL_CS+=8640000" >nul
set /a "VENTAS_SEGUNDOS=VENTAS_TOTAL_CS/100, VENTAS_DECIMAS=(VENTAS_TOTAL_CS%%100)/10" >nul
set "VENTAS_DURACION=%VENTAS_SEGUNDOS%.%VENTAS_DECIMAS% s"
exit /b 0
