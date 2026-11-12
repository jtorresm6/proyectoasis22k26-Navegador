# Compilar Ventas y Cuentas por Cobrar

Descargue o clone el repositorio completo. No copie solamente la carpeta de Ventas:
el modulo compila tambien Consultas, Navegador, Seguridad, Reporteador y Logistica.

Requisitos: Windows y Visual Studio o Build Tools con desarrollo de escritorio
.NET y las herramientas de desarrollo de .NET Framework 4.8.

Ejecute `build_ventas.bat` desde la carpeta superior. Para compilar sin menu:

```bat
build_ventas.bat Debug /sinpausa
build_ventas.bat Release /sinpausa
```

ReportViewer y SQL Server Types se referencian desde los paquetes versionados en
`codigo/componentes/reporteador/BtnVerReporte_Reporteador/packages`. Listado de
precios utiliza el paquete ODBC versionado en `Biblioteca de clase Ventas Y CxC/VentasYCxC/packages`.
No requieren copiar DLL manualmente, instalar ReportViewer en Visual Studio ni
descargar paquetes para ejecutar este BAT. Las rutas son relativas al repositorio.

Si falta una biblioteca de Reportes, la compilacion indica el archivo ausente.
Revise que la descarga incluya los componentes y que corresponda a la version
actualizada del repositorio. No use las carpetas `bin`, `obj` ni paquetes ignorados
de otra computadora para corregir la descarga.

Los mensajes de compilacion se muestran en la consola; el BAT no guarda archivos de log.

## Compilar desde Visual Studio

Puede abrir `Ejecutable_Ventas/Ejecucion_Ventas.sln` o la solucion de un area.
Las soluciones conservan sus propios proyectos MVC y pruebas. Al compilar desde
Visual Studio, `Directory.Build.targets` prepara las bibliotecas compartidas en
el mismo orden que el BAT, incluso si no estan cargadas en la solucion abierta.
La preparacion no se ejecuta durante IntelliSense ni se repite dentro de las
compilaciones internas del BAT. Al compilar el ejecutable tambien se reunen sus
DLL de ejecucion, como al usar el BAT.

El mensaje CS0006 sobre `CapaVista_Ventas.dll` puede ser consecuencia de un error
anterior en otra biblioteca. Revise siempre el primer error de compilacion.
Despues de actualizar el repositorio, recargue la solucion para que Visual Studio
importe la configuracion nueva y utilice **Compilar > Recompilar solucion**.
