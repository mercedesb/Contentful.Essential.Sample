using System.ComponentModel.DataAnnotations;

namespace Contentful.Essential.Sample.Models.ViewModels
{
    public class PatternSearchModel
    {
        [Display(Name = "Yarn Weight")]
        public int YarnWeight { get; set; }
        [Display(Name = "Maximum Yardage")]
        public int MaxYardage { get; set; }
        [Display(Name = "Search")]
        public string SearchText { get; set; }
    }
}