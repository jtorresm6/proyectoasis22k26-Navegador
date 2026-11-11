@echo off
:: BUILD RRHH: Consultas, Navegador, Reporteador, Seguridad, Logistica y RRHH.
:: Descomprimir el ZIP y colocar este .bat en la raiz del repositorio.
:: Doble clic: compilar con menu. /auto: terminar con codigo 0 o 1.
:: Requiere Visual Studio/Build Tools y desarrollo de escritorio .NET.
:: Compila Debug, restaura NuGet y reintenta hasta tres rondas.
:: Archivos auxiliares temporales; se eliminan al terminar.
:: Conserva copias Debug en bin\Release para referencias heredadas.
:: MSBUILD_MANUAL puede indicar una ruta alternativa a MSBuild.exe.
:: Modificado por: Byron Alexander Chiquito Paredes 0901-23-3747 (02/11/2026)
:: Omite los proyectos MSTest, solo restaura NuGet donde hay paquetes,
:: las rondas 2 y 3 solo repiten lo que fallo y el menu final abre el login.
setlocal EnableExtensions DisableDelayedExpansion
set "LOG_DIR="
chcp 65001 >nul
color 0A
title Compilacion global - Consultas, Navegador, Seguridad, Logistica y RRHH

:: --------------------------------------------------------------------------
:: OPCIONES (normalmente NO hay que tocar nada)
:: --------------------------------------------------------------------------
:: Dejar vacio para detectar MSBuild automaticamente. Solo si la deteccion
:: falla, escribir la ruta completa de MSBuild.exe, por ejemplo:
::   set "MSBUILD_MANUAL=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
:: MSBUILD_MANUAL puede definirse en el entorno.

set "TIPO_BUILD=Build"

:INICIO
set "PASO=0"
:: Limpia las marcas de proyectos compilados de una corrida anterior
for /f "delims==" %%V in ('set OK_ 2^>nul') do set "%%V="
set /a total=0
set /a ok=0
set /a fail=0
set /a P_OK=0
set /a P_FAIL=0

echo ============================================
echo COMPILACION GLOBAL - CONSULTAS, NAVEGADOR, SEGURIDAD, LOGISTICA, RRHH
echo ============================================

:: --------------------------------------------------------------------------
:: 1) CARPETA DE ESTE .BAT Y CARPETA DE LOGS
:: --------------------------------------------------------------------------
set "ROOT_DIR=%~dp0"
:: Las rutas se resuelven desde el .bat sin cambiar el directorio de la consola.
call :LimpiarTemporales
set "LOG_DIR=%TEMP%\asis_rrhh_%RANDOM%_%RANDOM%"
if not exist "%LOG_DIR%" mkdir "%LOG_DIR%"
if not exist "%LOG_DIR%\" goto FIN_ERROR
set "LOG_FILE=%LOG_DIR%\build_log.txt"
set "FALLIDOS=%LOG_DIR%\fallidos.txt"
set "FALL_PASS=%LOG_DIR%\fallidos_pasada.txt"
if exist "%FALLIDOS%" del "%FALLIDOS%"
if exist "%FALL_PASS%" del "%FALL_PASS%"
echo Compilacion iniciada: %date% %time% > "%LOG_FILE%" || goto FIN_ERROR

:: --------------------------------------------------------------------------
:: 2) LOCALIZAR LA CARPETA "componentes" DEL REPOSITORIO
:: --------------------------------------------------------------------------
set "COMPONENTES_DIR="
set "CUR=%~dp0"
if "%CUR:~-1%"=="\" set "CUR=%CUR:~0,-1%"

:BUSCAR_RAIZ
if exist "%CUR%\codigo\componentes\" (set "COMPONENTES_DIR=%CUR%\codigo\componentes" & goto RAIZ_LISTA)
if exist "%CUR%\componentes\" (set "COMPONENTES_DIR=%CUR%\componentes" & goto RAIZ_LISTA)
if exist "%CUR%\navegador\" if exist "%CUR%\seguridad\" (set "COMPONENTES_DIR=%CUR%" & goto RAIZ_LISTA)
for %%P in ("%CUR%\..") do set "PADRE=%%~fP"
if "%PADRE:~-1%"=="\" set "PADRE=%PADRE:~0,-1%"
if /I "%PADRE%"=="%CUR%" goto RAIZ_NO_ENCONTRADA
set "CUR=%PADRE%"
goto BUSCAR_RAIZ

