using Contentful.CodeFirst;
using Contentful.Core.Models.Management;
using Contentful.Essential.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Contentful.Essential.Sample.Models
{
    [ContentType(Id = "sodaDataset", Name = "SODA Dataset")]
    public class DataSet : BaseEntry
    {
        [ContentField(Type = SystemFieldTypes.Symbol)]
        public string Name { get; set; }

        [Display(Name = "API Endpoint")]
        [ContentField(Type = SystemFieldTypes.Symbol)]
        public string APIEndpoint { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        public string PageContent { get; set; }

    }
}