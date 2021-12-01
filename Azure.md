# Azure setup using CLI
## Create resource group
```bash
az group create --name SELearning --location "North Europe"
```

## Create database server
```
az sql server create --name selearning \
  --resource-group SELearning \
  --location "North Europe" \
  --admin-user selearning \
  --admin-password <db-password>
```

### Open the firewall for other azure resources
```
az sql server firewall-rule create --resource-group SELearning \
  --server selearning \
  --name AllowAzureIps \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0
```

### Tempoarily allow local computer access
```
az sql server firewall-rule create --name AllowLocalClient \
  --server selearning \
  --resource-group SELearning \
  --start-ip-address=<your ip address> \
  --end-ip-address=<your ip address>
```

## Create database
```
az sql db create --resource-group SELearning \
  --server selearning \
  --name SELearning \
  --service-objective Basic
```

### Get connections tring
```
az sql db show-connection-string \
  --client ado.net \
  --server selearning \
  --name SELearning
```

## Create Azure Container Registry
```
az acr create \
  --name SELearningRegistry \
  --resource-group SELearning \
  --sku Basic \
  --admin-enabled true
```

### Retrieve creds
```
az acr credential \
  --show \
  --resource-group SELearning \
  --name SELearningRegistry
```

### Login into docker
```
docker login selearningregistry.azurecr.io \
  --username SELearningRegistry
```

### Tag local build
```
docker tag selearning-image selearningregistry.azurecr.io/selearning-image:latest
```

### Push
```
docker push selearningregistry.azurecr.io/selearning-image:latest
```

## Create App Service
### Create App Service plan
```
az appservice plan create --name SELearningServicePlan \
  --resource-group SELearning \
  --sku FREE \
  --is-linux
```

### Create web app
```
az webapp create --resource-group SELearning \
  --plan SELearningServicePlan \
  --name SELearningApp \
  --deployment-container-image-name selearningregistry.azurecr.io/selearning-image:latest
```

### Set connection string
```
az webapp config appsettings set \
  --resource-group SELearning \
  --name SELearningApp \
  --settings ConnectionStrings__ProductionConnectionString="Server=selearning.database.windows.net,1433;Database=SELearning;Authentication=Active Directory Default"
```

### Set the port of the container
```
az webapp config appsettings set \
  --resource-group SELearning \
  --name SELearningApp \
  --settings WEBSITES_PORT=80
```

### Allow access through managed identity
```
az webapp identity assign \
  --resource-group SELearning \
  --name SELearningApp \
  --query principalId \
  --output tsv
```
Outputs: 5673272c-ded0-4f75-9841-507345edc891

### Retrieve subscription id
```
az account show \
  --query id \
  --output tsv
```
Outputs: 09ee5070-1982-4c1d-8007-7caf5f27d795

### Grant access to the container Registry
```
az role assignment create \
  --assignee <principal-id> \
  --scope /subscriptions/<subscription-id>/resourceGroups/selearning/providers/Microsoft.ContainerRegistry/registries/<registry-name> \
  --role "AcrPull"
```

### Use the managed identity to pull from Acr
```
az resource update \
  --ids /subscriptions/<subscription-id>/resourceGroups/selearning/providers/Microsoft.Web/sites/SELearningApp/config/web \
  --set properties.acrUseManagedIdentityCreds=True
```

## Deploy the image
```
az webapp config container set \
  --name SELearningApp \
  --resource-group SELearning \
  --docker-custom-image-name selearningregistry.azurecr.io/selearning-image:latest \
  --docker-registry-server-url https://selearningregistry.azurecr.io
```

### Enable container logs (OPTIONAL)
```
az webapp log config \
  --name SELearningApp \
  --resource-group SELearning \
  --docker-container-logging filesystem
```

### See log stream
```
az webapp log tail \
  --name SELearningApp \
  --resource-group SELearning
```
