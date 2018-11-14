# resource "azurerm_api_management" "functions_sample" {
#   name                = "functions-sample-apim"
#   resource_group_name = "${azurerm_resource_group.functions_sample.name}"
#   location            = "${azurerm_resource_group.functions_sample.location}"
#   publisher_name      = "Test Company"
#   publisher_email     = "company@terraform.io"
#   sku {
#     name     = "Developer"
#     capacity = 1
#   }
#   tags {
#     environment = "${var.environment}"
#   }
# }

