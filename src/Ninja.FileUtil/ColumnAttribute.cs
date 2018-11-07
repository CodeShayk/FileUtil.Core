using System;

namespace Ninja.FileUtil
{
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(int index, object defaultvalue = null)
        {
            Index = index;
            DefaultValue = defaultvalue;
        }

        public int Index { get; }
        public object DefaultValue { get; }
    }

}