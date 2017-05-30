using Contentful.Core.Models;
using Contentful.Essential.Models;
using System.Collections.Generic;

namespace Contentful.Essential.Sample.Models
{
    public class PatternMgmt : IManagementContentType
    {
        public Dictionary<string, string> Name { get; set; }

        public Dictionary<string, int> YarnWeight { get; set; }

        public Dictionary<string, string> Yardage { get; set; }

        public Dictionary<string, string> HookSize { get; set; }

        public Dictionary<string, string> PatternText { get; set; }

        public Dictionary<string, List<Asset>> FinishedProductImages { get; set; }
    }
}