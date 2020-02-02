using System;

namespace KustoWrapper.Schema.AttributeMappings.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class KustoColumnAttribute : Attribute
    {
        public string ColumnName { get; }

        public KustoColumnAttribute() { }

        public KustoColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
