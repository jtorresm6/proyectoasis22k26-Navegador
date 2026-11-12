:: ============================================================================
:: Area   : Contabilidad
:: Autor  : Gabriel Andre Guillen Pocon
:: Carne  : 0901-23-1998
:: Fecha  : 25/10/2026
:: ============================================================================
:: Compila, en orden, solo lo necesario para abrir Contabilidad desde el
:: login de Seguridad: Seguridad, Consultas, Reporteador y Navegador; Bancos,
:: Compras, Logistica, RRHH y Ventas (el login abre el MDI de cada modulo);
:: la base de Contabilidad, las 5 soluciones, el MDI y Ejecucion_Contabilidad.
:: Se detiene en el primer error.
:: Funciona desde cualquier carpeta: busca el repositorio en la ruta que se le
:: pase, subiendo desde el bat o desde la carpeta actual, y en C:\proyectoasis22k26.
:: Si no lo encuentra, pide la ruta.
:: ============================================================================
@echo off
setlocal EnableDelayedExpansion
chcp 65001 >nul

:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 09/11/2026
set "MARCA=codigo\modulos\contabilidad\Bibliotecas de Contabilidad\Contabilidad\Contabilidad.sln"
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 09/11/2026
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
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 09/11/2026
set "B=%M%\Bibliotecas de Contabilidad"
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 09/11/2026

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

:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 07/11/2026
set "MOD=%RAIZ%\codigo\modulos"
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
:: Ventas enlaza sus DLL con su propio .targets (igual que build_ventas.bat)
set "VENTAS=-p:CustomBeforeMicrosoftCommonTargets="%MOD%\ventas\Compilacion_Ventas\build_componentes_ventas.targets""
:: Compras apunta a los paquetes viejos de Ventas: ReportViewer se toma del Reporteador
set "COMPRAS=-p:ReferencePath="%C%\reporteador\Reporteador\packages\Microsoft.ReportingServices.ReportViewerControl.Winforms.150.1652.0\lib\net40""
:: Seguridad busca Compras y Ventas en sus carpetas viejas: se le indica donde quedaron
set "SEGURIDAD=-p:ReferencePath="%MOD%\compras\Bibliotecas de clase Compras\Compras\CapaVista_Compras\bin\Debug;%MOD%\ventas\Biblioteca de clase Ventas Y CxC\VentasYCxC\CapaVista_Ventas\bin\Debug""
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%C%\consultas\Consultas\CapaModelo_Consultas\CapaModelo_Consultas.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnActualizar_Reporteador\CapaModelo_BtnActualizar_Reporteador\CapaModelo_BtnActualizar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnBusqueda_Reporteador\CapaModelo_BtnBusqueda\CapaModelo_BtnBusqueda.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnEditar_Reporteador\CapaModelo_BtnEditar_Reporteador\CapaModelo_BtnEditar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnEliminar_Reporteador\CapaModelo_BtnEliminar_Reporteador\CapaModelo_BtnEliminar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnGuardar_Reporteador\CapaModelo_BtnGuardar_Reporteador\CapaModelo_BtnGuardar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnLimpiar_Reporteador\CapaModelo_BtnLimpiar_Reporteador\CapaModelo_BtnLimpiar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnRuta_Reporteador\CapaModelo_BtnRuta_Reporteador\CapaModelo_BtnRuta_Reporteador.csproj" || goto :fallo
call :compilar "%C%\seguridad\Seguridad\CapaModelo_Seguridad\CapaModelo_Seguridad.csproj" || goto :fallo
call :compilar "%C%\navegador\Navegador\CapaModelo_Navegador\CapaModelo_Navegador.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Autorizacion_OrdenesCompraBancos\CapaModelo_AutorizacionOrdenesBancos\CapaModelo_AutorizacionOrdenesBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Bancos\CapaModelo_Bancos\CapaModelo_Bancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Cierre_BancarioBancos\CapaModelo_CierreBancarioBancos\CapaModelo_CierreBancarioBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Disponibilidad_DiariaBancos\CapaModelo_DisponibilidadDiariaBancos\CapaModelo_DisponibilidadDiariaBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Enlace_Otros_ModulosBancos\CapaModelo_EnlaceBancos\CapaModelo_EnlaceBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Generacion_PagosBancos\CapaModelo_GeneracionPagosBancos\CapaModelo_GeneracionPagosBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Movimientos_BancariosBancos\CapaModelo_MovimientosBancariosBancos\CapaModelo_MovimientosBancariosBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Tipo_CambioBancos\CapaModelo_TipoCambioBancos\CapaModelo_TipoCambioBancos.csproj" || goto :fallo
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\AprobacionFinanciera\CapaModelo_AprobacionFinanciera\CapaModelo_AprobacionFinanciera.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\CierreMovimientos\CapaModelo_CierreMovimientos\CapaModelo_CierreMovimientos.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Componentes\CapaModelo_Componentes\CapaModelo_Componentes.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Herramientas\CapaModelo_Herramientas\CapaModelo_Herramientas.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Compras\CapaModelo_Compras\CapaModelo_Compras.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\MovimientosProveedores\CapaModelo_MovimientosProveedores\CapaModelo_MovimientosProveedores.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\OrdenesCompra\CapaModelo_OrdenesCompra\CapaModelo_OrdenesCompra.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\PagosProveedores\CapaModelo_PagosProveedores\CapaModelo_PagosProveedores.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Polizas\CapaModelo_Polizas\CapaModelo_Polizas.csproj" COMPRAS || goto :fallo
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%B%\Contabilidad\CapaModelo_Contabilidad\CapaModelo_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\ActivosFijos_Contabilidad\CapaModelo_ActivosFijos_Contabilidad\CapaModelo_ActivosFijos_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\CierreContable_Contabilidad\CapaModelo_CierreContable_Contabilidad\CapaModelo_CierreContable_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\EstadosFinancieros_Contabilidad\CapaModelo_EstadosFinancieros_Contabilidad\CapaModelo_EstadosFinancieros_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\PolizasCuentas_Contabilidad\CapaModelo_PolizasCuentas_Contabilidad\CapaModelo_PolizasCuentas_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\Presupuestos_Contabilidad\CapaModelo_Presupuestos_Contabilidad\CapaModelo_Presupuestos_Contabilidad.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\CierreInventario_Logistica\CapaModelo_CierreInventario_Logistica\CapaModelo_CierreInventario_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Inventario_Logistica\CapaModelo_Inventario_Logistica\CapaModelo_Inventario_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Logistica\CapaModelo_Logistica\CapaModelo_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\MuestreoAuditoria_Logistica\CapaModelo_MuestreoAuditoria_Logistica\CapaModelo_MuestreoAuditoria_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\TransporteExterno_Logistica\CapaModelo_TransporteExterno_Logistica\CapaModelo_TransporteExterno_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\TransporteInterno_Logistica\CapaModelo_TransporteInterno_Logistica\CapaModelo_TransporteInterno_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Traslados_Logistica\CapaModelo_Traslados_Logistica\CapaModelo_Traslados_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Asistencia\CapaModelo_Asistencia\CapaModelo_Asistencia.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Cierre de Nominas\CapaModelo_CierreNominas\CapaModelo_CierreNominas.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Empleados\CapaModelo_Empleado\CapaModelo_Empleado.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\EvaluacionDesempeño\CapaModelo_EvaluacionDesempeño\CapaModelo_EvaluacionDesempeño.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Liquidacion_Empleados_Rrhh\CapaModelo_Liquidacion_Rrhh\CapaModelo_Liquidacion_Rrhh.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\NominasEmpleados\CapaModelo_Nomina\CapaModelo_Nomina.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\PermisosVacaciones\CapaModelo_PermisosVacaciones\CapaModelo_PermisosVacaciones.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Recursos Humanos\CapaModelo_Rrhh\CapaModelo_Rrhh.csproj" || goto :fallo
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\VentasYCxC\CapaModelo_Ventas\CapaModelo_Ventas.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\CierreMovimientos\CapaModelo_CierreMovimientos\CapaModelo_CierreMovimientos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\ComisionVendedores\CapaModelo_ComisionVendedores\CapaModelo_ComisionVendedores.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Cotizaciones\CapaModelo_Cotizaciones\CapaModelo_Cotizaciones.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Facturas\CapaModelo_Facturas\CapaModelo_Facturas.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\ListadoPrecios\CapaModelo_ListadoPrecios\CapaModelo_ListadoPrecios.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Mantenimientos\CapaModelo_Mantenimientos\CapaModelo_Mantenimientos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\MovimientosClientes\CapaModelo_MovimientosClientes\CapaModelo_MovimientosClientes.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Pedidos\CapaModelo_Pedidos\CapaModelo_Pedidos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Reportes\CapaModelo_Reportes\CapaModelo_Reportes.csproj" VENTAS || goto :fallo
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%C%\consultas\Consultas\CapaControlador_Consultas\CapaControlador_Consultas.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnActualizar_Reporteador\CapaControlador_BtnActualizar_Reporteador\CapaControlador_BtnActualizar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnBusqueda_Reporteador\CapaControlador_BtnBusqueda\CapaControlador_BtnBusqueda.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnEditar_Reporteador\CapaControlador_BtnEditar_Reporteador\CapaControlador_BtnEditar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnEliminar_Reporteador\CapaControlador_BtnEliminar_Reporteador\CapaControlador_BtnEliminar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnGuardar_Reporteador\CapaControlador_BtnGuardar_Reporteador\CapaControlador_BtnGuardar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnLimpiar_Reporteador\CapaControlador_BtnLimpiar_Reporteador\CapaControlador_BtnLimpiar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnRuta_Reporteador\CapaControlador_BtnRuta_Reporteador\CapaControlador_BtnRuta_Reporteador.csproj" || goto :fallo
call :compilar "%C%\seguridad\Seguridad\CapaControlador_Seguridad\CapaControlador_Seguridad.csproj" || goto :fallo
call :compilar "%C%\navegador\Navegador\CapaControlador_Navegador\CapaControlador_Navegador.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Autorizacion_OrdenesCompraBancos\CapaControlador_AutorizacionOrdenesBancos\CapaControlador_AutorizacionOrdenesBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Bancos\CapaControlador_Bancos\CapaControlador_Bancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Cierre_BancarioBancos\CapaControlador_CierreBancarioBancos\CapaControlador_CierreBancarioBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Disponibilidad_DiariaBancos\CapaControlador_DisponibilidadDiariaBancos\CapaControlador_DisponibilidadDiariaBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Enlace_Otros_ModulosBancos\CapaControlador_EnlaceBancos\CapaControlador_EnlaceBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Generacion_PagosBancos\CapaControlador_GeneracionPagosBancos\CapaControlador_GeneracionPagosBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Movimientos_BancariosBancos\CapaControlador_MovimientosBancariosBancos\CapaControlador_MovimientosBancariosBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Tipo_CambioBancos\CapaControlador_TipoCambioBancos\CapaControlador_TipoCambioBancos.csproj" || goto :fallo
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\AprobacionFinanciera\CapaControlador_AprobacionFinanciera\CapaControlador_AprobacionFinanciera.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\CierreMovimientos\CapaControlador_CierreMovimientos\CapaControlador_CierreMovimientos.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Componentes\CapaControlador_Componentes\CapaControlador_Componentes.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Herramientas\CapaControlador_Herramientas\CapaControlador_Herramientas.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Compras\CapaControlador_Compras\CapaControlador_Compras.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\MovimientosProveedores\CapaControlador_MovimientosProveedores\CapaControlador_MovimientosProveedores.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\OrdenesCompra\CapaControlador_OrdenesCompra\CapaControlador_OrdenesCompra.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\PagosProveedores\CapaControlador_PagosProveedores\CapaControlador_PagosProveedores.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Polizas\CapaControlador_Polizas\CapaControlador_Polizas.csproj" COMPRAS || goto :fallo
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%B%\Contabilidad\CapaControlador_Contabilidad\CapaControlador_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\ActivosFijos_Contabilidad\CapaControlador_ActivosFijos_Contabilidad\CapaControlador_ActivosFijos_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\PolizasCuentas_Contabilidad\CapaControlador_PolizasCuentas_Contabilidad\CapaControlador_PolizasCuentas_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\CierreContable_Contabilidad\CapaControlador_CierreContable_Contabilidad\CapaControlador_CierreContable_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\EstadosFinancieros_Contabilidad\CapaControlador_EstadosFinancieros_Contabilidad\CapaControlador_EstadosFinancieros_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\Presupuestos_Contabilidad\CapaControlador_Presupuestos_Contabilidad\CapaControlador_Presupuestos_Contabilidad.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\CierreInventario_Logistica\CapaControlador_CierreInventario_Logistica\CapaControlador_CierreInventario_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Inventario_Logistica\CapaControlador_Inventario_Logistica\CapaControlador_Inventario_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Logistica\CapaControlador_Logistica\CapaControlador_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\MuestreoAuditoria_Logistica\CapaControlador_MuestreoAuditoria_Logistica\CapaControlador_MuestreoAuditoria_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\TransporteExterno_Logistica\CapaControlador_TransporteExterno_Logistica\CapaControlador_TransporteExterno_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\TransporteInterno_Logistica\CapaControlador_TransporteInterno_Logistica\CapaControlador_TransporteInterno_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Traslados_Logistica\CapaControlador_Traslados_Logistica\CapaControlador_Traslados_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Asistencia\CapaControlador_Asistencia\CapaControlador_Asistencia.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Cierre de Nominas\CapaControlador_CierreNominas\CapaControlador_CierreNominas.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Empleados\CapaControlador_Empleado\CapaControlador_Empleado.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\EvaluacionDesempeño\CapaControlador_EvaluacionDesempeño\CapaControlador_EvaluacionDesempeño.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Liquidacion_Empleados_Rrhh\CapaControlador_Liquidacion_Rrhh\CapaControlador_Liquidacion_Rrhh.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\NominasEmpleados\CapaControlador_Nomina\CapaControlador_Nomina.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\PermisosVacaciones\CapaControlador_PermisosVacaciones\CapaControlador_PermisosVacaciones.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Recursos Humanos\CapaControlador_Rrhh\CapaControlador_Rrhh.csproj" || goto :fallo
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\CierreMovimientos\CapaControlador_CierreMovimientos\CapaControlador_CierreMovimientos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\ComisionVendedores\CapaControlador_ComisionVendedores\CapaControlador_ComisionVendedores.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Cotizaciones\CapaControlador_Cotizaciones\CapaControlador_Cotizaciones.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Facturas\CapaControlador_Facturas\CapaControlador_Facturas.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\ListadoPrecios\CapaControlador_ListadoPrecios\CapaControlador_ListadoPrecios.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Mantenimientos\CapaControlador_Mantenimientos\CapaControlador_Mantenimientos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\MovimientosClientes\CapaControlador_MovimientosClientes\CapaControlador_MovimientosClientes.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Pedidos\CapaControlador_Pedidos\CapaControlador_Pedidos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Reportes\CapaControlador_Reportes\CapaControlador_Reportes.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\VentasYCxC\CapaControlador_Ventas\CapaControlador_Ventas.csproj" VENTAS || goto :fallo
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%C%\consultas\Consultas\CapaVista_Consultas\CapaVista_Consultas.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnActualizar_Reporteador\CapaVista_BtnActualizar_Reporteador\CapaVista_BtnActualizar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnBusqueda_Reporteador\CapaVista_BtnBusqueda\CapaVista_BtnBusqueda.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnEditar_Reporteador\CapaVista_BtnEditar_Reporteador\CapaVista_BtnEditar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnEliminar_Reporteador\CapaVista_BtnEliminar_Reporteador\CapaVista_BtnEliminar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnGuardar_Reporteador\CapaVista_BtnGuardar_Reporteador\CapaVista_BtnGuardar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnLimpiar_Reporteador\CapaVista_BtnLimpiar_Reporteador\CapaVista_BtnLimpiar_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\Reporteador\CapaModelo_Reporteador\CapaModelo_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\Reporteador\CapaControlador_Reporteador\CapaControlador_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\BtnRuta_Reporteador\CapaVista_BtnRuta_Reporteador\CapaVista_BtnRuta_Reporteador.csproj" || goto :fallo
call :compilar "%C%\reporteador\Reporteador\CapaVista_Reporteador\CapaVista_Reporteador.csproj" || goto :fallo
call :compilar "%C%\navegador\Navegador\CapaVista_Navegador\CapaVista_Navegador.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Autorizacion_OrdenesCompraBancos\CapaVista_AutorizacionOrdenesBancos\CapaVista_AutorizacionOrdenesBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Cierre_BancarioBancos\CapaVista_CierreBancarioBancos\CapaVista_CierreBancarioBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Conciliacion_BancariaBancos\CapaVista_ConciliacionBancariaBancos\CapaVista_ConciliacionBancariaBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Disponibilidad_DiariaBancos\CapaVista_DisponibilidadDiariaBancos\CapaVista_DisponibilidadDiariaBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Enlace_Otros_ModulosBancos\CapaVista_EnlaceBancos\CapaVista_EnlaceBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Generacion_PagosBancos\CapaVista_GeneracionPagosBancos\CapaVista_GeneracionPagosBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Movimientos_BancariosBancos\CapaVista_MovimientosBancariosBancos\CapaVista_MovimientosBancariosBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Tipo_CambioBancos\CapaVista_TipoCambioBancos\CapaVista_TipoCambioBancos.csproj" || goto :fallo
call :compilar "%MOD%\bancos\Bibliotecas de Bancos\Bancos\CapaVista_Bancos\CapaVista_Bancos.csproj" || goto :fallo
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Componentes\CapaVista_Componentes\CapaVista_Componentes.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Herramientas\CapaVista_Herramientas\CapaVista_Herramientas.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\AprobacionFinanciera\CapaVista_AprobacionFinanciera\CapaVista_AprobacionFinanciera.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\CierreMovimientos\CapaVista_CierreMovimientos\CapaVista_CierreMovimientos.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\MovimientosProveedores\CapaVista_MovimientosProveedores\CapaVista_MovimientosProveedores.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\OrdenesCompra\CapaVista_OrdenesCompra\CapaVista_OrdenesCompra.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\PagosProveedores\CapaVista_PagosProveedores\CapaVista_PagosProveedores.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Polizas\CapaVista_Polizas\CapaVista_Polizas.csproj" COMPRAS || goto :fallo
call :compilar "%MOD%\compras\Bibliotecas de clase Compras\Compras\CapaVista_Compras\CapaVista_Compras.csproj" COMPRAS || goto :fallo
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%B%\ActivosFijos_Contabilidad\CapaVista_ActivosFijos_Contabilidad\CapaVista_ActivosFijos_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\CierreContable_Contabilidad\CapaVista_CierreContable_Contabilidad\CapaVista_CierreContable_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\EstadosFinancieros_Contabilidad\CapaVista_EstadosFinancieros_Contabilidad\CapaVista_EstadosFinancieros_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\PolizasCuentas_Contabilidad\CapaVista_PolizasCuentas_Contabilidad\CapaVista_PolizasCuentas_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\Presupuestos_Contabilidad\CapaVista_Presupuestos_Contabilidad\CapaVista_Presupuestos_Contabilidad.csproj" || goto :fallo
call :compilar "%B%\Contabilidad\CapaVista_Contabilidad\CapaVista_Contabilidad.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\CierreInventario_Logistica\CapaVista_CierreInventario_Logistica\CapaVista_CierreInventario_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Enlace_OtrosModulos_Logistica\CapaVista_Enlace_Logistica\CapaVista_Enlace_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Inventario_Logistica\CapaVista_Inventario_Logistica\CapaVista_Inventario_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\MuestreoAuditoria_Logistica\CapaVista_MuestreoAuditoria_Logistica\CapaVista_MuestreoAuditoria_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\TransporteExterno_Logistica\CapaVista_TransporteExterno_Logistica\CapaVista_TransporteExterno_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\TransporteInterno_Logistica\CapaVista_TransporteInterno_Logistica\CapaVista_TransporteInterno_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Traslados_Logistica\CapaVista_Traslados_Logistica\CapaVista_Traslados_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\Logistica\Logistica\CapaVista_Logistica\CapaVista_Logistica.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\AsigCapacitacion\CapaVista_AsigCapacitacion\CapaVista_AsigCapacitacion.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Asistencia\CapaVista_Asistencia\CapaVista_Asistencia.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Cierre de Nominas\CapaVista_CierreNominas\CapaVista_CierreNominas.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Empleados\CapaVista_Empleado\CapaVista_Empleado.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\EvaluacionDesempeño\CapaVista_EvaluacionDesempeño\CapaVista_EvaluacionDesempeño.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Liquidacion_Empleados_Rrhh\CapaVista_Liquidacion_Rrhh\CapaVista_Liquidacion_Rrhh.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\NominasEmpleados\CapaVista_Nomina\CapaVista_Nomina.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\PermisosVacaciones\CapaVista_PermisosVacaciones\CapaVista_PermisosVacaciones.csproj" || goto :fallo
call :compilar "%MOD%\rrhh\Bibliotecas de clase RRHH\Recursos Humanos\CapaVista_Rrhh\CapaVista_Rrhh.csproj" || goto :fallo
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\CierreMovimientos\CapaVista_CierreMovimientos\CapaVista_CierreMovimientos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\ComisionVendedores\CapaVista_ComisionVendedores\CapaVista_ComisionVendedores.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Cotizaciones\CapaVista_Cotizaciones\CapaVista_Cotizaciones.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Facturas\CapaVista_Facturas\CapaVista_Facturas.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\ListadoPrecios\CapaVista_ListadoPrecios\CapaVista_ListadoPrecios.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Mantenimientos\CapaVista_Mantenimientos\CapaVista_Mantenimientos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\MovimientosClientes\CapaVista_MovimientosClientes\CapaVista_MovimientosClientes.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Pedidos\CapaVista_Pedidos\CapaVista_Pedidos.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\Reportes\CapaVista_Reportes\CapaVista_Reportes.csproj" VENTAS || goto :fallo
call :compilar "%MOD%\ventas\Biblioteca de clase Ventas Y CxC\VentasYCxC\CapaVista_Ventas\CapaVista_Ventas.csproj" VENTAS || goto :fallo
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%C%\seguridad\Seguridad\CapaVista_Seguridad\CapaVista_Seguridad.csproj" SEGURIDAD || goto :fallo
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
call :compilar "%M%\Ejecucion_Contabilidad\Ejecucion_Contabilidad\Ejecucion_Contabilidad.csproj" || goto :fallo
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 07/11/2026

echo.
echo Listo. Ejecutable:
echo %M%\Ejecucion_Contabilidad\Ejecucion_Contabilidad\bin\Debug\Ejecucion_Contabilidad.exe
echo.
pause
exit /b 0

:compilar
for %%F in ("%~dp1..") do set "SOL=%%~fF\\"
:: Inicio de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
:: La propiedad extra llega por nombre de variable para no perder comillas ni ;
set "EXTRA="
if not "%~2"=="" set "EXTRA=!%~2!"
:: Sin compilador compartido ni nodos reutilizados: no deben quedarse con el log abierto
"%MSB%" "%~1" -restore -nodeReuse:false -p:UseSharedCompilation=false -p:RestorePackagesConfig=true "-p:SolutionDir=!SOL!" -p:Configuration=Debug -v:quiet -nologo !EXTRA! >"%TEMP%\compilar_contabilidad.log" 2>&1
:: Fin de cambio: Gabriel Andre Guillen Pocon - 0901-23-1998 - 11/11/2026
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
