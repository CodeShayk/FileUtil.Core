using System.Collections.Generic;
using System.Linq;
using FileUtil.Configuration;

namespace FileUtil.Parser
{
    internal static class Extensions
    {
        public static bool In(this string input, params string[] values)
        {
            return values != null && values.Contains(input);
        }

        public static void SetError(this IFileLine obj, string error)
        {
            if (obj.Errors == null)
                obj.Errors = new List<string>();

            obj.Errors.Add(error);
        }

        public static char GetValue(this IDelimiter delimiter)
        {
            return delimiter?.Value ?? ',';
        }

        public static string GetLineHead(this ILineHeaders lineHeaders, LineType type)
        {
            switch (type)
            {
                case LineType.Header:
                    return lineHeaders?.Header ?? "H";
                case LineType.Footer:
                    return lineHeaders?.Footer ?? "F";
                default:
                    return lineHeaders?.Data ?? "D";
            }
        }
    }
}