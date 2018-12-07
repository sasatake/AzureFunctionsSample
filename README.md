# Azure Functions Sample

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

## deploy from local

- construct services and deploy source code.

```
sh deploy.sh init
```

- only construct services.

```
sh deploy.sh terraform
```

- destruct services.

```
sh deploy.sh rollback.
```

- only deploy source code.

```
sh deploy.sh
```

## execute locally

## unit test
