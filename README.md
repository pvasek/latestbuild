# DevOps Latest build

This is simple function that make a request to the https://devops.azure.com to get the latest build for the given project, build definition and artifact name.

It does two API calls. One to get list of latest builds and second to get the artifact for the latest build.

You can use it here:
```
https://devopslatestbuild.azurewebsites.net/api/v1/{company}/{project}/{buildDefinitionId}/{artifactName}
```