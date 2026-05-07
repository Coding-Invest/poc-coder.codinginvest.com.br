terraform {
  backend "azurerm" {
    resource_group_name  = "resources.codinginvest.com"
    storage_account_name = "tfstatecodercodinginvest" 
    container_name       = "tfstate"
    key                  = "infra-prod.tfstate"
  }
}