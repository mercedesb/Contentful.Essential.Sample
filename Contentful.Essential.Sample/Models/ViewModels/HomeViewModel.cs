using System.Collections.Generic;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            SearchModel = new PatternSearchModel();
        }
        public IEnumerable<Pattern> Patterns { get; set; }

        public PatternSearchModel SearchModel { get; set; }
    }
}