namespace DevOpsTools
{
    public class ArtifactResult {
        public ArtifactResource Resource { get;set;}
    }

    public class ArtifactResource
    {
        public string DownloadUrl { get; set; }
    }
}