:RAIZ_NO_ENCONTRADA
:: Solo se usa el repositorio que contiene este script.
echo.
echo [ERROR] No se encontro un repositorio con codigo\componentes y codigo\modulos\rrhh.
echo         Guarda este .bat dentro del repositorio clonado y vuelve a ejecutarlo.
echo         Carpeta desde la que se busco: %~dp0
goto FIN_ERROR

:RAIZ_LISTA
for %%i in ("%COMPONENTES_DIR%\..") do set "CODIGO_DIR=%%~fi"
for %%i in ("%CODIGO_DIR%\..") do set "REPO_ROOT=%%~fi"
set "MODULOS_DIR=%CODIGO_DIR%\modulos"
if not exist "%MODULOS_DIR%\rrhh\" goto RAIZ_NO_ENCONTRADA
echo Repositorio (componentes): %COMPONENTES_DIR%

:: --------------------------------------------------------------------------
:: 3) LOCALIZAR MSBUILD (CUALQUIER EDICION DE VISUAL STUDIO)
:: --------------------------------------------------------------------------
set "MSBUILD_PATH="
if defined MSBUILD_MANUAL if exist "%MSBUILD_MANUAL%" set "MSBUILD_PATH=%MSBUILD_MANUAL%"
if defined MSBUILD_PATH goto MSBUILD_LISTO

:: 3a) vswhere (herramienta oficial de Microsoft). La salida se guarda en un
:: archivo temporal para evitar problemas de comillas en el bucle FOR.
set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
if not exist "%VSWHERE%" goto MSBUILD_FALLBACK
set "VSW_OUT=%LOG_DIR%\vswhere_msbuild.txt"

:: Primero Visual Studio 2022 (version 17).
if exist "%VSW_OUT%" del "%VSW_OUT%"
"%VSWHERE%" -version "[17.0,18.0)" -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > "%VSW_OUT%" 2>nul
if exist "%VSW_OUT%" set /p MSBUILD_PATH=<"%VSW_OUT%"
if defined MSBUILD_PATH if not exist "%MSBUILD_PATH%" set "MSBUILD_PATH="
if defined MSBUILD_PATH goto MSBUILD_LISTO

:: Si no hay 2022, la version mas reciente instalada.
if exist "%VSW_OUT%" del "%VSW_OUT%"
"%VSWHERE%" -latest -products * -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe > "%VSW_OUT%" 2>nul
if exist "%VSW_OUT%" set /p MSBUILD_PATH=<"%VSW_OUT%"
if defined MSBUILD_PATH if not exist "%MSBUILD_PATH%" set "MSBUILD_PATH="
if defined MSBUILD_PATH goto MSBUILD_LISTO

:MSBUILD_FALLBACK
:: 3b) Respaldo: rutas habituales de cada edicion.
for %%V in (2022 2019) do for %%E in (Enterprise Professional Community BuildTools) do if not defined MSBUILD_PATH if exist "%ProgramFiles%\Microsoft Visual Studio\%%V\%%E\MSBuild\Current\Bin\MSBuild.exe" set "MSBUILD_PATH=%ProgramFiles%\Microsoft Visual Studio\%%V\%%E\MSBuild\Current\Bin\MSBuild.exe"
if defined MSBUILD_PATH goto MSBUILD_LISTO

echo.
echo [ERROR] No se encontro MSBuild. Instala Visual Studio 2022 con la carga de
echo         trabajo "Desarrollo de escritorio con .NET", o escribe la ruta de
echo         MSBuild.exe en la variable MSBUILD_MANUAL al inicio de este archivo.
goto FIN_ERROR

:MSBUILD_LISTO
echo MSBuild: %MSBUILD_PATH%
echo MSBuild: %MSBUILD_PATH% >> "%LOG_FILE%"

:: Reintentos para dependencias entre proyectos.
set "FALL_PREV=%LOG_DIR%\fallidos_prev.txt"
if exist "%FALL_PREV%" del "%FALL_PREV%"
set "RONDA=1"

