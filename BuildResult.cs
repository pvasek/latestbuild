using System.Collections.Generic;

namespace DevOpsTools
{
    public class BuildResult 
    {
        public List<Build> Value { get; set; }
    }

    public class Build 
    {
        public string Id { get; set; }
    }
}