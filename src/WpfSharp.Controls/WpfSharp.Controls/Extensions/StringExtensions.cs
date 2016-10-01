using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Documents;

namespace WpfSharp.Controls.Extensions
{
    internal static class StringExtensions
    {
        internal static List<string> Split(this string value, string regex, RegexOptions options)
        {
            var parts = new List<string>();
            int pos = 0;
            foreach (Match match in Regex.Matches(value, regex, options))
            {
                parts.Add(value.Substring(pos, match.Index - pos));
                parts.Add(match.Value);
                pos = match.Index + match.Length;
            }
            parts.Add(value.Substring(pos));
            return parts;
        }

        internal static bool IsValidRegex(this string inRegex)
        {
            if (string.IsNullOrEmpty(inRegex))
                return false;
            try { Regex.Match("", inRegex); }
            catch { return false; }
            return true;
        }

        internal static string ToRegex(this List<string> words)
        {
            string regex = string.Empty;
            foreach (string word in words)
            {
                if (regex.Length > 0)
                    regex += "|";
                regex += word.RegexWrap();
            }
            return regex;
        }

        internal static string RegexWrap(this string inString)
        {
            return string.Format("({0})", inString);
        }

        internal static List<Inline> GetRunLines(this string value, object model, string regex, RegexOptions options)
        {
            var lines = new List<Inline>();
            List<string> split = value.Split(regex, options);
            foreach (var str in split)
            {
                Run run = new Run(str);
                if (Regex.IsMatch(str, regex, options))
                {
                    run.SetBinding(model, Run.BackgroundProperty, "HighlightBackground");
                    run.SetBinding(model, Run.ForegroundProperty, "HighlightForeground");
                    run.SetBinding(model, Run.FontWeightProperty, "HighlightFontWeight");
                }
                lines.Add(run);
            }
            return lines;
        }
    }
}
