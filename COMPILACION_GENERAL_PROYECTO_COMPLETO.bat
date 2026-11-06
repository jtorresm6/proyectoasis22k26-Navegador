@echo off
:: BUILD GLOBAL: Consultas, Navegador, Reporteador, Seguridad, Logistica, RRHH,
:: Bancos, Compras, Contabilidad y Ventas (todo lo que necesita FrmSplash).
:: Descomprimir el ZIP y colocar este .bat en la raiz del repositorio.
:: Doble clic: compilar con menu. /auto: terminar con codigo 0 o 1.
:: Requiere Visual Studio/Build Tools y desarrollo de escritorio .NET.
:: Compila Debug, restaura NuGet y reintenta hasta tres rondas.
:: Logs: logs\build_rrhh junto al .bat; cada proyecto tiene su propio log.
:: Conserva copias Debug en bin\Release para referencias heredadas.
:: Cada DLL compilada se copia ademas a logs\build_rrhh\componentes. Un .targets
:: temporal (logs\build_rrhh\build_global.targets) usa esa carpeta como respaldo
:: de referencias rotas y agrega a CapaVista_Seguridad las referencias a Bancos,
:: Ventas, Contabilidad y Compras SIN modificar ningun .csproj ni .cs.
:: MSBUILD_MANUAL puede indicar una ruta alternativa a MSBuild.exe.
:: MODO RAPIDO (por defecto): compilacion incremental, sin restore ni Rebuild.
::   Si un proyecto falla, se reintenta solo con restore y Rebuild completo.
:: MODO COMPLETO: restore + Rebuild de todo. Se usa en la primera corrida
::   (cuando no existe logs\build_rrhh\restaurado.flag) o con el parametro /limpio.
:: Parametros: /auto (sin menu, codigo de salida) y /limpio (fuerza modo completo).
setlocal EnableExtensions DisableDelayedExpansion
chcp 65001 >nul
color 0A
title Compilacion global - Componentes y todos los modulos
set "AUTO=0"
set "MODO_LIMPIO=0"
for %%A in (%*) do (
    if /i "%%~A"=="/auto" set "AUTO=1"
    if /i "%%~A"=="/limpio" set "MODO_LIMPIO=1"
)

:: --------------------------------------------------------------------------
:: OPCIONES (normalmente NO hay que tocar nada)
:: --------------------------------------------------------------------------
:: Dejar vacio para detectar MSBuild automaticamente. Solo si la deteccion
:: falla, escribir la ruta completa de MSBuild.exe, por ejemplo:
::   set "MSBUILD_MANUAL=C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
:: MSBUILD_MANUAL puede definirse en el entorno.


:INICIO
set "PASO=0"
set /a total=0
set /a ok=0
set /a fail=0
set /a P_OK=0
set /a P_FAIL=0

echo ============================================
echo COMPILACION GLOBAL - COMPONENTES Y TODOS LOS MODULOS
echo ============================================

