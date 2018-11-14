provider "azurerm" {}

resource "azurerm_resource_group" "functions_sample" {
  name     = "functions-sample-resource-group"
  location = "japaneast"

  tags {
    environment = "${var.environment}"
  }
}
