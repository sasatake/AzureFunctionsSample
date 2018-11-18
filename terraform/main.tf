provider "azurerm" {}

resource "azurerm_resource_group" "functions_sample" {
  name     = "functions-sample-resource-group"
  location = "${var.main_region}"

  tags {
    environment = "${var.environment}"
  }
}
