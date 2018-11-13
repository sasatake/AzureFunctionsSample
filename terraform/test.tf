provider "azurerm" {}

resource "azurerm_resource_group" "functions_sample" {
  name     = "functions-sample-resource-group"
  location = "japaneast"

  tags {
    environment = "${var.environment}"
  }
}

resource "azurerm_api_management" "functions_sample" {
  name                = "functions-sample-apim"
  resource_group_name = "${azurerm_resource_group.functions_sample.name}"
  location            = "${azurerm_resource_group.functions_sample.location}"
  publisher_name      = "Test Company"
  publisher_email     = "company@terraform.io"

  sku {
    name     = "Developer"
    capacity = 1
  }

  tags {
    environment = "${var.environment}"
  }
}

resource "azurerm_storage_account" "functions_sample" {
  name                     = "functionssamplestorage"
  resource_group_name      = "${azurerm_resource_group.functions_sample.name}"
  location                 = "${azurerm_resource_group.functions_sample.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags {
    environment = "${var.environment}"
  }
}

resource "azurerm_app_service_plan" "functions_sample" {
  name                = "functions-sample-service-plan"
  resource_group_name = "${azurerm_resource_group.functions_sample.name}"
  location            = "${azurerm_resource_group.functions_sample.location}"
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "functions_sample" {
  name                      = "functions-sample-functions"
  resource_group_name       = "${azurerm_resource_group.functions_sample.name}"
  location                  = "${azurerm_resource_group.functions_sample.location}"
  app_service_plan_id       = "${azurerm_app_service_plan.functions_sample.id}"
  storage_connection_string = "${azurerm_storage_account.functions_sample.primary_connection_string}"
}
