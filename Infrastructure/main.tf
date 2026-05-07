resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

resource "azurerm_log_analytics_workspace" "law" {
  name                = "law-containerapps-economico"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "PerGB2018"
  retention_in_days   = 30
}

resource "azurerm_container_app_environment" "env" {
  name                       = "cae-economico"
  location                   = azurerm_resource_group.rg.location
  resource_group_name        = azurerm_resource_group.rg.name
  log_analytics_workspace_id = azurerm_log_analytics_workspace.law.id
}

resource "random_integer" "rand" {
  min = 10000
  max = 99999
}

resource "azurerm_storage_account" "sa" {
  name                     = "stapprepos${random_integer.rand.result}"
  resource_group_name      = azurerm_resource_group.rg.name
  location                 = azurerm_resource_group.rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_share" "repos" {
  name                 = "repos"
  storage_account_id = azurerm_storage_account.sa.id
  quota                = 50
}

resource "azurerm_container_app_environment_storage" "repos" {
  name                         = "repos"
  container_app_environment_id = azurerm_container_app_environment.env.id

  account_name = azurerm_storage_account.sa.name
  share_name   = azurerm_storage_share.repos.name
  access_key   = azurerm_storage_account.sa.primary_access_key

  access_mode = "ReadWrite"
}

data "azurerm_container_registry" "acr" {
  name                = var.acr_name
  resource_group_name = var.acr_resource_group_name
}

resource "azurerm_role_assignment" "containerapp_acr_pull" {
  scope                = data.azurerm_container_registry.acr.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.containerapp.principal_id
}

resource "azurerm_user_assigned_identity" "containerapp" {
  name                = "uai-containerapps"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_container_app" "server" {
  name                         = "server"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

  depends_on = [
    azurerm_container_app_environment_storage.repos
  ]

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.containerapp.id]
  }

  registry {
    server   = data.azurerm_container_registry.acr.login_server
    identity = azurerm_user_assigned_identity.containerapp.id
  }

  template {
    min_replicas = 0
    max_replicas = 1

    container {
      name   = "server"
      image  = var.server_image
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "ASPNETCORE_URLS" #It is needs to define the exact address, when service
        value = "http://0.0.0.0:8080"
      }

      volume_mounts {
        name = "repos"
        path = "/app/repos"
      }
    }

    volume {
      name         = "repos"
      storage_type = "AzureFile"
      storage_name = azurerm_container_app_environment_storage.repos.name
    }
  }

  ingress {
    external_enabled = false
    target_port      = 8080
    transport        = "auto"
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }
}

resource "azurerm_container_app" "client" {
  name                         = "client"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.containerapp.id]
  }

  registry {
    server   = data.azurerm_container_registry.acr.login_server
    identity = azurerm_user_assigned_identity.containerapp.id
  }

  template {
    min_replicas = 0
    max_replicas = 1

    container {
      name   = "client"
      image  = var.client_image
      cpu    = 0.25
      memory = "0.5Gi"

      env {
        name  = "ASPNETCORE_HTTP_PORTS"
        value = "80"
      }

      env {
        name  = "SERVER_BASE_URL"
        value = "http://server/api/JsonRPC"
      }
    }
  }

  ingress {
    external_enabled = true
    target_port      = 80
    transport        = "auto"

    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

}

resource "azurerm_container_app_custom_domain" "coder" {
  count = var.custom_domain_enabled ? 1 : 0
  name             = var.custom_domain
  container_app_id = azurerm_container_app.client.id
  certificate_binding_type = "SniEnabled"
  lifecycle {
    ignore_changes = [certificate_binding_type, container_app_environment_certificate_id]
  }
}

output "client_url" {
  value = azurerm_container_app.client.latest_revision_fqdn
}