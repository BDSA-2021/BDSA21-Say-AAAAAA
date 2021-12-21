# Manual local development setup
[Shamelessly ripped from ondfisk](https://github.com/ondfisk/BDSA2021/blob/main/Notes.md)

Assume the location these commands are run from is the root of the project, unless specifically stated.

Run sql server in docker (Windows):
```
$password = New-Guid
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$database = "SELearning"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password;TrustServerCertificate=true"
```

Run sql server in docker (Linux):
```
export password=$(uuidgen)
sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
export database=SELearning
export connectionString=$(echo "server=localhost;database=${database};user id=sa;password=${password};TrustServerCertificate=true")
```

#### Enable User Secrets
```powershell
dotnet user-secrets init --project SELearning.API
dotnet user-secrets set "ConnectionStrings:SELearning" "$connectionString" --project SELearning.API
```

#### Migrate database
Note of notes: The below note is not applicable when running the program locally using the docker command described above (i have no idea why)
Note: The migrations are done automatically after the first one. The first migration also creates the database which is necessary for the system to run.
```
dotnet tool install --global --version 6.0.0 dotnet-ef
dotnet ef database update --project SELearning.Infrastructure --startup-project SELearning.API
```

#### Set enviroment to development
```
$Env:ASPNETCORE_ENVIRONMENT = "Development" (Windows powershell)
export ASPNETCORE_ENVIRONMENT="Development"  (Linux)
```

#### Run the system
```
dotnet run --project SELearning.API
```
This will build the Blazor project and serve it along with the API

