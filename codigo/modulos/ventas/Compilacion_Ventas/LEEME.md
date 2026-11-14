# Compilar Ventas y Cuentas por Cobrar

Abra la carpeta superior de Ventas y ejecute `build_ventas.bat`. Desde una terminal puede usar:

```bat
build_ventas.bat Debug /sinpausa
build_ventas.bat Release /sinpausa
```

Se requiere Windows, MSBuild de Visual Studio y las herramientas de desarrollo de .NET Framework 4.8. El BAT compila las bibliotecas de Ventas y los componentes compartidos que usa el MDI. Compila el login compartido de Seguridad con las DLL ya compiladas de Bancos, Compras, Contabilidad, Logistica y RR. HH. Puede indicar su carpeta con la variable VentasDllCompartidas.

NuGet restaura los paquetes de Ventas en la cache global del usuario. La salida del ejecutable queda en `../Ejecutable_Ventas/Ejecutable_Ventas/bin/<perfil>/`.

Para abrir el ejecutable en Visual Studio use `../Ejecutable_Ventas/Ejecucion_Ventas.sln`. Las soluciones individuales conservan su propia compilacion y sus pruebas.
