using System.Collections.Generic;

namespace KustoWrapper.Schema.AttributeMappings.Models
{
    public class KustoTableInfo
    {
        public string TableName { get; set; }
        public Dictionary<string, KustoColumnInfo> Columns { get; set; }
    }
}
