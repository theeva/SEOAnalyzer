using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEOAnalyzer.Models
{
    public class ContentModel
    {
        [Display(Name = "Input")]
        [Required(AllowEmptyStrings = false)]
        public string Input { get; set; }
        public string Content { get; set; }

        public bool IsValidModel { get; set; }

        public bool FilterStopWord { get; set; } = true;
        public bool CalculateNumberWordOccurence { get; set; } = true;
        public bool CalculateNumberWordOccurenceInMetaTag { get; set; } = true;
        public bool CalculateNumberExternalLink { get; set; } = true;
        public bool ValidationChecked { get; set; }
        public bool HasError { get; set; }

        public Dictionary<string, int> NumberWordOccurences { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> NumberWordOccurencesFromMetaTags { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> NumberExternalLinks { get; set; } = new Dictionary<string, int>();
    }
}