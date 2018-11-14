#!/bin/sh

publish_dir="publish/"
zip_file="publish.zip"
resource_group="functions-sample-resource-group"
functions_name="functions-sample-functions"

dotnet restore && dotnet publish -c Release -o ${publish_dir} && \
cd ${publish_dir} && \
zip -r ../${zip_file} ./* && \
cd ../ && \
az functionapp deployment source config-zip \
  --src ${zip_file} \
  --resource-group ${resource_group} \
  --name ${functions_name} && \
rm -rf ${publish_dir} ${zip_file}