:RONDA
set /a IDX=0
set /a total=0
set /a ok=0
set /a fail=0
if exist "%FALLIDOS%" del "%FALLIDOS%"
set /a P_OK=0, P_FAIL=0, PASADA=1
if exist "%FALL_PASS%" del "%FALL_PASS%"
set "INCLUIR_EXE=0"
for %%C in (seguridad consultas navegador reporteador) do call :Bases "%COMPONENTES_DIR%\%%C"
call :Bases "%MODULOS_DIR%\rrhh"
call :Bases "%MODULOS_DIR%\Logistica"
for %%C in (consultas reporteador navegador) do call :Vistas "%COMPONENTES_DIR%\%%C"
:: Logistica: primero sus vistas y al final su MDI (carpeta Logistica)
for /d %%D in ("%MODULOS_DIR%\Logistica\*") do if /i not "%%~nxD"=="Logistica" call :Vistas "%%~fD"
call :Vistas "%MODULOS_DIR%\Logistica\Logistica"
:: Seguridad <-> RRHH forman un ciclo: pasada 0 de Seguridad (sin MDI RRHH) antes de las vistas RRHH
call :SeguridadBootstrap
for /d %%D in ("%MODULOS_DIR%\rrhh\Bibliotecas de clase RRHH\*") do if /i not "%%~nxD"=="Recursos Humanos" call :Vistas "%%~fD"
call :Vistas "%MODULOS_DIR%\rrhh\Bibliotecas de clase RRHH\Recursos Humanos"
:: La vista de Seguridad completa va al final: el Splash usa los MDI de Logistica y RRHH
call :SeguridadFinal
set "INCLUIR_EXE=1"
for %%C in (consultas reporteador navegador seguridad) do call :Ejecutables "%COMPONENTES_DIR%\%%C"
call :Ejecutables "%MODULOS_DIR%\rrhh\Bibliotecas de clase RRHH"
call :Ejecutables "%MODULOS_DIR%\Logistica"
set /a total=P_OK+P_FAIL, ok=P_OK, fail=P_FAIL
if exist "%FALL_PASS%" type "%FALL_PASS%" >> "%FALLIDOS%"
if %fail% EQU 0 goto RONDA_FIN
if %RONDA% GEQ 3 goto RONDA_FIN
if exist "%FALL_PREV%" (
    fc /b "%FALLIDOS%" "%FALL_PREV%" >nul 2>&1
    if not errorlevel 1 goto RONDA_SIN_PROGRESO
)
copy /y "%FALLIDOS%" "%FALL_PREV%" >nul 2>&1
set /a RONDA+=1
echo.
echo ============================================
echo Hubo errores. Se repite todo el ciclo: RONDA %RONDA% de 3
echo (navegador y seguridad dependen entre si)
echo ============================================
goto RONDA

:RONDA_SIN_PROGRESO
echo.
echo Los mismos proyectos siguen fallando que en la ronda anterior:
echo no se repite mas. Revisa los errores de abajo.

:RONDA_FIN

echo.
echo ============================================
echo COMPILACION GLOBAL FINALIZADA
echo Total de proyectos: %total%
echo Correctos: %ok%
echo Con errores: %fail%
echo Los archivos auxiliares se eliminan al terminar.
echo ============================================
if exist "%FALLIDOS%" echo.
if exist "%FALLIDOS%" echo PROYECTOS QUE NO COMPILARON:
if exist "%FALLIDOS%" type "%FALLIDOS%"
if exist "%FALLIDOS%" echo (revise los errores mostrados arriba)

call :LimpiarTemporales
:: --------------------------------------------------------------------------
if /I "%~1"=="/auto" goto FIN_OK

:MENU
echo.
echo Presiona una opcion:
echo [L] Abrir el login de Seguridad
echo [R] Recompilar
echo [C] Compilar todo desde cero
echo [S] Salir
choice /c LRCS /n /m "Seleccion: "
if errorlevel 4 goto FIN_OK
if errorlevel 3 (set "TIPO_BUILD=Rebuild" & goto INICIO)
if errorlevel 2 (set "TIPO_BUILD=Build" & goto INICIO)
if errorlevel 1 goto ABRIR_LOGIN
goto MENU

