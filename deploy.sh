#!/bin/sh

publish_dir="publish/"
zip_file="publish.zip"
project_prefix="user-sample"
resource_group="${project_prefix}-resource-group"
functions_name="${project_prefix}-functions"

source .env

function infra(){
  echo "terraform"
  terraform init terraform/ && \
  terraform plan -var 'project_prefix='${project_prefix} terraform/ && \
  terraform apply -state=./terraform/terraform.tfstate -var 'project_prefix='${project_prefix} -auto-approve terraform/
}

function rollback(){
  echo "rollback"
  terraform destroy -state=./terraform/terraform.tfstate -var 'project_prefix='${project_prefix} -auto-approve terraform/
}

function build(){
  echo "build"
  dotnet restore && dotnet publish -c Release -o ${publish_dir} && \
  cd ${publish_dir} && \
  zip -r ../${zip_file} ./* && \
  cd ../
}

function deploy(){
  echo "deploy"
  az login --service-principal --username $ARM_CLIENT_ID --password $ARM_CLIENT_SECRET --tenant $ARM_TENANT_ID
  az functionapp deployment source config-zip \
    --src ${zip_file} \
    --resource-group ${resource_group} \
    --name ${functions_name} && \
  rm -rf ${publish_dir} ${zip_file}
}

# main

case "${1:-''}" in
  "init") infra && build && deploy ;;
  "terraform") infra ;;
  "rollback") rollback ;;
  * ) build && deploy ;;
esac