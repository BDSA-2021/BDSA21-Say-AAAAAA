FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

ENV ASPNETCORE_URLS=http://+:80/

WORKDIR /source

COPY . .

RUN apt-get update && apt-get install -y python3
RUN dotnet workload install wasm-tools
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app
COPY --from=build /app .

EXPOSE 80

ENTRYPOINT ["dotnet", "SELearning.API.dll"]
