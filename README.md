# proyectoasis22k26
Repositorio oficial proyecto Anàlisis de Sistemas II

## Compilar el proyecto completo

Ejecuta **build_proyecto.bat** desde la raiz del repositorio. Unifica Consultas,
Navegador, Seguridad, Reporteador y todos los modulos de Bancos, incluidos sus
ejecutables. Los .bat anteriores se conservan; no es necesario ejecutarlos antes.

Requiere Visual Studio o Build Tools con MSBuild, las herramientas de escritorio
.NET y los paquetes de destino de .NET Framework utilizados por los proyectos.
La restauracion de paquetes NuGet puede requerir conexion a Internet.

Desde PowerShell: `.\build_proyecto.bat`. Para terminar sin menu:
`.\build_proyecto.bat /auto` (codigo de salida 0 = correcto, 1 = error).

Compila en Debug, ordena las capas y deja los ejecutables para el final. Reintenta
hasta tres rondas para resolver dependencias entre DLL. Conserva la copia de DLL
Debug a bin\Release utilizada por el script de Bancos para referencias existentes;
esas copias no son compilaciones Release. No incluye prototipos.

Los registros quedan en **logs/build_proyecto/**: `build_log.txt` contiene toda
la salida, `fallidos.txt` enumera los fallos de la ultima ronda y los archivos
`ronda_*_paso_*.log` contienen el detalle por proyecto. Los errores de codigo o
referencias ausentes se informan como fallos, aunque existan DLL antiguas.
