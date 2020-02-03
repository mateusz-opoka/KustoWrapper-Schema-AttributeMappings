using KustoWrapper.Schema.AttributeMappings.Attributes;
using KustoWrapper.Schema.AttributeMappings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KustoWrapper.Schema.AttributeMappings
{
    public class KustoTableSchemaBuilder
    {
        /// <summary>
        /// Data types supported in Kusto
        /// https://docs.microsoft.com/en-us/azure/kusto/query/scalar-data-types/
        /// </summary>
        private static readonly Type[] SupportedDataTypes =
        {
            typeof(bool),
            typeof(DateTime),
            typeof(object),
            typeof(Guid),
            typeof(int),
            typeof(long),
            typeof(double),
            typeof(string),
            typeof(TimeSpan),
            typeof(decimal)
        };

        public static KustoTableInfo Build<T>() => Build(typeof(T));

        public static KustoTableInfo Build(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var tableAttribute = GetKustoTableAttribute(type);
            var tableName = tableAttribute?.TableName ?? type.Name;

            var columnsDictionary = new Dictionary<string, KustoColumnInfo>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var columnAttribute = GetKustoColumnAttribute(propertyInfo);
                if (columnAttribute == null) continue;

                var columnName = columnAttribute.ColumnName ?? propertyInfo.Name;

                if (columnsDictionary.ContainsKey(columnName))
                    throw new InvalidOperationException($"Column name `{columnName}` already exist.");

                columnsDictionary.Add(columnName, BuildPropertyDefinition(columnName, propertyInfo));
            }

            return new KustoTableInfo
            {
                TableName = tableName,
                Columns = columnsDictionary
            };
        }

        private static KustoColumnInfo BuildPropertyDefinition(string columnName, PropertyInfo propertyInfo)
        {
            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            if (!SupportedDataTypes.Contains(propertyType)) propertyType = typeof(object);
            return new KustoColumnInfo(columnName, propertyType, propertyInfo.Name);
        }

        private static KustoColumnAttribute GetKustoColumnAttribute(ICustomAttributeProvider propertyInfo) =>
            propertyInfo.GetCustomAttributes(false).OfType<KustoColumnAttribute>().SingleOrDefault();

        private static KustoTableAttribute GetKustoTableAttribute(ICustomAttributeProvider type) =>
            type.GetCustomAttributes(false).OfType<KustoTableAttribute>().SingleOrDefault();
    }
}
