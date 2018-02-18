# WebApi-Deploy-Sample
## Compilación
## Despliegue en Azure
### Preparar el entorno
- Instalar herramientas de línea de comandos [ver](https://docs.microsoft.com/es-es/cli/azure/install-azure-cli?view=azure-cli-latest)
- Usar plantillas ARM
   - [Plantilla base](https://azure.microsoft.com/es-es/resources/templates/201-web-app-sql-database/)
   - [Gestión de parámetros](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-templates-parameters)
   - [Artículo de como generar las credenciales de SQl Server](https://www.vivien-chevallier.com/Articles/deploying-a-web-app-and-an-azure-sql-database-with-arm-template-without-providing-any-password)
   - [En las plantillas se pueden usar funciones](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-template-functions-string) aunque con ciertas limitaciones y en los archivos de parámetros no :(
   - [Aquí algo de información de como "depurar" errores](https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-manager-troubleshoot-tips)
- Ejecutar comandos:
```
az login

az group create --name dnz-webapi --location "West Europe"

az group deployment create `
    --name DnzWebApiDeployment `
    --resource-group dnz-webapi `
    --template-file azuredeploy.json `
```