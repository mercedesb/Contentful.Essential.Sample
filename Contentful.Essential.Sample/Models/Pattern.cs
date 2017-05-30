using Contentful.CodeFirst;
using Contentful.Core.Models;
using Contentful.Core.Models.Management;
using Contentful.Core.Search;
using Contentful.Essential.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contentful.Essential.Sample.Models
{
    [ContentType]
    public class Pattern : BaseEntry
    {
        [ContentField(Type = SystemFieldTypes.Symbol, Required = true)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Yarn Weight")]
        public int YarnWeight { get; set; }

        [ContentField(Type = SystemFieldTypes.Symbol)]
        public string Yardage { get; set; }

        [ContentField(Type = SystemFieldTypes.Symbol)]
        [Display(Name = "Hook Size")]
        public string HookSize { get; set; }

        [ContentField(Localized = true)]
        [Display(Name = "Pattern Text")]
        public string PatternText { get; set; }

        [MimeType(MimeTypes = new[] { MimeTypeRestriction.Image })]
        public List<Asset> FinishedProductImages { get; set; }
    }
}