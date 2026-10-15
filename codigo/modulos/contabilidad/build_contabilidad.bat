:: ============================================================================
:: BUILD CONTABILIDAD - VISUAL STUDIO 2022
:: ============================================================================
:: Compila Modelo -> Controlador -> Vista -> Ejecucion, con referencias DLL.
:: Contabilidad tiene su propio login y ya no depende de la Vista de Seguridad.
::
:: REQUISITO: CapaVista_Contabilidad referencia CapaVista_Navegador.dll por
:: HintPath (igual que CapaVista_Seguridad), por lo que primero hay que haber
:: ejecutado codigo\componentes\navegador\build_navegador.bat al menos una vez.
::
:: Salida: Ejecucion_Contabilidad\Ejecucion_Contabilidad\bin\Debug\Ejecucion_Contabilidad.exe
:: ============================================================================
@echo off
setlocal

:: Si tienes Professional o Enterprise, cambia "Community" por la edicion.
set "MSBUILD_PATH=C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
set "COMP=%~dp0..\..\componentes"
set "MOD=%~dp0"

if not exist "%MSBUILD_PATH%" (
    echo [ERROR] No se encontro MSBuild en: %MSBUILD_PATH%
    echo         Ajusta MSBUILD_PATH al inicio de este archivo.
    goto FIN_ERROR
)
if not exist "%COMP%\navegador\Navegador\CapaVista_Navegador\bin\Debug\CapaVista_Navegador.dll" (
    echo [ERROR] Falta CapaVista_Navegador.dll. Ejecuta primero:
    echo         %COMP%\navegador\build_navegador.bat
    goto FIN_ERROR
)

echo Compilando las tres capas de Contabilidad...
"%MSBUILD_PATH%" "%MOD%Contabilidad\Contabilidad.sln" /restore /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
if errorlevel 1 goto FIN_ERROR

echo Compilando Ejecucion_Contabilidad...
"%MSBUILD_PATH%" "%MOD%Ejecucion_Contabilidad\Ejecucion_Contabilidad.sln" /restore /t:Rebuild /p:Configuration=Debug /v:minimal /nologo
if errorlevel 1 goto FIN_ERROR

echo.
echo [OK] %MOD%Ejecucion_Contabilidad\Ejecucion_Contabilidad\bin\Debug\Ejecucion_Contabilidad.exe
exit /b 0

:FIN_ERROR
echo.
pause
exit /b 1
