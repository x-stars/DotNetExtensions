$Projects = Get-Item 'XstarS.*' | ForEach-Object { $_.Name }
$Configuration, $Framework = 'Release', 'netcoreapp3.1'
$Pattern = "$PSScriptRoot\{0}\bin\$Configuration\$Framework\{0}.dll"
$Assemblies = $Projects | ForEach-Object { $Pattern -f $_ } | Where-Object { Test-Path $_ }
$References = $Assemblies | ForEach-Object { "--reference:""$_""" }
dotnet fsi --langversion:preview @References
