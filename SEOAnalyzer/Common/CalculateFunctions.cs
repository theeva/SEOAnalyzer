using SEOAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SEOAnalyzer.Common
{
    public class CalculateFunctions : GenericFunctions
    {       
        public void CountNumberOfExternalLinks(ContentModel model)
        {
            if (!model.CalculateNumberExternalLink)
            {
                return;
            }

            string[] words = model.Content.Split(' ');

            Regex linksregex = new Regex(@"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)");

            string urls = string.Empty;

            foreach (Match m in linksregex.Matches(model.Content))
            {
                urls += " " + m.Value;
            }

            model.NumberExternalLinks = CountNumberOfWord(urls);
        }

        public void CalculateWordOccurencesInMetaTag(ContentModel model)
        {
            if (!model.CalculateNumberWordOccurenceInMetaTag)
            {
                return;
            }

            Regex metaTag = new Regex(@"<meta.*?>");
            Dictionary<string, string> metaInformation = new Dictionary<string, string>();

            string metaData = string.Empty;
            foreach (Match m in metaTag.Matches(model.Content))
            {
                metaData += " " + m.Value;
            }

            model.NumberWordOccurencesFromMetaTags = CountNumberOfWord(metaData);
        }

        public void CalculateWordOccurences(ContentModel model)
        {
            if (!model.CalculateNumberWordOccurence)
            {
                return;
            }

            string plainTextContent = ConvertHtmlToPlainText(model.Content);
            model.NumberWordOccurences = CountNumberOfWord(plainTextContent);
        }

        public void FilterStopWords(ContentModel model, string[] StopWords)
        {

            if (!model.FilterStopWord)
            {
                return;
            }

            string[] words = model.Content.Split(' ');

            model.Content = string.Join(" ",
                            words.Where(wrds => !string.IsNullOrEmpty(wrds) && !StopWords.Contains(wrds)));
        }
    }
}