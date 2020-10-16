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

            command.Should().Be(".create table SampleItems (timestamp:datetime)");
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

            command.Should().Be(".create-or-alter table SampleItems ingestion json mapping 'sampleMapping' '[" +
                                "{\"Properties\":{\"Path\":\"$.Timestamp\"},\"column\":\"timestamp\",\"datatype\":null}" +
                                "]' with (removeOldestIfRequired=False) ");
        }

        private static class Fixture
        {
            public static KustoTableInfo SampleKustoTable => new KustoTableInfo
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
