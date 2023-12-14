using FluentAssertions;
using KustoWrapper.Schema.AttributeMappings.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace KustoWrapper.Schema.AttributeMappings.Tests
{
    [TestFixture]
    public class KustoWrapperCommandGeneratorTests
    {
        [Test]
        public void GenerateTableCreateCommand_OnNull_ShouldThrowException()
        {
            Action act = () => KustoWrapperCommandGenerator.GenerateTableCreateCommand(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GenerateTableCreateCommand_OnSampleKustoTable_ShouldStringCommand()
        {
            var command = KustoWrapperCommandGenerator
                .GenerateTableCreateCommand(Fixture.SampleKustoTable);

            command.Should().Be(".create table SampleItems (timestamp:datetime) ");
        }

        [Test]
        public void GenerateTableCreateCommand_OnSampleKustoTable_OnCustomTableName_ShouldStringCommand()
        {
            var tableInfo = Fixture.SampleKustoTable;
            tableInfo.TableName = "TestCustomTableName";

            var command = KustoWrapperCommandGenerator
                .GenerateTableCreateCommand(tableInfo);

            command.Should().Be(".create table TestCustomTableName (timestamp:datetime) ");
        }

        [Test]
        public void GenerateTableCreateCommand_OnSampleKustoTable_OnCustomTableNameWithSpecialChar_ShouldStringCommand()
        {
            var tableInfo = Fixture.SampleKustoTable;
            tableInfo.TableName = "Test-Custom-Table-Name";

            var command = KustoWrapperCommandGenerator
                .GenerateTableCreateCommand(tableInfo);

            command.Should().Be(".create table ['Test-Custom-Table-Name'] (timestamp:datetime) ");
        }

        [Test]
        public void GenerateTableCreateMergeCommand_OnNull_ShouldThrowException()
        {
            Action act = () => KustoWrapperCommandGenerator.GenerateTableCreateMergeCommand(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void GenerateTableCreateMergeCommand_OnSampleKustoTable_ShouldStringCommand()
        {
            var command = KustoWrapperCommandGenerator
                .GenerateTableCreateMergeCommand(Fixture.SampleKustoTable);

            command.Should().Be(".create-merge table SampleItems (timestamp:datetime)");
        }

        [Test]
        public void GenerateTableCreateMergeCommand_OnSampleKustoTable_OnCustomTableName_ShouldStringCommand()
        {
            var tableInfo = Fixture.SampleKustoTable;
            tableInfo.TableName = "TestCustomTableName";

            var command = KustoWrapperCommandGenerator
                .GenerateTableCreateMergeCommand(tableInfo);

            command.Should().Be(".create-merge table TestCustomTableName (timestamp:datetime)");
        }

        [Test]
        public void GenerateTableCreateMergeCommand_OnSampleKustoTable_OnCustomTableNameWithSpecialChar_ShouldStringCommand()
        {
            var tableInfo = Fixture.SampleKustoTable;
            tableInfo.TableName = "Test-Custom-Table-Name";

            var command = KustoWrapperCommandGenerator
                .GenerateTableCreateMergeCommand(tableInfo);

            command.Should().Be(".create-merge table ['Test-Custom-Table-Name'] (timestamp:datetime)");
        }

        [Test]
        public void GenerateTableJsonMappingCreateOrAlterCommand_OnNull_ShouldThrowException()
        {
            Action act = () => KustoWrapperCommandGenerator
                .GenerateTableJsonMappingCreateOrAlterCommand(null, "sampleMapping");

            act.Should().Throw<ArgumentNullException>();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GenerateTableJsonMappingCreateOrAlterCommand_OnEmptyMappingName_ShouldThrowException(string mappingName)
        {
            Action act = () => KustoWrapperCommandGenerator
                .GenerateTableJsonMappingCreateOrAlterCommand(Fixture.SampleKustoTable, mappingName);

            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GenerateTableJsonMappingCreateOrAlterCommand_OnSampleKustoTable_ShouldReturnCommand()
        {
            var command = KustoWrapperCommandGenerator
                .GenerateTableJsonMappingCreateOrAlterCommand(Fixture.SampleKustoTable, "sampleMapping");

            const string expected = """
                                    .create-or-alter table SampleItems ingestion json mapping 'sampleMapping'
                                    ```
                                    [{"Properties":{"Path":"$.Timestamp"},"column":"timestamp","datatype":null}]
                                    ```
                                    with (removeOldestIfRequired=False)
                                    """;

            command.Should().Be(expected);
        }

        private static class Fixture
        {
            public static KustoTableInfo SampleKustoTable => new()
            {
                TableName = "SampleItems",
                Columns = new Dictionary<string, KustoColumnInfo>
                {
                    {"timestamp", new KustoColumnInfo("timestamp", typeof(DateTime), "Timestamp")}
                }
            };
        }
    }
}
