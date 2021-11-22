$Projects = Get-Item 'XstarS.*' | ForEach-Object { $_.Name }
$Configuration, $Framework = 'Release', 'net6.0*'
$Pattern = "$PSScriptRoot\{0}\bin\$Configuration\$Framework\{0}.dll"
$Assemblies = $Projects | ForEach-Object { $Pattern -f $_ }
$References = $Assemblies | Get-Item | ForEach-Object { '-r', $_.FullName }
dotnet fsi --langversion:latest @References
