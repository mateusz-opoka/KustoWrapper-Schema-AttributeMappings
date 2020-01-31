using Azure.Kusto.Schema.AttributeMappings.Attributes;
using Azure.Kusto.Schema.AttributeMappings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Azure.Kusto.Schema.AttributeMappings
{
    public class KustoColumnMappingsBuilder
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

        public static Dictionary<string, KustoColumnInfo> Build<T>() => Build(typeof(T));

        public static Dictionary<string, KustoColumnInfo> Build(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            var descriptions = new Dictionary<string, KustoColumnInfo>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var attribute = GetKustoColumnAttribute(propertyInfo);
                if (attribute == null) continue;

                var columnName = attribute.ColumnName ?? propertyInfo.Name;

                if (descriptions.ContainsKey(columnName))
                    throw new InvalidOperationException($"Column name `{columnName}` already exist.");

                descriptions.Add(columnName, BuildPropertyDefinition(columnName, propertyInfo));
            }

            return descriptions;
        }

        private static KustoColumnInfo BuildPropertyDefinition(string columnName, PropertyInfo propertyInfo)
        {
            var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            if (!SupportedDataTypes.Contains(propertyType)) propertyType = typeof(object);
            return new KustoColumnInfo(columnName, propertyType, propertyInfo.Name);
        }

        private static KustoColumnAttribute GetKustoColumnAttribute(ICustomAttributeProvider propertyInfo) =>
            propertyInfo.GetCustomAttributes(false).OfType<KustoColumnAttribute>().SingleOrDefault();
    }
}
