variable "location" {
  default = "eastus"
}

variable "resource_group_name" {
  default = "resource.coder.codinginvest.com"
}

variable "acr_name"{
  default = "codinginvest"
}

variable "acr_resource_group_name"{
  default = "resources.codinginvest.com"
}

variable "client_image"{
  default = "codinginvest.azurecr.io/client:latest"
}

variable "server_image"{
  default = "codinginvest.azurecr.io/server:latest"
}

variable "custom_domain"{
  default = "coder.codinginvest.com"
}

variable "custom_domain_enabled"{
  type = bool
  default = false
}

variable "subscription_id" {
}
variable "tenant_id" {
}
variable "client_id" {
}
variable "client_secret" {
  sensitive = true
}
