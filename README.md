[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Custom.Azure.Kusto.Schema.AttributeMappings)](https://www.nuget.org/packages/Custom.Azure.Kusto.Schema.AttributeMappings/)
[![build](https://github.com/mateusz-opoka/Azure-Kusto-Schema-AttributeMappings/workflows/build/badge.svg?branch=master)](#)
[![codecov](https://codecov.io/gh/mateusz-opoka/Azure-Kusto-Schema-AttributeMappings/branch/master/graph/badge.svg)](https://codecov.io/gh/mateusz-opoka/Azure-Kusto-Schema-AttributeMappings)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/mateusz-opoka/Azure-Kusto-Schema-AttributeMappings/blob/master/LICENSE)

# Azure-Kusto-Schema-AttributeMappings

## Installation
```
dotnet add package Custom.Azure.Kusto.Schema.AttributeMappings --version 1.0.0-preview1
```

## Example Usage
```csharp
public class KustoSampleEntity
{
    [KustoColumn("date")]
    public DateTime Date { get; set; }

    [KustoColumn("item_Id")]
    public Guid ItemId { get; set; }

    [KustoColumn("item_Description")]
    public string Description { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, KustoColumnInfo> columns = KustoColumnMappingsBuilder.Build<KustoSampleEntity>();
    }
}
```
