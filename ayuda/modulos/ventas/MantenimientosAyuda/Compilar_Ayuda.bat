@echo off
setlocal
cd /d "%~dp0"
title Compilar ayuda - Mantenimientos
echo ============================================
echo  Compilando Mantenimientos.chm
echo ============================================

set "HHC="
if exist "%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe" set "HHC=%ProgramFiles(x86)%\HTML Help Workshop\hhc.exe"
if exist "%ProgramFiles%\HTML Help Workshop\hhc.exe" set "HHC=%ProgramFiles%\HTML Help Workshop\hhc.exe"
if not defined HHC for /f "delims=" %%I in ('where hhc.exe 2^>nul') do set "HHC=%%I"

if not defined HHC goto :SIN_COMPILADOR

rem Se borra el CHM anterior para comprobar que el nuevo se genere.
if exist "..\Mantenimientos.chm" del /f /q "..\Mantenimientos.chm"

"%HHC%" Mantenimientos.hhp

rem OJO: hhc.exe devuelve 1 cuando compila bien y 0 cuando falla,
rem por eso se verifica la existencia del archivo y no el errorlevel.
if not exist "..\Mantenimientos.chm" goto :ERROR

echo.
echo Listo: se genero "%~dp0..\Mantenimientos.chm"

rem Uso opcional: Compilar_Ayuda.bat "C:\ruta\del\repositorio\ayuda"
rem copia el CHM a la carpeta indicada.
if not "%~1"=="" copy /y "..\Mantenimientos.chm" "%~1\" >nul && echo Copiado a "%~1"
echo.
pause
exit /b 0

:SIN_COMPILADOR
echo.
echo No se encontro hhc.exe de HTML Help Workshop.
echo Instalelo e intente de nuevo, o agregue su carpeta al PATH.
echo.
pause
exit /b 1

:ERROR
echo.
echo No se genero Mantenimientos.chm. Revise los mensajes del compilador de arriba.
echo.
pause
exit /b 1
