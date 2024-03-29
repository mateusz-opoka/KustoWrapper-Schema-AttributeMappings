﻿using Kusto.Data.Common;
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

            var columns = kustoTable.Columns.Select(BuildColumnSchema);
            var tableSchema = new TableSchema(kustoTable.TableName, columns);

            return CslCommandGenerator.GenerateTableCreateCommand(tableSchema);
        }

        public static string GenerateTableCreateMergeCommand(KustoTableInfo kustoTable)
        {
            if (kustoTable == null) throw new ArgumentNullException(nameof(kustoTable));

            var columns = kustoTable.Columns.Select(BuildColumnSchema);
            var tableSchema = new TableSchema(kustoTable.TableName, columns);

            return CslCommandGenerator.GenerateTableCreateMergeCommand(tableSchema);
        }

        public static string GenerateTableJsonMappingCreateOrAlterCommand(KustoTableInfo kustoTable, string mappingName)
        {
            if (kustoTable == null) throw new ArgumentNullException(nameof(kustoTable));

            var mapping = kustoTable.Columns.Select(BuildColumnMapping);

            return CslCommandGenerator.GenerateTableMappingCreateOrAlterCommand(
                IngestionMappingKind.Json, kustoTable.TableName, mappingName, mapping);
        }

        private static ColumnSchema BuildColumnSchema(KeyValuePair<string, KustoColumnInfo> column)
        {
            var clrType = CslType.FromClrType(column.Value.Type);
            return ColumnSchema.FromNameAndCslType(column.Value.Name, clrType.ToString());
        }

        private static ColumnMapping BuildColumnMapping(KeyValuePair<string, KustoColumnInfo> column)
        {
            var properties = new Dictionary<string, string>
            {
                {"Path", $"$.{column.Value.SourcePropertyName}"}
            };

            return new ColumnMapping(column.Value.Name, null, properties);
        }
    }
}
