# BDSA21-Say-AAAAAA
BDSA2021 final project

This app has been deployed to an Azure app service [here](https://app.ituwu.dk) or [here](https://selearningapp.azurewebsites.net)

## Build and run
For both docker-compose setups a few files are needed, in WSL or linux/mac run the command below to generate these files.
```
sh scripts/docker-compose-init.sh
```
The files are:
- `db_password.txt`: A password to use for the MSSQL server database
- `connection_string.txt`: Connection string for the program to use, contains the db_password contents
- `cert_password.txt`: Certificate password
- `cert.pfx`: A HTTPS certifcate, locked with the certificate password

### Local development
For manual setup see [docs/manual-setup.md](./docs/manual-setup.md)

To run a database and the program run
```
docker-compose -f docker-compose.dev.yml up
```
The command above will create a database server, a database and build the app for development as well as serving it on `https://localhost:5001`

In case the newest changes arent included use:
```
docker-compose -f docker-compose.dev.yml up --force-recreate --build
```

### Production build and run
**WARNING: Building the production image usually takes ~25 minutes due to WebAssembly**

To run a database and the program run
```
docker-compose -f docker-compose.prod.yml up
```
The command above will create a database server, a database and build the app for production as well as serving it on `https://localhost:5001`

In case the newest changes arent included use:
```
docker-compose -f docker-compose.prod.yml up --force-recreate --build
```

## Documentation
We have embedded some examples and explanations in source code documentation comments, following [the guidelines from the official docs](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#seealso).

Unfortunately, many of the extra suggested tags do not show up when hovering over the symbol name in Visual Studio Code or Visual Studio. For example, see `SELearning.Infrastructure.Authorization.AuthorizePermissionAttribute`.

## Known issues
### Recursive page embedding
New content has to have the url format `https://youtube.com/embed/{id}`, taking a normal youtube url such as `https://www.youtube.com/watch?v=dQw4w9WgXcQ` format it as `https://youtube.com/embed/dQw4w9WgXcQ`. If a valid url is not pasted it will recursively embed the page inside itself.