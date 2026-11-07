$ErrorActionPreference = 'Stop'
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'ComprasUsrTextBox.repaired.cs') -Destination (Join-Path $PSScriptRoot '../../Componentes/CapaVista_Componentes/Componentes/ComprasUsrTextBox.cs')
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'ClsPresentacionCompras.repaired.cs') -Destination (Join-Path $PSScriptRoot '../../Componentes/CapaVista_Componentes/Componentes/ClsPresentacionCompras.cs')
& 'C:/Program Files/Microsoft Visual Studio/2022/Community/MSBuild/Current/Bin/MSBuild.exe' (Join-Path $PSScriptRoot '../../Componentes/CapaVista_Componentes/CapaVista_Componentes.csproj') /t:Build /p:Configuration=Debug /nologo /verbosity:minimal
exit $LASTEXITCODE
