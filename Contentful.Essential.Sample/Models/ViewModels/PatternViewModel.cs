using System.Collections.Generic;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class PatternViewModel
    {
        public Pattern CurrentPattern { get; set; }

        public IEnumerable<string> ManipulatedImageUrls { get; set; }
    }
}