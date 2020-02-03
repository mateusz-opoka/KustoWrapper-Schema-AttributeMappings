[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/KustoWrapper.Schema.AttributeMappings)](https://www.nuget.org/packages/KustoWrapper.Schema.AttributeMappings/)
[![build](https://github.com/mateusz-opoka/KustoWrapper-Schema-AttributeMappings/workflows/build/badge.svg?branch=master)](#)
[![codecov](https://codecov.io/gh/mateusz-opoka/KustoWrapper-Schema-AttributeMappings/branch/master/graph/badge.svg)](https://codecov.io/gh/mateusz-opoka/KustoWrapper-Schema-AttributeMappings)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/mateusz-opoka/KustoWrapper-Schema-AttributeMappings/blob/master/LICENSE)

# KustoWrapper.Schema.AttributeMappings

## Installation
```
dotnet add package KustoWrapper.Schema.AttributeMappings --version 1.0.0-preview1
```

## Example Usage
```csharp
[KustoTable("Items")]
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
        KustoTableInfo kustoTable = KustoColumnMappingsBuilder.Build<KustoSampleEntity>();
    }
}
```
