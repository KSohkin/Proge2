dotnet tool install -g dotnet-reportgenerator-globaltool
$TestOutput = dotnet test --collect "XPlat Code Coverage" --results-directory ./BuildReports/UnitTests
$TestReportsParts = $TestOutput | Select-String coverage.cobertura.xml | ForEach-Object { $_.Line.Trim() }
$TestReportsCrappy = ($TestReportsParts -join ';')

$guid_regex = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}" 
$TestReports = $TestReportsCrappy -replace $guid_regex
$TestReports = $TestReports.Replace("//","/").Replace('\\','\') #.Replace("\\UnitTests","\\Coverage")

copy $TestReportsCrappy $TestReports
del $TestReportsCrappy

Get-ChildItem -Path ./BuildReports/UnitTests -Directory -Recurse | Remove-Item -Force  

reportgenerator "-reports:$TestReports" "-targetdir:.//BuildReports//Coverage" "-reporttype:Html" "-classfilters:-AspNetCoreGeneratedDocument.*"

start "BuildReports\Coverage\index.htm"