:ABRIR_LOGIN
set "LOGIN_EXE=%COMPONENTES_DIR%\seguridad\Ejecucion_Seguridad\Ejecucion_Seguridad\bin\Debug\Ejecucion_Seguridad.exe"
if not exist "%LOGIN_EXE%" (
    echo [ERROR] No se encontro Ejecucion_Seguridad.exe. Compila primero.
    goto MENU
)
for %%i in ("%LOGIN_EXE%") do start "" /d "%%~dpi" "%%~fi"
echo Login de Seguridad abierto.
goto MENU

:FIN_OK
set "EXIT_CODE=0"
if %fail% GTR 0 set "EXIT_CODE=1"
if %total% EQU 0 set "EXIT_CODE=1"
call :LimpiarTemporales
endlocal & exit /b %EXIT_CODE%

:FIN_ERROR
call :LimpiarTemporales
echo.
if /I not "%~1"=="/auto" pause
endlocal
exit /b 1


:: ==========================================================================
:CompilarNoCapa
echo "%~nx1"| findstr /i /c:"CapaModelo_" /c:"CapaControlador_" /c:"CapaVista_" >nul && exit /b 0
echo "%~1"| findstr /i /l /c:"\packages" >nul && exit /b 0
type "%~1" | findstr /i /c:"<OutputType>Library</OutputType>" >nul
if errorlevel 1 (set "ES_LIB=0") else (set "ES_LIB=1")
if "%~2"=="LIB" if "%ES_LIB%"=="0" exit /b 0
if "%~2"=="EXE" if "%ES_LIB%"=="1" exit /b 0
if "%~2"=="EXE" if not "%INCLUIR_EXE%"=="1" exit /b 0
call :CompilarProyecto "%~1"
exit /b 0


:: ==========================================================================
:: FUNCION: COMPILAR UN .CSPROJ
:: /restore + RestorePackagesConfig -> baja los paquetes NuGet de un clone nuevo
:: /t:Build                         -> compila (salta lo que ya esta al dia)
:: /t:Rebuild (opcion [C])          -> recompila desde cero
:: ==========================================================================
:CompilarProyecto
echo "%~1"| findstr /i /l /c:"\packages\" /c:"\obj\" /c:"\bin\" >nul && exit /b 0
call :EsPrueba "%~1" && exit /b 0
:: Si ya compilo en una ronda anterior no se vuelve a compilar
set /a IDX+=1
if defined OK_%IDX% (
    set /a P_OK+=1
    exit /b 0
)
set "PROJ=%~1"
set "PROJ_NAME=%~n1"
set "PROJ_DIR=%~dp1"
set /a PASO+=1 >nul
set "PLOG=%LOG_DIR%\ronda_%RONDA%_paso_%PASO%.log"
echo   Compilando %PROJ_NAME% ...
call :BuscarSln "%PROJ_DIR%"
set "SDARG="
if not defined SLN_DIR set "SLN_DIR=%PROJ_DIR%..\"
set SDARG=/p:SolutionDir="%SLN_DIR%\"
:: Si un .csproj (Reporteador, Cierre Inventario...) apunta a un Class1.cs que ya no existe, se omite.
:: El .csproj original no se modifica.
set "FIX_ARG="
if not exist "%PROJ_DIR%Class1.cs" findstr /i /c:"Class1.cs" "%PROJ%" >nul && call :PrepararFixReporteador
:: Solo se restaura NuGet en los proyectos que tienen paquetes y cuando hace falta:
:: si ya estan bajados se salta, salvo en compilacion completa o al reintentar
set "RESTORE_ARG="
set "NECESITA_NUGET=0"
if exist "%PROJ_DIR%packages.config" if not exist "%SLN_DIR%packages\" set "NECESITA_NUGET=1"
findstr /i /c:"<PackageReference" "%PROJ%" >nul && if not exist "%PROJ_DIR%obj\project.assets.json" set "NECESITA_NUGET=1"
if /I "%TIPO_BUILD%"=="Rebuild" set "NECESITA_NUGET=1"
if not "%RONDA%"=="1" set "NECESITA_NUGET=1"
if exist "%PROJ_DIR%packages.config" if "%NECESITA_NUGET%"=="1" set "RESTORE_ARG=/restore /p:RestorePackagesConfig=true"
findstr /i /c:"<PackageReference" "%PROJ%" >nul && if "%NECESITA_NUGET%"=="1" set "RESTORE_ARG=/restore /p:RestorePackagesConfig=true"
"%MSBUILD_PATH%" "%PROJ%" %FIX_ARG% %RESTORE_ARG% %SDARG% /t:%TIPO_BUILD% /nodeReuse:true /p:Configuration=Debug /p:RunAnalyzers=false /nologo /verbosity:minimal > "%PLOG%" 2>&1
set "RC=%errorlevel%"
>>"%LOG_FILE%" echo ------ %PROJ%
type "%PLOG%" >> "%LOG_FILE%"
if not "%RC%"=="0" goto CP_ERROR
call :CopiarDebugARelease "%PROJ_DIR%"
if errorlevel 1 goto CP_ERROR
echo   [OK] %PROJ_NAME%
set "OK_%IDX%=1"
set /a P_OK+=1
exit /b 0

