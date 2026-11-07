$ErrorActionPreference = 'Stop'
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'actualizado_ClsControladorMoneda.fixed.cs') -Destination (Join-Path $PSScriptRoot '../../Herramientas/CapaControlador_Herramientas/ClsControladorMoneda.cs')
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'actualizado_UsrMonedaVisualizacion.fixed.cs') -Destination (Join-Path $PSScriptRoot '../../Herramientas/CapaVista_Herramientas/Controles/UsrMonedaVisualizacion.cs')
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'actualizado_UsrMonedaVisualizacion.Designer.fixed.cs') -Destination (Join-Path $PSScriptRoot '../../Herramientas/CapaVista_Herramientas/Controles/UsrMonedaVisualizacion.Designer.cs')
& (Join-Path $PSScriptRoot 'RepararHerramientas.ps1')
exit $LASTEXITCODE
