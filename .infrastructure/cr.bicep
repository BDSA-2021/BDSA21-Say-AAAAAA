param location string
param containerRegistryName string

param appId string
param appPrincipal string

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2021-06-01-preview' = {
  name: containerRegistryName
  location: location
  sku: {
    name: 'Basic'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    adminUserEnabled: true
    anonymousPullEnabled: false
    policies: {
      retentionPolicy: {
        days: 7
        status: 'disabled'
      }
    }
  }
}

resource webAppToContainerRegistryRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-08-01-preview' = {
  name: guid(appId, containerRegistry.id)
  scope: containerRegistry
  properties: {
    principalId: appPrincipal 
    principalType: 'ServicePrincipal'
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')
  }
}