:: --------------------------------------------------------------------------
:: 1) CARPETA DE ESTE .BAT Y CARPETA DE LOGS
:: --------------------------------------------------------------------------
set "ROOT_DIR=%~dp0"
:: Las rutas se resuelven desde el .bat sin cambiar el directorio de la consola.
set "LOG_DIR=%ROOT_DIR%logs\build_rrhh"
if not exist "%LOG_DIR%" mkdir "%LOG_DIR%"
if not exist "%LOG_DIR%\" goto FIN_ERROR
set "LOG_FILE=%LOG_DIR%\build_log.txt"
set "FALLIDOS=%LOG_DIR%\fallidos.txt"
set "FALL_PASS=%LOG_DIR%\fallidos_pasada.txt"
if exist "%FALLIDOS%" del "%FALLIDOS%"
if exist "%FALL_PASS%" del "%FALL_PASS%"
echo Compilacion iniciada: %date% %time% > "%LOG_FILE%" || goto FIN_ERROR
:: Carpeta central de DLL y .targets auxiliar (se regeneran en cada corrida).
set "CENTRAL_DIR=%LOG_DIR%\componentes"
set "GLOBAL_TARGETS=%LOG_DIR%\build_global.targets"
if exist "%CENTRAL_DIR%\" rmdir /s /q "%CENTRAL_DIR%"
mkdir "%CENTRAL_DIR%"
if not exist "%CENTRAL_DIR%\" goto FIN_ERROR
call :PrepararTargetsGlobal
if not exist "%LOG_DIR%\restaurado.flag" set "MODO_LIMPIO=1"
if "%MODO_LIMPIO%"=="1" echo Modo: COMPLETO - restore y Rebuild de todo
if not "%MODO_LIMPIO%"=="1" echo Modo: RAPIDO - incremental, sin restore

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
set "BANCOS_DIR=%MODULOS_DIR%\bancos"
set "COMPRAS_DIR=%MODULOS_DIR%\compras"
set "CONTABILIDAD_DIR=%MODULOS_DIR%\contabilidad"
set "VENTAS_DIR="
:: La carpeta de Ventas se detecta por su MDI (VentasYCxC\CapaVista_Ventas).
for /d %%D in ("%MODULOS_DIR%\*") do if exist "%%~fD\VentasYCxC\CapaVista_Ventas\CapaVista_Ventas.csproj" set "VENTAS_DIR=%%~fD"
if not defined VENTAS_DIR set "VENTAS_DIR=%MODULOS_DIR%\ventas"

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
:: Ventas: primero el modelo y controlador de su MDI (los demas formularios los usan).
call :MdiBase "%VENTAS_DIR%\VentasYCxC"
:: Contabilidad: primero modelo y controlador base (carpeta Contabilidad), luego sus 5 submodulos.
call :MdiBase "%CONTABILIDAD_DIR%\Contabilidad"
call :Bases "%BANCOS_DIR%"
call :Bases "%COMPRAS_DIR%"
call :Bases "%CONTABILIDAD_DIR%"
call :Bases "%VENTAS_DIR%"
for %%C in (consultas reporteador navegador) do call :Vistas "%COMPONENTES_DIR%\%%C"
:: Logistica: primero sus vistas y al final su MDI (carpeta Logistica)
for /d %%D in ("%MODULOS_DIR%\Logistica\*") do if /i not "%%~nxD"=="Logistica" call :Vistas "%%~fD"
call :Vistas "%MODULOS_DIR%\Logistica\Logistica"
for /d %%D in ("%MODULOS_DIR%\rrhh\Bibliotecas de clase RRHH\*") do if /i not "%%~nxD"=="Recursos Humanos" call :Vistas "%%~fD"
call :Vistas "%MODULOS_DIR%\rrhh\Bibliotecas de clase RRHH\Recursos Humanos"
:: Cada modulo: primero sus vistas y al final su MDI (CapaVista_<Modulo>).
call :VistasConMdi "%BANCOS_DIR%" CapaVista_Bancos
call :VistasConMdi "%COMPRAS_DIR%" CapaVista_Compras
call :VistasConMdi "%CONTABILIDAD_DIR%" CapaVista_Contabilidad
call :VistasConMdi "%VENTAS_DIR%" CapaVista_Ventas
:: La vista de Seguridad va al final: el Splash usa el MDI de todos los modulos
call :Vistas "%COMPONENTES_DIR%\seguridad"
set "INCLUIR_EXE=1"
for %%C in (consultas reporteador navegador seguridad) do call :Ejecutables "%COMPONENTES_DIR%\%%C"
call :Ejecutables "%MODULOS_DIR%\rrhh\Bibliotecas de clase RRHH"
call :Ejecutables "%MODULOS_DIR%\Logistica"
call :Ejecutables "%BANCOS_DIR%"
call :Ejecutables "%COMPRAS_DIR%"
call :Ejecutables "%CONTABILIDAD_DIR%"
call :Ejecutables "%VENTAS_DIR%"
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
if %fail% EQU 0 call :EnlaceFinal
if %fail% EQU 0 if %total% GTR 0 echo ok> "%LOG_DIR%\restaurado.flag"

echo.
echo ============================================
echo COMPILACION GLOBAL FINALIZADA
echo Total de proyectos: %total%
echo Correctos: %ok%
echo Con errores: %fail%
echo Log completo: %LOG_FILE%
echo ============================================
if exist "%FALLIDOS%" echo.
if exist "%FALLIDOS%" echo PROYECTOS QUE NO COMPILARON:
if exist "%FALLIDOS%" type "%FALLIDOS%"
if exist "%FALLIDOS%" echo (errores por ronda y paso en "%LOG_DIR%")

