using Kusto.Data.Common;
using KustoWrapper.Schema.AttributeMappings.Models;
using System;
using System.Linq;

namespace KustoWrapper.Schema.AttributeMappings
{
    public static class KustoWrapperCommandGenerator
    {
        public static string GenerateTableCreateCommand(KustoTableInfo kustoTable)
        {
            if (kustoTable == null) throw new ArgumentNullException(nameof(kustoTable));

            var columns = kustoTable.Columns
                .Select(column => new Tuple<string, Type>(column.Value.Name, column.Value.Type));

            return CslCommandGenerator.GenerateTableCreateCommand(kustoTable.TableName, columns);
        }

        public static string GenerateTableJsonMappingCreateOrAlterCommand(KustoTableInfo kustoTable, string mappingName)
        {
            if (kustoTable == null) throw new ArgumentNullException(nameof(kustoTable));

            var mapping = kustoTable.Columns
                .Select(column => new JsonColumnMapping
                {
                    ColumnName = column.Value.Name,
                    JsonPath = $"$.{column.Value.SourcePropertyName}"
                });

            return CslCommandGenerator.GenerateTableJsonMappingCreateOrAlterCommand(
                kustoTable.TableName, mappingName, mapping);
        }
    }
}
