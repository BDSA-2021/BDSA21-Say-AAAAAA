param location string
param sqlServerName string
param sqlAzureAdLogin string
param sqlAzureAdPrincipalType string
param sqlAzureAdPrincipalId string
param sqlDatabaseName string

resource sqlServer 'Microsoft.Sql/servers@2021-05-01-preview' = {
  name: sqlServerName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      login: sqlAzureAdLogin
      principalType: sqlAzureAdPrincipalType
      sid: sqlAzureAdPrincipalId
      tenantId: subscription().tenantId
    }
    minimalTlsVersion: '1.2'
  }

  resource firewallRules 'firewallRules' = {
    name: 'AllowAzureIps'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }

  resource sqlDatabase 'databases' = {
    name: sqlDatabaseName
    location: location
    sku: {
      name: 'Basic'
      tier: 'Basic'
      capacity: 5
    }
    properties: {}
  }
}

output sqlServerUrl string = sqlServer.properties.fullyQualifiedDomainName
