# BDSA21-Say-AAAAAA
BDSA2021 final project

## Local development
[Shamelessly ripped from ondfisk](https://github.com/ondfisk/BDSA2021/blob/main/Notes.md)

Run sql server in docker:
```
$password = New-Guid
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$database = "SELearning"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"
```

## Enable User Secrets
```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:SELearning" "$connectionString"
```

## Set enviroment to development
```
$Env:ASPNETCORE_ENVIRONMENT = "Development" (Windows powershell)
```