:: --------------------------------------------------------------------------
if "%AUTO%"=="1" goto FIN_OK
echo.
echo Presiona una opcion:
echo [R] Recompilar rapido
echo [L] Recompilar completo - restore y Rebuild
echo [S] Salir
choice /c RLS /n /m "Seleccion: "
if errorlevel 3 goto FIN_OK
if errorlevel 2 set "MODO_LIMPIO=1"
if errorlevel 2 goto INICIO
set "MODO_LIMPIO=0"
goto INICIO

:FIN_OK
set "EXIT_CODE=0"
if %fail% GTR 0 set "EXIT_CODE=1"
if %total% EQU 0 set "EXIT_CODE=1"
endlocal & exit /b %EXIT_CODE%

:FIN_ERROR
echo.
if not "%AUTO%"=="1" pause
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
:: /t:Rebuild                       -> recompila desde cero
:: ==========================================================================
:CompilarProyecto
echo "%~1"| findstr /i /l /c:"\packages\" /c:"\obj\" /c:"\bin\" >nul && exit /b 0
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
if not exist "%PROJ_DIR%Class1.cs" type "%PROJ%" | findstr /i /c:"Class1.cs" >nul && call :PrepararFixReporteador
if "%MODO_LIMPIO%"=="1" goto CP_COMPLETO
"%MSBUILD_PATH%" "%PROJ%" %FIX_ARG% %SDARG% /p:ComponentesGlobalDir="%CENTRAL_DIR%" /p:CustomBeforeMicrosoftCommonTargets="%GLOBAL_TARGETS%" /p:BuildProjectReferences=false /t:Build /p:Configuration=Debug /nologo /verbosity:minimal > "%PLOG%" 2>&1
set "RC=%errorlevel%"
if "%RC%"=="0" goto CP_COMPILADO
echo   [REINTENTO] %PROJ_NAME% con restore y Rebuild completo
:CP_COMPLETO
"%MSBUILD_PATH%" "%PROJ%" %FIX_ARG% /restore /p:RestorePackagesConfig=true %SDARG% /p:ComponentesGlobalDir="%CENTRAL_DIR%" /p:CustomBeforeMicrosoftCommonTargets="%GLOBAL_TARGETS%" /t:Rebuild /p:Configuration=Debug /nologo /verbosity:minimal > "%PLOG%" 2>&1
set "RC=%errorlevel%"
:CP_COMPILADO
>>"%LOG_FILE%" echo ------ %PROJ%
type "%PLOG%" >> "%LOG_FILE%"
if not "%RC%"=="0" goto CP_ERROR
call :CopiarDebugARelease "%PROJ_DIR%"
if errorlevel 1 goto CP_ERROR
echo   [OK] %PROJ_NAME%
set /a P_OK+=1
exit /b 0

:CP_ERROR
echo   [ERROR] %PROJ_NAME%  (codigo %RC%)
set "EFILE=%LOG_DIR%\errores_ronda_%RONDA%_paso_%PASO%.txt"
findstr /i /c:": error " /c:"error MSB" /c:"error NU" /c:"MSB3245" /c:"MSB3243" "%PLOG%" > "%EFILE%"
:: Mostrar errores; el log por paso conserva toda la salida.
type "%EFILE%"
echo   Detalle: "%PLOG%"
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
copy /Y "%CD_SRC%\*.dll" "%CD_DST%\" >nul 2>&1
exit /b %ERRORLEVEL%


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

:MdiBase
if not exist "%~1\" exit /b 0
for %%P in (CapaModelo_ CapaControlador_) do for /r "%~1" %%F in (%%P*.csproj) do call :CompilarProyecto "%%~fF"
exit /b 0

:VistasConMdi
call :ComprobarCarpeta "%~1" || exit /b 1
for /r "%~1" %%F in (CapaVista_*.csproj) do if /i not "%%~nF"=="%~2" call :CompilarProyecto "%%~fF"
for /r "%~1" %%F in (%~2*.csproj) do if /i "%%~nF"=="%~2" call :CompilarProyecto "%%~fF"
exit /b 0

