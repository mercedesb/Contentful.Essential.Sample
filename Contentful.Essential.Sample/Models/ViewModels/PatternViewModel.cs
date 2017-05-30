using Contentful.Core.Images;
using System.Collections.Generic;
using System.Linq;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class PatternViewModel
    {
        public Pattern CurrentPattern { get; set; }

        public IEnumerable<string> ManipulatedImageUrls
        {
            get
            {
                if (CurrentPattern != null && CurrentPattern.FinishedProductImages != null)
                    return CurrentPattern.FinishedProductImages.Select(img => img.File != null ? $"{img.File.Url}{ImageUrlBuilder.New().SetWidth(250).Build()}" : string.Empty);

                return Enumerable.Empty<string>();
            }
        }

        public string PatternText
        {
            get
            {
                if (CurrentPattern != null && !string.IsNullOrWhiteSpace(CurrentPattern.PatternText))
                    return CurrentPattern.PatternText.Replace("\n", "<br/>");

                return string.Empty;
            }
        }
    }
}