param location string = resourceGroup().location
param sqlServerName string
param sqlAzureAdLogin string
param sqlAzureAdPrincipalType string = 'Group'
param sqlAzureAdPrincipalId string
param sqlDatabaseName string

param appServicePlanName string
param appName string

param hostName string
param certificateName string

param containerRegistryName string

module sqlModule './sql.bicep' = {
  name: 'sqlDeploy'
  params: {
    location: location
    sqlServerName: sqlServerName
    sqlAzureAdLogin: sqlAzureAdLogin
    sqlAzureAdPrincipalId: sqlAzureAdPrincipalId
    sqlAzureAdPrincipalType: sqlAzureAdPrincipalType
    sqlDatabaseName: sqlDatabaseName
  }
}

module appModule './app.bicep' = {
  name: 'appDeploy'
  params: {
    location: location
    appServicePlanName: appServicePlanName
    appName: appName
    hostName: hostName
    certificateName: certificateName
    
    sqlServer: sqlModule.outputs.sqlServerUrl
    sqlDatabase: sqlDatabaseName
  }
}

module containerRegistryModule './cr.bicep' = {
  name: 'containerRegistryDeploy'
  params: {
    location: location
    containerRegistryName: containerRegistryName
    
    appId: appModule.outputs.appId
    appPrincipal: appModule.outputs.appPrincipal
  }
}
