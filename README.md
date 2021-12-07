# BDSA21-Say-AAAAAA
BDSA2021 final project

This app has been deployed to an Azure app service [here.](https://selearningapp.azurewebsites.net)

## Build and run
### Local development
[Shamelessly ripped from ondfisk](https://github.com/ondfisk/BDSA2021/blob/main/Notes.md)

Assume the location these commands are run from is the root of the project, unless specifically stated.

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
export connectionstring=$(echo "server=localhost;database=${database};user id=sa;password=${password}")
```

#### Enable User Secrets
```powershell
dotnet user-secrets init --project SELearning.API
dotnet user-secrets set "ConnectionStrings:SELearning" "$connectionString" --project SELearning.API
```

#### Migrate database
Note: The migrations are done automatically after the first one. The first migration also creates the database which is necessary for the system to run.
```
dotnet tool install --global --version 6.0.0 dotnet-ef
dotnet ef database update --project SELearning.Infrastructure --startup-project SELearning.API
```

#### Set enviroment to development
```
$Env:ASPNETCORE_ENVIRONMENT = "Development" (Windows powershell)
export ASPNETCORE_ENVIROMENT="Development"  (Linux)
```

#### Run the system
```
dotnet run --project SELearning.API
```
This will build the Blazor project and serve it along with the API

### Production build and run
```
docker-compose up
```
The command above will create a database server, a database and build the app for production as well as serving it on port 7207

## Documentation

We have embedded some examples and explanations in source code documentation comments, following [the guidelines from the official docs](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#seealso).

Unfortunately, many of the extra suggested tags do not show up when hovering over the symbol name in Visual Studio Code or Visual Studio. For example, see `SELearning.Infrastructure.Authorization.AuthorizePermissionAttribute`.
