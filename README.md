# Azure Functions Sample

## required

- [Azure CLI](https://docs.microsoft.com/ja-jp/cli/azure/?view=azure-cli-latest)
- [Azure Functions Core Tools](https://docs.microsoft.com/ja-jp/azure/azure-functions/functions-run-local)
- [Terraform](https://www.terraform.io/docs/commands/index.html)
- [jq](https://stedolan.github.io/jq/)

## setup

- Login to Azure.

```
az login
```

- Create environment file(.env) to construct services by Terraform.
  - Depends on `jq` command.

```
sh setenv.sh
```

## execute locally

```
func host start
```

## deploy from local

- deploy source code.

```
sh deploy.sh
```

## unit test

- Under Construct
