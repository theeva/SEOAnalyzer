using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SEOAnalyzer.Common
{
    public class GenericFunctions
    {
        public string ConvertHtmlToPlainText(string htmldoc)
        {

            Regex tagRemove = new Regex(@"<[^>]*(>|$)", RegexOptions.Multiline);
            Regex compressSpaces = new Regex(@"[\s\r\n]+", RegexOptions.Multiline);
            Regex lineBreak = new Regex(@"<(br|BR)\s{0,1}\/{0,1}>", RegexOptions.Multiline);
            Regex WhiteSpace = new Regex(@"(>|$)(\W|\n|\r)+<", RegexOptions.Multiline);

            var plaintext = htmldoc;
            plaintext = WhiteSpace.Replace(plaintext, "><");
            plaintext = lineBreak.Replace(plaintext, Environment.NewLine);
            plaintext = tagRemove.Replace(plaintext, string.Empty);
            plaintext = tagRemove.Replace(plaintext, string.Empty);
            plaintext = compressSpaces.Replace(plaintext, " ");
            plaintext = System.Net.WebUtility.HtmlDecode(plaintext);

            return plaintext;
        }

        public Dictionary<string, int> CountNumberOfWord(string contents)
        {
            string[] words = contents.Split(' ');
            return words.Where(wrds => !string.IsNullOrEmpty(wrds)).GroupBy(wrds => wrds)
                             .ToDictionary(group => group.Key, group => group.Count());
        }
    }
}