provider "azurerm" {}

resource "azurerm_resource_group" "functions_sample" {
  name     = "${var.project_prefix}-resource-group"
  location = "${var.main_region}"

  tags {
    environment = "${var.environment}"
  }
}
