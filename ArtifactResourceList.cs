using System.Collections.Generic;

namespace DevOpsTools
{
    public class ArtifactResourceList 
    {
        public List<ArtifactResult> Value { get; set; }
    }

    public class ArtifactResult 
    {
        public string Name { get; set; }
        public ArtifactResource Resource { get; set; }
    }

    public class ArtifactResource
    {
        public string DownloadUrl { get; set; }
    }
}