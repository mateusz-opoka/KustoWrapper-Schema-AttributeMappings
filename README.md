[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/KustoWrapper.Schema.AttributeMappings)](https://www.nuget.org/packages/KustoWrapper.Schema.AttributeMappings/)
[![build](https://github.com/mateusz-opoka/KustoWrapper-Schema-AttributeMappings/workflows/build/badge.svg?branch=master)](#)
[![codecov](https://codecov.io/gh/mateusz-opoka/KustoWrapper-Schema-AttributeMappings/branch/master/graph/badge.svg)](https://codecov.io/gh/mateusz-opoka/KustoWrapper-Schema-AttributeMappings)
[![Maintainability](https://api.codeclimate.com/v1/badges/9c9b6a526c66c0968fe9/maintainability)](https://codeclimate.com/github/mateusz-opoka/KustoWrapper-Schema-AttributeMappings/maintainability)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://github.com/mateusz-opoka/KustoWrapper-Schema-AttributeMappings/blob/master/LICENSE)

# KustoWrapper.Schema.AttributeMappings

## Installation
```
dotnet add package KustoWrapper.Schema.AttributeMappings
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
        KustoTableInfo kustoTable = KustoTableSchemaBuilder.Build<KustoSampleEntity>();
        
        string tableCreateCommand = KustoWrapperCommandGenerator
            .GenerateTableCreateCommand(kustoTable);

        string tableJsonMappingCreateOrAlterCommand = KustoWrapperCommandGenerator
            .GenerateTableJsonMappingCreateOrAlterCommand(kustoTable, "ItemsMapping");
    }
}
```
