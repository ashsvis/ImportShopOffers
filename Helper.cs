using System;
using System.Text.RegularExpressions;

namespace ImportShopOffers
{
    public static class Helper
    {
        public static string CleanFromHtml(string value)
        {
            var step1 = Regex.Replace(value, @"</p>\s*|<br />\s*", @"[br]");
            var step2 = Regex.Replace(step1, @"\[br\]+", @"[br]");
            var step3 = Regex.Replace(step2, @"<[^>]+>", "").Trim();
            var step4 = Regex.Replace(step3, @"&nbsp;", " ").Trim();
            var step5 = Regex.Replace(step4, @"\s{2,}", " ");
            var step6 = Regex.Replace(step5, @"\[br\]", Environment.NewLine).Trim();
            return step6;
        }

        public static string CleanMoreSpaces(string value)
        {
            return Regex.Replace(value, @"\s{2,}", " ").Trim();
        }
    }
}
