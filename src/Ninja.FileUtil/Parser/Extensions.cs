using System.Collections.Generic;
using System.Linq;
using Ninja.FileUtil.Configuration;

namespace Ninja.FileUtil.Parser
{
    internal static class Extensions
    {
        public static bool In(this string input, params string[] values)
        {
            return values != null && values.Contains(input);
        }

        public static void SetError(this IFileLine obj, string error)
        {
            if (obj.Errors == null)  obj.Errors = new List<string>();

            obj.Errors.Add(error);
        }

        public static char GetValue(this IDelimiter delimiter)
        {
            return delimiter?.Value ?? ',';
        }

        public static string GetHeaderValue(this ILineHeaders lineHeaders)
        {
            return lineHeaders?.Header ?? "H";
        }
        public static string GetFooterValue(this ILineHeaders lineHeaders)
        {
            return lineHeaders?.Footer ?? "F";
        }
        public static string GetDataValue(this ILineHeaders lineHeaders)
        {
            return lineHeaders?.Data ?? "D";
        }
    }
}