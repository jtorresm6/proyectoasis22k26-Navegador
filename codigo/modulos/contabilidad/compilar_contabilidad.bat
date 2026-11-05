:: ============================================================================
:: Area   : Contabilidad
:: Autor  : Gabriel Andre Guillen Pocon
:: Carne  : 0901-23-1998
:: Fecha  : 25/10/2026
:: ============================================================================
:: Compila, en orden, solo lo necesario para abrir Contabilidad:
::   Seguridad (modelo y controlador), Consultas, Reporteador, Navegador,
::   base de Contabilidad, las 5 soluciones, el MDI y Ejecucion_Contabilidad.
:: Se detiene en el primer error.
:: Funciona desde cualquier carpeta: busca el repositorio en la ruta que se le
:: pase, subiendo desde el bat o desde la carpeta actual, y en C:\proyectoasis22k26.
:: Si no lo encuentra, pide la ruta.
:: ============================================================================
@echo off
setlocal EnableDelayedExpansion
chcp 65001 >nul

set "MARCA=codigo\modulos\contabilidad\Contabilidad\Contabilidad.sln"
set "RAIZ="
if not "%~1"=="" if exist "%~f1\%MARCA%" set "RAIZ=%~f1"
if not defined RAIZ call :subir "%~dp0."
if not defined RAIZ call :subir "%CD%"
if not defined RAIZ if exist "C:\proyectoasis22k26\%MARCA%" set "RAIZ=C:\proyectoasis22k26"
if not defined RAIZ for /d %%D in ("C:\proyectoasis22k26\*") do (
    if not defined RAIZ if exist "%%~fD\%MARCA%" set "RAIZ=%%~fD"
)
:pedir
if not defined RAIZ (
    set /p "RUTA=No se encontro el repositorio. Escriba su ruta: "
    if "!RUTA!"=="" exit /b 1
    for %%I in ("!RUTA!") do if exist "%%~fI\%MARCA%" set "RAIZ=%%~fI"
    goto :pedir
)

set "C=%RAIZ%\codigo\componentes"
set "M=%RAIZ%\codigo\modulos\contabilidad"

set "MSB=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
if not exist "%MSB%" (
    for /f "usebackq delims=" %%P in (`"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "MSB=%%P"
)
if not exist "%MSB%" (
    echo No se encontro MSBuild de Visual Studio 2022.
    pause
    exit /b 1
)

echo Repositorio: %RAIZ%
echo.

call :compilar "%C%\seguridad\Seguridad\CapaModelo_Seguridad\CapaModelo_Seguridad.csproj" || goto :fallo
call :compilar "%C%\seguridad\Seguridad\CapaControlador_Seguridad\CapaControlador_Seguridad.csproj" || goto :fallo

for %%K in (CapaModelo CapaControlador CapaVista) do (
    call :compilar "%C%\consultas\Consultas\%%K_Consultas\%%K_Consultas.csproj" || goto :fallo
)

for %%B in (BtnActualizar BtnEditar BtnEliminar BtnGuardar BtnLimpiar BtnRuta BtnVerReporte) do (
    for %%K in (CapaModelo CapaControlador CapaVista) do (
        call :compilar "%C%\reporteador\%%B_Reporteador\%%K_%%B_Reporteador\%%K_%%B_Reporteador.csproj" || goto :fallo
    )
)
for %%K in (CapaModelo CapaControlador CapaVista) do (
    call :compilar "%C%\reporteador\BtnBusqueda_Reporteador\%%K_BtnBusqueda\%%K_BtnBusqueda_Reporteador.csproj" || goto :fallo
)
for %%K in (CapaModelo CapaControlador CapaVista) do (
    call :compilar "%C%\reporteador\Reporteador\%%K_Reporteador\%%K_Reporteador.csproj" || goto :fallo
)

for %%K in (CapaModelo CapaControlador CapaVista) do (
    call :compilar "%C%\navegador\Navegador\%%K_Navegador\%%K_Navegador.csproj" || goto :fallo
)

call :compilar "%M%\Contabilidad\CapaModelo_Contabilidad\CapaModelo_Contabilidad.csproj" || goto :fallo
call :compilar "%M%\Contabilidad\CapaControlador_Contabilidad\CapaControlador_Contabilidad.csproj" || goto :fallo

for %%S in (PolizasCuentas Presupuestos ActivosFijos CierreContable EstadosFinancieros) do (
    for %%K in (CapaModelo CapaControlador CapaVista) do (
        call :compilar "%M%\%%S_Contabilidad\%%K_%%S_Contabilidad\%%K_%%S_Contabilidad.csproj" || goto :fallo
    )
)

call :compilar "%M%\Contabilidad\CapaVista_Contabilidad\CapaVista_Contabilidad.csproj" || goto :fallo
call :compilar "%M%\Ejecucion_Contabilidad\Ejecucion_Contabilidad\Ejecucion_Contabilidad.csproj" || goto :fallo

echo.
echo Listo. Ejecutable:
echo %M%\Ejecucion_Contabilidad\Ejecucion_Contabilidad\bin\Debug\Ejecucion_Contabilidad.exe
echo.
pause
exit /b 0

:compilar
for %%F in ("%~dp1..") do set "SOL=%%~fF\\"
"%MSB%" "%~1" -restore -p:RestorePackagesConfig=true "-p:SolutionDir=!SOL!" -p:Configuration=Debug -v:quiet -nologo >"%TEMP%\compilar_contabilidad.log" 2>&1
if errorlevel 1 (
    echo [ERROR] %~n1
    findstr /c:": error " "%TEMP%\compilar_contabilidad.log"
    exit /b 1
)
echo [OK]    %~n1
exit /b 0

:subir
set "D=%~f1"
:subir_siguiente
if exist "%D%\%MARCA%" (
    set "RAIZ=%D%"
    exit /b 0
)
for %%P in ("%D%\..") do set "P=%%~fP"
if /i "%P%"=="%D%" exit /b 0
set "D=%P%"
goto :subir_siguiente

:fallo
echo.
echo La compilacion se detuvo por un error.
pause
exit /b 1
