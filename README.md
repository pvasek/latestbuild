# Latest build

This is a simple app (Azure function app) that make a request to the https://devops.azure.com to get the latest build for the given project, build definition and artifact name.

It does two API calls. The first to get list of latest builds and the second to get the artifact download url for the latest build. Finally, it redirects you to the download url of the given artifact.

The build needs to be a publicly available.

The `artifactName` can be either full or partial ending with `*`. In that case, only the beginning of the name has to match.

Examples:

- full: `sourcekit-lsp-vscode-dev.vsix`
- partial: `sourcekit-lsp-vscode-*`

In case it fails because of wrong request (wrong company/project/build definition/artifact name) it returns 412 together with json response:

```json
{ "isOk": false, "error": "<error description>" }
```

You can use it in the following fashion:
```
https://latestbuild.azurewebsites.net/api/v1/devops/{company}/{project}/{buildDefinitionId}/{artifactName}
```

Example:
```
https://latestbuild.azurewebsites.net/api/v1/devops/pvasek/sourcekit-lsp-vscode-extension/1/sourcekit-lsp-vscode-dev.vsix
```