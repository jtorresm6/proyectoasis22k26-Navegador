$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms
$ruta = Join-Path $PSScriptRoot 'CapaVista_Compras/bin/Debug'
[Reflection.Assembly]::LoadFrom((Join-Path $ruta 'CapaVista_Compras.dll')) | Out-Null
$form = New-Object CapaVista_Compras.FrmBitacoraCompras
try {
    $flags = [Reflection.BindingFlags]'Instance, NonPublic'
    $tipo = $form.GetType()
    $lista = New-Object 'System.Collections.Generic.List[CapaControlador_Seguridad.ClsModeloBitacora]'
    $fila = New-Object CapaControlador_Seguridad.ClsModeloBitacora
    $fila.IdBitacora = 1
    $fila.NombreUsuario = 'UsuarioPrueba'
    $fila.AccionBitacora = 'Insertar'
    $fila.TablaBitacora = 'tblOrdenCompra'
    $fila.DetallesBitacora = 'Orden de prueba'
    $fila.FechaHoraBitacora = [DateTime]'2026-11-07 14:30:00'
    $lista.Add($fila)
    $tipo.GetField('_listaBitacoraCompleta', $flags).SetValue($form, $lista)
    $buscar = $tipo.GetField('ComprasTxtBuscar', $flags).GetValue($form)
    $tabla = $tipo.GetField('ComprasDgvBitacora', $flags).GetValue($form)
    $filtrar = $tipo.GetMethod('AplicarFiltro', $flags)
    foreach ($criterio in @('', 'USUARIOPRUEBA', 'insertar', 'tblOrdenCompra', '07/11/2026', '2026-11-07', 'sin coincidencias')) {
        $buscar.Text = $criterio
        $filtrar.Invoke($form, @()) | Out-Null
        $esperados = if ($criterio -eq 'sin coincidencias') { 0 } else { 1 }
        if ($tabla.DataSource.Count -ne $esperados) { throw "Falló filtro: $criterio" }
    }
    $buscar.Text = ''
    $filtrar.Invoke($form, @()) | Out-Null
    if (!$tabla.ReadOnly -or $tabla.AllowUserToAddRows -or $tabla.AllowUserToDeleteRows) { throw 'La tabla debe ser de solo lectura.' }
    $recurso = $tipo.Assembly.GetManifestResourceStream('CapaVista_Compras.Reportes.RpReporteBitacora.rdlc')
    if ($null -eq $recurso) { throw 'Falta el reporte incrustado.' }
    $recurso.Dispose()
    $encabezado = $tipo.GetField('ComprasEncabezado', $flags).GetValue($form)
    $icono = $encabezado.Controls.Find('ComprasPbxIcono', $true)[0]
    $botonReporte = $encabezado.Controls.Find('ComprasBtnReportes', $true)[0]
    if ($null -eq $icono.BackgroundImage -and $null -eq $icono.Image) { throw 'Falta el icono del encabezado.' }
    if ($null -eq $botonReporte.BackgroundImage -and $null -eq $botonReporte.Image) { throw 'Falta el icono de reportes.' }
    $archivoReporte = Join-Path $ruta $encabezado.RutaReporte
    if (!(Test-Path -LiteralPath $archivoReporte)) { throw 'No se copio el RDLC a la carpeta de salida.' }
    Write-Output 'OK: iconos del encabezado y RDLC en la carpeta de salida.'
    $handleFormulario = $form.Handle
    function CrearControles($control) {
        $handleControl = $control.Handle
        foreach ($hijo in $control.Controls) { CrearControles $hijo }
        $control.PerformLayout()
    }
    CrearControles $form
    $form.PerformLayout()
    $bitmap = New-Object Drawing.Bitmap($form.Width, $form.Height)
    try {
        $form.Controls[0].DrawToBitmap($bitmap, (New-Object Drawing.Rectangle(0, 0, $form.ClientSize.Width, $form.ClientSize.Height)))
        $bitmap.Save((Join-Path $PSScriptRoot 'BitacoraCompras-preview.png'))
    } finally { $bitmap.Dispose() }
    Write-Output 'OK: filtros, solo lectura, reporte incrustado y render del formulario.'
} finally { $form.Dispose() }
