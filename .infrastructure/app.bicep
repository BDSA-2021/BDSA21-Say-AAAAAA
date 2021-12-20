param location string
param appServicePlanName string
param appName string
param hostName string
param certificateName string

param sqlServer string
param sqlDatabase string

resource servicePlan 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
    size: 'B1'
    family: 'B'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    reserved: true
    isXenon: false
    hyperV: false
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
}

resource webApp 'Microsoft.Web/sites@2021-02-01' = {
  name: appName
  location: location
  kind: 'app,linux,container'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    httpsOnly: true
    reserved: true
    serverFarmId: servicePlan.id
    enabled: true
    hostNameSslStates: [
      {
        name: 'app.ituwu.dk'
        sslState: 'SniEnabled'
        thumbprint: 'E17148A9F582A0B3B0661F103B3EC7A565EB2BDC'
        hostType: 'Standard'
      }
      {
        name: 'selearningapp.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: 'selearningapp.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    customDomainVerificationId: 'BD89702B2DBD69E3327D4CD217BA87EB8A2851D82B9E35DC470885D0E0AC4FE3'
    clientAffinityEnabled: true
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
  }

  resource hostNameBinding 'hostNameBindings' = {
    name: hostName
    properties: {
      siteName: appName
      hostNameType: 'Verified'
      sslState: 'SniEnabled'
      thumbprint: 'E17148A9F582A0B3B0661F103B3EC7A565EB2BDC'
    }
  }

  resource web 'config' = {
    name: 'web'
    properties: {
      ftpsState: 'Disabled'
      http20Enabled: true
      minTlsVersion: '1.2'
      scmMinTlsVersion: '1.0'
      netFrameworkVersion: 'v6.0'
      acrUseManagedIdentityCreds: true
      httpLoggingEnabled: true
      healthCheckPath: '/healthz'
    }
  }

  resource appSettings 'config' = {
    name: 'appsettings'
    properties: {
      ASPNETCORE_URLS: 'http://+'
      ASPNETCORE_ENVIRONMENT: 'Production'
      WEBSITE_HTTPLOGGING_RETENTION_DAYS: '3'
      WEBSITES_ENABLE_APP_SERVICE_STORAGE: 'false'
      WEBSITES_PORT: '80'
    }
  }

  resource connectionStrings 'config' = {
    name: 'connectionstrings'
    properties: {
      ProductionConnectionString: {
        value: 'Server=${sqlServer},1433;Initial Catalog=${sqlDatabase};Authentication=Active Directory Managed Identity'
        type: 'SQLAzure'
      }
    }
  }
}

resource certificate 'Microsoft.Web/certificates@2021-02-01' = {
  name: certificateName
  location: location
  properties: {
    serverFarmId: servicePlan.id
    hostNames: [
      hostName
    ]
    canonicalName: hostName
  }
}

output appId string = webApp.id
output appPrincipal string = webApp.identity.principalId
