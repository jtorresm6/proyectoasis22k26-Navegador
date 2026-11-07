$ErrorActionPreference = 'Stop'
$raizHerramientas = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../../Herramientas/CapaVista_Herramientas'))
Get-ChildItem -LiteralPath $raizHerramientas -Filter *.cs -Recurse | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' } | ForEach-Object {
    $texto = [IO.File]::ReadAllText($_.FullName)
    $nuevo = $texto
    $nuevo = $nuevo.Replace('UsrLabelCompras', 'ComprasUsrLabel')
    $nuevo = $nuevo.Replace('UsrDataGridViewCompras', 'ComprasUsrDataGridView')
    $nuevo = $nuevo.Replace('UsrEncabezadoCompras', 'ComprasUsrEncabezado')
    $nuevo = $nuevo.Replace('UsrComboBoxCompras', 'ComprasUsrComboBox')
    $nuevo = $nuevo.Replace('UsrTextBoxCompras', 'ComprasUsrTextBox')
    $nuevo = $nuevo.Replace('UsrRangoFechasCompras', 'ComprasUsrRangoFechas')
    if ($nuevo -ne $texto) { [IO.File]::WriteAllText($_.FullName, $nuevo, [Text.UTF8Encoding]::new($true)) }
}
& 'C:/Program Files/Microsoft Visual Studio/2022/Community/MSBuild/Current/Bin/MSBuild.exe' (Join-Path $PSScriptRoot '../../Herramientas/Herramientas.sln') /t:Build /p:Configuration=Debug /nologo /verbosity:minimal
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
& 'C:/Program Files/Microsoft Visual Studio/2022/Community/MSBuild/Current/Bin/MSBuild.exe' (Join-Path $PSScriptRoot '../../Compras/CapaVista_Compras/CapaVista_Compras.csproj') /t:Build /p:Configuration=Debug /nologo /verbosity:minimal
exit $LASTEXITCODE
