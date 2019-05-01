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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/{company}/{projectName}/{definitionId}/{artifactName}")] HttpRequest req,
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
            var builds = await latestBuildResponse.Content.ReadAsAsync<BuildResult>();
            var buildId = builds.Value[0].Id;
            var getArtifactUrl = $"https://dev.azure.com/{company}/{projectName}/_apis/build/builds/{buildId}/artifacts?artifactName={artifactName}&api-version=4.1";            
            var artifcatsResponse = await _httpClient.GetAsync(getArtifactUrl);
            var artifacts = await artifcatsResponse.Content.ReadAsAsync<ArtifactResult>();
            
            return new RedirectResult(artifacts.Resource.DownloadUrl);
        }
    }
}
