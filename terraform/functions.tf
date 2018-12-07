resource "azurerm_storage_account" "functions_sample" {
  name                     = "${replace(lower(var.project_prefix),"-","")}storage"
  resource_group_name      = "${azurerm_resource_group.functions_sample.name}"
  location                 = "${azurerm_resource_group.functions_sample.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"

  tags {
    environment = "${var.environment}"
  }
}

resource "azurerm_app_service_plan" "functions_sample" {
  name                = "${var.project_prefix}-service-plan"
  resource_group_name = "${azurerm_resource_group.functions_sample.name}"
  location            = "${azurerm_resource_group.functions_sample.location}"
  kind                = "FunctionApp"

  tags {
    environment = "${var.environment}"
  }

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "functions_sample" {
  name                      = "${var.project_prefix}-functions"
  resource_group_name       = "${azurerm_resource_group.functions_sample.name}"
  location                  = "${azurerm_resource_group.functions_sample.location}"
  app_service_plan_id       = "${azurerm_app_service_plan.functions_sample.id}"
  storage_connection_string = "${azurerm_storage_account.functions_sample.primary_connection_string}"
  version                   = "~2"

  tags {
    environment = "${var.environment}"
  }

  app_settings {
    "AppInsights_InstrumentationKey" = "${azurerm_application_insights.functions_sample.instrumentation_key}"
  }
}

resource "azurerm_application_insights" "functions_sample" {
  name                = "${var.project_prefix}-insights"
  resource_group_name = "${azurerm_resource_group.functions_sample.name}"
  location            = "${var.sub_region}"
  application_type    = "Web"

  tags {
    environment = "${var.environment}"
  }
}
