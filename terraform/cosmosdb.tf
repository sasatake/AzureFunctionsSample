resource "azurerm_cosmosdb_account" "db" {
  name                = "${var.project_prefix}-db"
  resource_group_name = "${azurerm_resource_group.functions_sample.name}"
  location            = "${azurerm_resource_group.functions_sample.location}"
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"

  enable_automatic_failover = true

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location          = "${azurerm_resource_group.functions_sample.location}"
    failover_priority = 0
  }
}
