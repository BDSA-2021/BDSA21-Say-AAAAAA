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
  --start-ip-address=80.71.143.84 \
  --end-ip-address=80.71.143.84
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
  --name SELearning \
  --runtime 'DOTNET|6.0'
```
