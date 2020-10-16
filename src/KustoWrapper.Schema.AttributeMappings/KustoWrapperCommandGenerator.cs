using Kusto.Data.Common;
using Kusto.Data.Ingestion;
using KustoWrapper.Schema.AttributeMappings.Models;
using System;
using System.Collections.Generic;
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

            var mapping = kustoTable.Columns.Select(BuildColumnMapping).ToList();

            return CslCommandGenerator.GenerateTableMappingCreateOrAlterCommand(
                IngestionMappingKind.Json, kustoTable.TableName, mappingName, mapping);
        }

        private static ColumnMapping BuildColumnMapping(KeyValuePair<string, KustoColumnInfo> column)
        {
            var (_, value) = column;

            var properties = new Dictionary<string, string>
            {
                {"Path", $"$.{value.SourcePropertyName}"}
            };

            return new ColumnMapping(value.Name, null, properties);
        }
    }
}
