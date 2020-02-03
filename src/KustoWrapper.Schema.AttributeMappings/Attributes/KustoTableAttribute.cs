using System;

namespace KustoWrapper.Schema.AttributeMappings.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KustoTableAttribute : Attribute
    {
        public string TableName { get; }

        public KustoTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
