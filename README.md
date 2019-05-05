# Latest build

This is a simple app (Azure function app) that make a request to the https://devops.azure.com to get the latest build for the given project, build definition and artifact name.

It does two API calls. The first to get list of latest builds and the second to get the artifact for the latest build. Finally it redirects you to that build.

The build needs to be publicly available.

You can use it in the following fashion:
```
https://latestbuild.azurewebsites.net/api/v1/devops/{company}/{project}/{buildDefinitionId}/{artifactName}
```

Example:
```
https://latestbuild.azurewebsites.net/api/v1/devops/pvasek/sourcekit-lsp-vscode-extension/1/sourcekit-lsp-vscode-dev.vsix
```