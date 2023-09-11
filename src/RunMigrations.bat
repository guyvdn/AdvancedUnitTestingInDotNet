pushd %~dp0
dotnet build WeatherService.Migrations\WeatherService.Migrations.csproj -c Release 
WeatherService.Migrations\bin\Release\net7.0\WeatherService.Migrations.exe --ConnectionString "Data Source=.;Initial Catalog=Weather;Integrated Security=SSPI;TrustServerCertificate=True;"
popd
pause