:CP_ERROR
echo   [ERROR] %PROJ_NAME%  (codigo %RC%)
set "EFILE=%LOG_DIR%\errores_ronda_%RONDA%_paso_%PASO%.txt"
findstr /i /c:": error " /c:"error MSB" /c:"error NU" /c:"MSB3245" /c:"MSB3243" "%PLOG%" > "%EFILE%"
:: Mostrar errores; el log por paso conserva toda la salida.
type "%EFILE%"
echo   Revise los errores mostrados arriba.
>>"%FALL_PASS%" echo "%PROJ%"
set /a P_FAIL+=1
exit /b 1


:: ==========================================================================
:: FUNCION: BUSCAR EL .SLN MAS CERCANO (hacia arriba) -> SLN_DIR (con "\" final)
:: Hace falta porque los .csproj usan $(SolutionDir) para restaurar NuGet y,
:: compilados sueltos, MSBuild no sabe cual es la solucion.
:: ==========================================================================
:BuscarSln
set "SLN_DIR="
set "WD=%~1"
:BS_LOOP
if exist "%WD%*.sln" (set "SLN_DIR=%WD%" & exit /b 0)
for %%p in ("%WD%..") do set "NWD=%%~fp"
if not "%NWD:~-1%"=="\" set "NWD=%NWD%\"
if /I "%NWD%"=="%WD%" exit /b 0
set "WD=%NWD%"
goto BS_LOOP


:: ==========================================================================
:: FUNCION: COPIAR LAS DLL DE bin\Debug A bin\Release DE UN PROYECTO
:: ==========================================================================
:CopiarDebugARelease
set "CD_SRC=%~1bin\Debug"
if not exist "%CD_SRC%\*.dll" exit /b 0
set "CD_DST=%~1bin\Release"
if not exist "%CD_DST%\" mkdir "%CD_DST%"
:: /D copia solo las DLL mas nuevas que las que ya estan en Release
xcopy "%CD_SRC%\*.dll" "%CD_DST%\" /D /Y /Q >nul 2>&1
if errorlevel 2 exit /b 1
exit /b 0


:: ==========================================================================
:Bases
call :ComprobarCarpeta "%~1" || exit /b 1
for %%P in (CapaModelo_ CapaControlador_) do for /r "%~1" %%F in (%%P*.csproj) do call :CompilarProyecto "%%~fF"
for /r "%~1" %%F in (*.csproj) do call :CompilarNoCapa "%%~fF" LIB
exit /b 0

:Vistas
call :ComprobarCarpeta "%~1" || exit /b 1
for /r "%~1" %%F in (CapaVista_*.csproj) do call :CompilarProyecto "%%~fF"
exit /b 0

:Ejecutables
call :ComprobarCarpeta "%~1" || exit /b 1
for /r "%~1" %%F in (*.csproj) do call :CompilarNoCapa "%%~fF" EXE
exit /b 0

:ComprobarCarpeta
if exist "%~1\" exit /b 0
echo [ERROR] No existe "%~1"
>> "%FALL_PASS%" echo Carpeta ausente: "%~1"
set /a P_FAIL+=1 >nul
exit /b 1

:PrepararFixReporteador
set "FIX_TARGETS=%LOG_DIR%\reporteador_sin_class1.targets"
> "%FIX_TARGETS%" echo ^<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003"^>
>> "%FIX_TARGETS%" echo   ^<Target Name="ExcluirClass1Ausente" BeforeTargets="CoreCompile"^>
>> "%FIX_TARGETS%" echo     ^<ItemGroup^>
>> "%FIX_TARGETS%" echo       ^<Compile Remove="Class1.cs" Condition="!Exists('$(MSBuildProjectDirectory)\Class1.cs')" /^>
>> "%FIX_TARGETS%" echo     ^</ItemGroup^>
>> "%FIX_TARGETS%" echo   ^</Target^>
>> "%FIX_TARGETS%" echo ^</Project^>
set FIX_ARG=/p:CustomAfterMicrosoftCommonTargets="%FIX_TARGETS%"
echo   [AJUSTE] Se omite Class1.cs ausente sin modificar %PROJ_NAME%.csproj.
exit /b 0


:: ==========================================================================
:: FUNCION: DETECTAR LOS PROYECTOS MSTest (no se compilan aqui)
:: ==========================================================================
:EsPrueba
echo %~n1| findstr /i /b /c:"MSTest" >nul && goto EP_SI
findstr /i /c:"MSTest.TestFramework" "%~1" >nul && goto EP_SI
exit /b 1
:EP_SI
if "%RONDA%"=="1" echo   [OMITIDO] %~n1 (pruebas MSTest)
exit /b 0


:: ==========================================================================
:: SEGURIDAD EN DOS PASADAS (rompe el ciclo Seguridad <-> RRHH sin tocar codigo)
:: El Splash de Seguridad usa el MDI de RRHH y las vistas de RRHH usan
:: CapaVista_Seguridad. En un clone limpio ninguna DLL existe todavia.
:: Pasada 0: compila CapaVista_Seguridad con una COPIA temporal de FrmSplash.cs
::           (en logs\build_rrhh) sin la referencia a RRHH. El codigo del repo
::           no se modifica.
:: Pasada final: Rebuild de CapaVista_Seguridad con el FrmSplash.cs original,
::           cuando ya existen las DLL de RRHH.
:: ==========================================================================
:SeguridadBootstrap
if defined OK_SEGFULL exit /b 0
set "SEG_PROJ="
for /r "%COMPONENTES_DIR%\seguridad" %%F in (CapaVista_Seguridad*.csproj) do if not defined SEG_PROJ set "SEG_PROJ=%%~fF"
if not defined SEG_PROJ exit /b 0
for %%F in ("%SEG_PROJ%") do set "SEG_DIR=%%~dpF"
set "SPLASH_SRC=%SEG_DIR%FrmSplash.cs"
if not exist "%SPLASH_SRC%" exit /b 0
:: Si ya hay una DLL previa (como en una copia vieja) y no es Rebuild, no hace falta
if exist "%SEG_DIR%bin\Debug\CapaVista_Seguridad.dll" if /I "%TIPO_BUILD%"=="Build" exit /b 0

set "SPLASH_BOOT=%LOG_DIR%\FrmSplash.bootstrap.cs"
set "SPLASH_PS=%LOG_DIR%\splash_bootstrap.ps1"
set "SEG_TARGETS=%LOG_DIR%\seguridad_bootstrap.targets"
if exist "%SPLASH_BOOT%" del "%SPLASH_BOOT%"

:: Script que copia FrmSplash.cs quitando el using de RRHH y el case "RRHH"
> "%SPLASH_PS%" echo $l = Get-Content -LiteralPath $env:SPLASH_SRC -Encoding UTF8
>> "%SPLASH_PS%" echo $o = New-Object System.Collections.Generic.List[string]
>> "%SPLASH_PS%" echo $skip = $false
>> "%SPLASH_PS%" echo foreach ($x in $l) {
>> "%SPLASH_PS%" echo   if ($x -match 'CapaVista_Rrhh') { continue }
>> "%SPLASH_PS%" echo   if (($x -match 'case\s+"RRHH"') -and ($x -notmatch '//')) { $skip = $true; continue }
>> "%SPLASH_PS%" echo   if ($skip) {
>> "%SPLASH_PS%" echo     if ($x -match 'default:') { $skip = $false } else { if ($x -match 'break;') { $skip = $false }; continue }
>> "%SPLASH_PS%" echo   }
>> "%SPLASH_PS%" echo   $o.Add($x)
>> "%SPLASH_PS%" echo }
>> "%SPLASH_PS%" echo Set-Content -LiteralPath $env:SPLASH_BOOT -Value $o -Encoding UTF8
powershell -NoProfile -ExecutionPolicy Bypass -File "%SPLASH_PS%" >nul 2>&1
if not exist "%SPLASH_BOOT%" goto SB_SIN_COPIA

:: Targets: en esta pasada se compila la copia en lugar del FrmSplash.cs original
> "%SEG_TARGETS%" echo ^<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003"^>
>> "%SEG_TARGETS%" echo   ^<Target Name="SplashBootstrap" BeforeTargets="CoreCompile"^>
>> "%SEG_TARGETS%" echo     ^<ItemGroup^>
>> "%SEG_TARGETS%" echo       ^<Compile Remove="**\FrmSplash.cs" /^>
>> "%SEG_TARGETS%" echo       ^<Compile Include="%SPLASH_BOOT%" /^>
>> "%SEG_TARGETS%" echo     ^</ItemGroup^>
>> "%SEG_TARGETS%" echo   ^</Target^>
>> "%SEG_TARGETS%" echo ^</Project^>

set "PLOG=%LOG_DIR%\ronda_%RONDA%_seguridad_pasada0.log"
echo   [PASADA 0] CapaVista_Seguridad sin el MDI de RRHH ...
call :BuscarSln "%SEG_DIR%"
if not defined SLN_DIR set "SLN_DIR=%SEG_DIR%..\"
set SDARG=/p:SolutionDir="%SLN_DIR%\"
set "RESTORE_ARG="
if exist "%SEG_DIR%packages.config" if not exist "%SLN_DIR%packages\" set "RESTORE_ARG=/restore /p:RestorePackagesConfig=true"
findstr /i /c:"<PackageReference" "%SEG_PROJ%" >nul && if not exist "%SEG_DIR%obj\project.assets.json" set "RESTORE_ARG=/restore /p:RestorePackagesConfig=true"
"%MSBUILD_PATH%" "%SEG_PROJ%" /p:CustomAfterMicrosoftCommonTargets="%SEG_TARGETS%" %RESTORE_ARG% %SDARG% /t:Build /nodeReuse:true /p:Configuration=Debug /p:RunAnalyzers=false /nologo /verbosity:minimal > "%PLOG%" 2>&1
set "RC=%errorlevel%"
>>"%LOG_FILE%" echo ------ PASADA 0 %SEG_PROJ%
type "%PLOG%" >> "%LOG_FILE%"
if not "%RC%"=="0" goto SB_ERROR
call :CopiarDebugARelease "%SEG_DIR%"
echo   [OK] CapaVista_Seguridad (pasada 0)
exit /b 0

:SB_ERROR
echo   [AVISO] Fallo la pasada 0 de CapaVista_Seguridad (codigo %RC%).
findstr /i /c:": error " /c:"error MSB" /c:"error NU" "%PLOG%"
echo   Detalle: "%PLOG%"
exit /b 0

:SB_SIN_COPIA
echo   [AVISO] No se pudo generar la copia temporal de FrmSplash.cs; se omite la pasada 0.
exit /b 0

:SeguridadFinal
set "TIPO_PREV=%TIPO_BUILD%"
set "TIPO_BUILD=Rebuild"
set /a PF_ANTES=P_FAIL
call :Vistas "%COMPONENTES_DIR%\seguridad"
set "TIPO_BUILD=%TIPO_PREV%"
if %P_FAIL% EQU %PF_ANTES% set "OK_SEGFULL=1"
:LimpiarTemporales
if defined LOG_DIR if exist "%LOG_DIR%\" (
    del /q "%LOG_DIR%\*" >nul 2>&1
    rd "%LOG_DIR%" >nul 2>&1
)
exit /b 0
