using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace DevOpsTools
{
    public static class RedirectToLatestDevOpsBuild
    {
        private static HttpClient _httpClient = new HttpClient();
        
        [FunctionName("RedirectToLatestDevOpsBuild")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/devops/{company}/{projectName}/{definitionId}/{artifactName}")] HttpRequest req,
            string company,
            string projectName,
            string definitionId,
            string artifactName,
            ILogger log)
        {
            log.LogInformation("Get latest build for company: {company}, projectName: {projectName}, definitionId: {definitionId}, artifactName: {artifactName}", 
                company, projectName, definitionId, artifactName);

            company = WebUtility.UrlEncode(company);
            projectName = WebUtility.UrlEncode(projectName);
            definitionId = WebUtility.UrlEncode(definitionId);
            artifactName = WebUtility.UrlEncode(artifactName);

            var getLatestBuildsUrl = $"https://dev.azure.com/{company}/{projectName}/_apis/build/builds?properties=id&$top=3&definitions={definitionId}&queryOrder=finishTimeDescending&api-version=4.1";
            var latestBuildResponse = await _httpClient.GetAsync(getLatestBuildsUrl);
            if (latestBuildResponse.IsSuccessStatusCode == false) 
            {
                return GetError($"Getting the builds failed with response: {latestBuildResponse.StatusCode}", log);
            }

            var builds = await latestBuildResponse.Content.ReadAsAsync<BuildResult>();

            if (builds.Value.Count == 0) 
            {
                return GetError("There is no build for this build definition yet.", log);
            }

            var buildId = builds.Value[0].Id;
            var getArtifactUrl = $"https://dev.azure.com/{company}/{projectName}/_apis/build/builds/{buildId}/artifacts?artifactName={artifactName}&api-version=4.1";            
            var artifcatsResponse = await _httpClient.GetAsync(getArtifactUrl);

            if (artifcatsResponse.IsSuccessStatusCode == false) 
            {
                return GetError($"Getting build artifacts fails with response: {artifcatsResponse.StatusCode}", log);
            }

            var artifacts = await artifcatsResponse.Content.ReadAsAsync<ArtifactResult>();
            
            if (string.IsNullOrWhiteSpace(artifacts?.Resource?.DownloadUrl))
            {
                return GetError("The artifact download link not found", log);
            }

            return new RedirectResult(artifacts.Resource.DownloadUrl);
        }
    

        private static IActionResult GetError(string error, ILogger log) 
        {
            log.LogError(error);
            var result = new 
            { 
                isOk = false, 
                error = error
            };
            return new JsonResult(result) 
            {
                StatusCode = StatusCodes.Status412PreconditionFailed 
            };
        }
    }
}
