param([ValidateSet('Debug', 'Release')][string]$Configuracion = 'Debug')
$ErrorActionPreference = 'Stop'
$raizOrdenes = Split-Path -Parent $PSScriptRoot
$vswhereOrdenes = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
$compiladorOrdenes = & $vswhereOrdenes -latest -products '*' -find 'MSBuild/**/Bin/Roslyn/csc.exe' | Select-Object -First 1
if (-not $compiladorOrdenes) { throw 'No se encontró el compilador de C# de Visual Studio.' }
$salidaOrdenes = Join-Path $PSScriptRoot "bin/$Configuracion"
New-Item -ItemType Directory -Force -Path $salidaOrdenes | Out-Null
foreach ($capaOrdenes in @('CapaModelo_OrdenesCompra', 'CapaControlador_OrdenesCompra')) {
    Copy-Item -LiteralPath (Join-Path $raizOrdenes "$capaOrdenes/bin/$Configuracion/$capaOrdenes.dll") -Destination $salidaOrdenes -Force
}
Copy-Item -LiteralPath (Join-Path $raizOrdenes "CapaModelo_OrdenesCompra/bin/$Configuracion/CapaModelo_Seguridad.dll") -Destination $salidaOrdenes -Force
$argumentosOrdenes = @('/nologo', '/target:exe', "/out:$salidaOrdenes/ValidarOrdenesCompra.exe", '/r:System.Data.dll',
    "/r:$salidaOrdenes/CapaModelo_OrdenesCompra.dll", "/r:$salidaOrdenes/CapaControlador_OrdenesCompra.dll",
    "/r:$salidaOrdenes/CapaModelo_Seguridad.dll", (Join-Path $PSScriptRoot 'ValidarOrdenesCompra.cs'))
& $compiladorOrdenes @argumentosOrdenes
if ($LASTEXITCODE -ne 0) { throw 'Falló la compilación de las pruebas.' }
& (Join-Path $salidaOrdenes 'ValidarOrdenesCompra.exe')
if ($LASTEXITCODE -ne 0) { throw 'Falló la validación de órdenes de compra.' }
$vistaOrdenes = Join-Path $raizOrdenes "CapaVista_OrdenesCompra/bin/$Configuracion"
Get-ChildItem -LiteralPath $vistaOrdenes -Filter '*.dll' | Copy-Item -Destination $salidaOrdenes -Force
$argumentosVista = @($argumentosOrdenes | Where-Object { $_ -notlike '/out:*' }) + @(
    "/out:$salidaOrdenes/ValidarVistaOrdenesCompra.exe", '/main:ValidarVistaOrdenesCompra', '/platform:x64',
    '/r:System.Drawing.dll', '/r:System.Windows.Forms.dll',
    "/r:$salidaOrdenes/CapaVista_OrdenesCompra.dll", "/r:$salidaOrdenes/CapaVista_Componentes.dll",
    (Join-Path $PSScriptRoot 'ValidarVistaOrdenesCompra.cs'))
& $compiladorOrdenes @argumentosVista
if ($LASTEXITCODE -ne 0) { throw 'Falló la compilación de las pruebas de vista.' }
& (Join-Path $salidaOrdenes 'ValidarVistaOrdenesCompra.exe')
if ($LASTEXITCODE -ne 0) { throw 'Falló la validación de la vista de órdenes de compra.' }
