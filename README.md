# BDSA21-Say-AAAAAA
BDSA2021 final project

## Local development
[Shamelessly ripped from ondfisk](https://github.com/ondfisk/BDSA2021/blob/main/Notes.md)

Run sql server in docker (Windows):
```
$password = New-Guid
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$database = "SELearning"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"
```

Run sql server in docker (Linux):
```
export password=$(uuidgen)
sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
export database=SELearning
export connectionString=$(echo "Server=localhost;Database=${database};User Id=sa;Password=${password}")
```

## Enable User Secrets (In the SELearning project)
```powershell
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:SELearning" "$connectionString"
```

## Migrate database (In the SELearning.Infrastructure project)
```
dotnet tool install --global --version 6.0.0 dotnet-ef
dotnet ef database update --startup-project ../SELearning
```

## Set enviroment to development
```
$Env:ASPNETCORE_ENVIRONMENT = "Development" (Windows powershell)
export ASPNETCORE_ENVIROMENT="Development"  (Linux)
```