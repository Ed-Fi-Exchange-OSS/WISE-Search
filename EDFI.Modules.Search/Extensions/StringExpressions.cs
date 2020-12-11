using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EDFI.Modules.Search.Extensions
{
    public static class StringExpressions
    {
        public static string ReplaceTokens(this string input, Dictionary<string, string> tokenReplacements)
        {
            if (input != null)
            {
                foreach (Match match in Regex.Matches(input, @"\$\{(?<token>.*?)\}"))
                {
                    //string tokenName = match.Value.Trim('{', '}');
                    string tokenName = match.Groups["token"].Value;
                    if (tokenReplacements.ContainsKey(tokenName))
                    {
                        var value = tokenReplacements[tokenName];
                        input = input.Replace(match.Value, value);
                    }
                }
            }

            return input;
        }

        public static DateTime? ParseDateOrNull(this string input)
        {
            DateTime result;
            if (DateTime.TryParse(input, out result)) return result;

            return null;
        }

        public static int? ParseIntOrNull(this string input)
        {
            int result;
            if (int.TryParse(input, out result)) return result;

            return null;
        }

        public static long? ParseLongOrNull(this string input)
        {
            long result;
            if (long.TryParse(input, out result)) return result;

            return null;
        }

        public static float? ParseFloatOrNull(this string input)
        {
            float result;
            if (float.TryParse(input, out result)) return result;

            return null;
        }

        public static bool ParseOrDefault(this string input, bool defaultValue)
        {
            bool result;
            if (bool.TryParse(input, out result)) return result;

            return defaultValue;
        }
    }
}