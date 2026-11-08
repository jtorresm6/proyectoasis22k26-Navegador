$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Windows.Forms
[void][Reflection.Assembly]::LoadFrom((Join-Path $PSScriptRoot '../CapaVista_Componentes/bin/Debug/CapaVista_Componentes.dll'))
Add-Type -TypeDefinition @"
using System;
using System.Runtime.InteropServices;
public static class ComprasPruebaTeclado {
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr SendMessage(IntPtr h, int m, IntPtr w, IntPtr l);
}
"@
$form = New-Object System.Windows.Forms.Form
$form.Opacity = 0
$form.ShowInTaskbar = $false
$c = New-Object CapaVista_Componentes.Componentes.ComprasUsrTextBox
$form.Controls.Add($c)
try {
    $form.Show()
    $c.Entrada.Focus() | Out-Null
    [System.Windows.Forms.Application]::DoEvents()
    foreach ($mode in @('Texto','Enteros','Moneda')) {
        $c.TipoEntrada = [CapaVista_Componentes.Componentes.ComprasEnumTipoEntrada]::$mode
        $c.Text = ''
        $c.Entrada.Focus() | Out-Null
        [System.Windows.Forms.Application]::DoEvents()
        $value = if ($mode -eq 'Enteros') { '550' } else { '5.50' }
        foreach ($ch in $value.ToCharArray()) {
            [void][ComprasPruebaTeclado]::SendMessage($c.Entrada.Handle, 0x0102, [IntPtr][int]$ch, [IntPtr]::Zero)
            [System.Windows.Forms.Application]::DoEvents()
        }
        $expected = if ($mode -eq 'Moneda') { 'Q 5.50' } else { $value }
        if ($c.Text -ne $expected) { throw "${mode}: expected '$expected', got '$($c.Text)'" }
        Write-Output "PASS ${mode}: $($c.Text)"
        $c.SelectAll()
        foreach ($ch in '12'.ToCharArray()) {
            [void][ComprasPruebaTeclado]::SendMessage($c.Entrada.Handle, 0x0102, [IntPtr][int]$ch, [IntPtr]::Zero)
            [System.Windows.Forms.Application]::DoEvents()
        }
        $expected = if ($mode -eq 'Moneda') { 'Q 12' } else { '12' }
        if ($c.Text -ne $expected) { throw "${mode}: replacement expected '$expected', got '$($c.Text)'" }
        Write-Output "PASS ${mode} replacement: $($c.Text)"
    }
} finally {
    $form.Dispose()
}
