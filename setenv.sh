#!/bin/bash

id_list=$(az account show)

subscriptionId=$(echo $id_list | jq -r ".id")
tenantId=$(echo $id_list | jq -r ".tenantId")

echo "create service-principal"
service_principal=$(az ad sp create-for-rbac \
  --role="Contributor" \
  --scopes="/subscriptions/${subscriptionId}")

appId=$(echo $service_principal | jq -r ".appId")
password=$(echo $service_principal | jq -r ".password")

echo "subscriptionId: "$subscriptionId
echo "tenantId: "$tenantId
echo "appId: "$appId

cat <<EOF > .env
echo "Setting environment variables for Terraform"
export ARM_SUBSCRIPTION_ID=$subscriptionId
export ARM_CLIENT_ID=$appId
export ARM_CLIENT_SECRET=$password
export ARM_TENANT_ID=$tenantId
EOF