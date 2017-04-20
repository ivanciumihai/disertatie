using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace personal_pages.Helpers
{
    public static class StringHelper
    {
        private static readonly CultureInfo Ci = new CultureInfo("en-US");
        //Convert all first latter
        public static string ToTitleCase(this string str)
        {
            str = str.ToLower();
            var strArray = str.Split(' ');
            if (strArray.Length <= 1) return Ci.TextInfo.ToTitleCase(str);
            strArray[0] = Ci.TextInfo.ToTitleCase(strArray[0]);
            return string.Join(" ", strArray);
        }
        public static string ToTitleCase(this string str, TitleCase tcase)
        {
            str = str.ToLower();
            switch (tcase)
            {
                case TitleCase.First:
                    var strArray = str.Split(' ');
                    if (strArray.Length > 1)
                    {
                        strArray[0] = Ci.TextInfo.ToTitleCase(strArray[0]);
                        return string.Join(" ", strArray);
                    }
                    break;
                case TitleCase.All:
                    return Ci.TextInfo.ToTitleCase(str);
                default:
                    break;
            }
            return Ci.TextInfo.ToTitleCase(str);
        }
        public static string CutWhiteSpace(string s)
        {
            const RegexOptions options = RegexOptions.None;
            var regex = new Regex("[ ]{2,}", options);
            return regex.Replace(s, " ");
        }

        public static string SubStringCapital(string s)
        {
            var r = new Regex(@"[A-Z]", RegexOptions.IgnorePatternWhitespace);
            var result = r.Matches(s);
            return result.Cast<Match>().Aggregate(string.Empty, (current, match) => current + (match.Value + "."));
        }
    }
    public enum TitleCase
    {
        First,
        All
    }
}