:EnlaceFinal
:: Copia a Ejecucion_Seguridad las DLL de los modulos que aun no tenga (no pisa nada).
set "EXE_SEG=%COMPONENTES_DIR%\seguridad\Ejecucion_Seguridad\Ejecucion_Seguridad\bin\Debug"
if not exist "%EXE_SEG%\" exit /b 0
for %%F in ("%CENTRAL_DIR%\*.dll") do if not exist "%EXE_SEG%\%%~nxF" copy /y "%%~fF" "%EXE_SEG%\" >nul 2>&1
echo [ENLACE] DLL de modulos copiadas a: %EXE_SEG%
exit /b 0

:PrepararTargetsGlobal
set "GT=%GLOBAL_TARGETS%"
> "%GT%" echo ^<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003"^>
>> "%GT%" echo   ^<Target Name="ResolverReferenciasGlobal" BeforeTargets="ResolveAssemblyReferences" Condition="'$(ComponentesGlobalDir)' != ''"^>
>> "%GT%" echo     ^<ItemGroup^>
>> "%GT%" echo       ^<Reference^>
>> "%GT%" echo         ^<RutaGlobal^>$(ComponentesGlobalDir)\$([System.Text.RegularExpressions.Regex]::Replace('%%(Reference.Identity)', ',.*', '')).dll^</RutaGlobal^>
>> "%GT%" echo         ^<HintPath Condition="'%%(Reference.HintPath)' != '' And !Exists('%%(Reference.HintPath)') And Exists('%%(Reference.RutaGlobal)')"^>%%(Reference.RutaGlobal)^</HintPath^>
>> "%GT%" echo       ^</Reference^>
>> "%GT%" echo     ^</ItemGroup^>
>> "%GT%" echo   ^</Target^>
>> "%GT%" echo   ^<Target Name="InyectarModulosSplash" BeforeTargets="ResolveAssemblyReferences" Condition="'$(MSBuildProjectName)' == 'CapaVista_Seguridad' And '$(ComponentesGlobalDir)' != ''"^>
>> "%GT%" echo     ^<PropertyGroup^>
>> "%GT%" echo       ^<RefsActualesSplash^>@(Reference)^</RefsActualesSplash^>
>> "%GT%" echo     ^</PropertyGroup^>
>> "%GT%" echo     ^<ItemGroup^>
>> "%GT%" echo       ^<Reference Include="CapaVista_Bancos" Condition="!$(RefsActualesSplash.Contains('CapaVista_Bancos')) And Exists('$(ComponentesGlobalDir)\CapaVista_Bancos.dll')"^>^<HintPath^>$(ComponentesGlobalDir)\CapaVista_Bancos.dll^</HintPath^>^</Reference^>
>> "%GT%" echo       ^<Reference Include="CapaVista_Ventas" Condition="!$(RefsActualesSplash.Contains('CapaVista_Ventas')) And Exists('$(ComponentesGlobalDir)\CapaVista_Ventas.dll')"^>^<HintPath^>$(ComponentesGlobalDir)\CapaVista_Ventas.dll^</HintPath^>^</Reference^>
>> "%GT%" echo       ^<Reference Include="CapaVista_Contabilidad" Condition="!$(RefsActualesSplash.Contains('CapaVista_Contabilidad')) And Exists('$(ComponentesGlobalDir)\CapaVista_Contabilidad.dll')"^>^<HintPath^>$(ComponentesGlobalDir)\CapaVista_Contabilidad.dll^</HintPath^>^</Reference^>
>> "%GT%" echo       ^<Reference Include="CapaVista_Compras" Condition="!$(RefsActualesSplash.Contains('CapaVista_Compras')) And Exists('$(ComponentesGlobalDir)\CapaVista_Compras.dll')"^>^<HintPath^>$(ComponentesGlobalDir)\CapaVista_Compras.dll^</HintPath^>^</Reference^>
>> "%GT%" echo     ^</ItemGroup^>
>> "%GT%" echo   ^</Target^>
>> "%GT%" echo   ^<Target Name="CopiarBibliotecaGlobal" AfterTargets="Build" Condition="'$(OutputType)' == 'Library' And '$(ComponentesGlobalDir)' != ''"^>
>> "%GT%" echo     ^<Copy SourceFiles="$(TargetPath)" DestinationFolder="$(ComponentesGlobalDir)" SkipUnchangedFiles="true" ContinueOnError="true" /^>
>> "%GT%" echo   ^</Target^>
>> "%GT%" echo ^</Project^>
exit /b 0
