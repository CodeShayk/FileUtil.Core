using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FileUtil.Configuration;
using Ninja.FileUtil;

namespace FileUtil.Parser.Impl
{
    internal class LineParser : ILineParser
    {
        private readonly IParserSettings parserSettings;

        public LineParser(IParserSettings parserSettings)
        {
            this.parserSettings = parserSettings;
        }

        public T[] Parse<T>(string[] lines) where T : IFileLine, new()
        {
            return Parse<T>(lines, LineType.Data, false);
        }

        public T[] Parse<T>(string[] lines, LineType type) where T : IFileLine, new()
        {
            var filteredLines = lines?.Where(x => x.StartsWith(parserSettings.LineHeaders.GetLineHead(type))).ToArray();
            return Parse<T>(filteredLines, type, true);
        }

        private T[] Parse<T>(string[] lines, LineType type, bool hasLineHeader) where T : IFileLine, new()
        {

            if (lines == null || lines.Length == 0)
                return Enumerable.Empty<T>().ToArray();

            var list = new T[lines.Length];

            var objLock = new object();

            var index = 0;
            var inputs = lines.Select(line => new { Line = line, Index = index++, Type = type });

            Parallel.ForEach(inputs, () => new List<T>(),
                (obj, loopstate, localStorage) =>
                {
                    var parsed = ParseLine<T>(obj.Line, hasLineHeader);

                    parsed.Index = obj.Index;
                    parsed.Type = obj.Type;

                    localStorage.Add(parsed);
                    return localStorage;
                },
                finalStorage =>
                {
                    if (finalStorage == null)
                        return;

                    lock (objLock)
                        finalStorage.ForEach(f => list[f.Index] = f);
                });


            return list;
        }


        private T ParseLine<T>(string line, bool hasLineHeader) where T : IFileLine, new()
        {
            var obj = new T();

            var values = GetDelimiterSeparatedValues(line);

            if (values.Length == 0 || values.Length == 1)
            {
                obj.SetError(Resources.InvalidLineFormat);
                return obj;
            }

            var propInfos = GetLineClassPropertyInfos<T>();

            if (propInfos.Length == 0)
            {
                obj.SetError(string.Format(Resources.NoColumnAttributesFoundFormat, typeof(T).Name));
                return obj;
            }

            if (!hasLineHeader && propInfos.Length != values.Length ||
                hasLineHeader && propInfos.Length + 1 != values.Length)
            {
                obj.SetError(Resources.InvalidLengthErrorFormat);
                return obj;
            }

            foreach (var propInfo in propInfos)
                try
                {
                    var attribute = (ColumnAttribute)propInfo.GetCustomAttributes(typeof(ColumnAttribute), true).First();

                    var pvalue = values[!hasLineHeader ? attribute.Index : attribute.Index + 1];

                    if (string.IsNullOrWhiteSpace(pvalue) && attribute.DefaultValue != null)
                        pvalue = attribute.DefaultValue.ToString();

                    if (propInfo.PropertyType.IsEnum)
                    {
                        if (string.IsNullOrWhiteSpace(pvalue))
                        {
                            obj.SetError(string.Format(Resources.InvalidEnumValueErrorFormat, propInfo.Name));
                            continue;
                        }

                        if (long.TryParse(pvalue, out var enumLong))
                        {
                            var numeric = Enum.ToObject(propInfo.PropertyType, enumLong);
                            propInfo.SetValue(obj, numeric, null);
                            continue;
                        }

                        var val = Enum.Parse(propInfo.PropertyType, pvalue, true);
                        propInfo.SetValue(obj, val, null);
                        continue;
                    }

                    var converter = TypeDescriptor.GetConverter(propInfo.PropertyType);
                    var value = converter.ConvertFrom(pvalue);

                    propInfo.SetValue(obj, value, null);
                }
                catch (Exception e)
                {
                    obj.SetError(string.Format(Resources.LineExceptionFormat, propInfo.Name, e.Message));
                }

            return obj;
        }

        private static PropertyInfo[] GetLineClassPropertyInfos<T>() where T : IFileLine, new()
        {
            var propInfos = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(ColumnAttribute), true).Any() && p.CanWrite)
                .ToArray();
            return propInfos;
        }

        private string[] GetDelimiterSeparatedValues(string line)
        {
            var values = line.Split(parserSettings.Delimiter.GetValue())
                .Select(x => !string.IsNullOrWhiteSpace(x) ? x.Trim() : x)
                .ToArray();
            return values;
        }
    }
}
