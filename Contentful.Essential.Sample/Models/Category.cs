using Contentful.CodeFirst;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Essential.Models;
using System.Collections.Generic;

namespace Contentful.Essential.Sample.Models
{
    [ContentType(Id = "datasetCategory", Name = "Category")]
    public class Category : BaseEntry
    {
        [ContentField(Type = SystemFieldTypes.Symbol)]
        public string Name { get; set; }

        [LinkContentType("sodaDataset")]
        public List<DataSet> Datasets { get; set; }

        public string PageContent { get; set; }
    }
}