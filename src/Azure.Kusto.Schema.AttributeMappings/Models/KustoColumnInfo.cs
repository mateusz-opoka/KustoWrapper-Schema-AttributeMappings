using System;

namespace Azure.Kusto.Schema.AttributeMappings.Models
{
    public class KustoColumnInfo
    {
        public string Name { get; }
        public Type Type { get; }
        public string SourcePropertyName { get; }

        public KustoColumnInfo(string name, Type type, string sourcePropertyName)
        {
            Name = name;
            Type = type;
            SourcePropertyName = sourcePropertyName;
        }
    }
}
