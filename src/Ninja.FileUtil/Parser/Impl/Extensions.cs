using System.Collections.Generic;
using System.Linq;

namespace Ninja.FileUtil.Parser.Impl
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
    }
}