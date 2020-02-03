using FluentAssertions;
using FluentAssertions.Execution;
using KustoWrapper.Schema.AttributeMappings.Attributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace KustoWrapper.Schema.AttributeMappings.Tests
{
    [TestFixture]
    public class KustoColumnMappingsBuilderTests
    {
        [Test]
        public void Build_OnNull_Should_ThrowException()
        {
            Action act = () => KustoColumnMappingsBuilder.Build(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [TestCase(typeof(Fixture.StructBasedPropertiesClass<bool>), typeof(bool))]
        [TestCase(typeof(Fixture.StructBasedPropertiesClass<DateTime>), typeof(DateTime))]
        [TestCase(typeof(Fixture.StructBasedPropertiesClass<Guid>), typeof(Guid))]
        [TestCase(typeof(Fixture.StructBasedPropertiesClass<int>), typeof(int))]
        [TestCase(typeof(Fixture.StructBasedPropertiesClass<long>), typeof(long))]
        [TestCase(typeof(Fixture.StructBasedPropertiesClass<double>), typeof(double))]
        [TestCase(typeof(Fixture.StructBasedPropertiesClass<TimeSpan>), typeof(TimeSpan))]
        [TestCase(typeof(Fixture.StructBasedPropertiesClass<decimal>), typeof(decimal))]
        public void Build_OnStructBasedProperties_Should_BuildDescription(Type classWithProperies, Type expectedPropertyType)
        {
            var act = KustoColumnMappingsBuilder.Build(classWithProperies);

            using (new AssertionScope())
            {
                act.Columns["property"].Name.Should().Be("property");
                act.Columns["property"].Type.Should().Be(expectedPropertyType);
                act.Columns["property"].SourcePropertyName.Should().Be(nameof(Fixture.StructBasedPropertiesClass<bool>.Property));
                act.Columns["nullable_property"].Name.Should().Be("nullable_property");
                act.Columns["nullable_property"].Type.Should().Be(expectedPropertyType);
                act.Columns["nullable_property"].SourcePropertyName.Should().Be(nameof(Fixture.StructBasedPropertiesClass<bool>.NullableProperty));
            }
        }

        [TestCase(typeof(Fixture.ClassBasedPropertiesClass<object>), typeof(object))]
        [TestCase(typeof(Fixture.ClassBasedPropertiesClass<string>), typeof(string))]
        [TestCase(typeof(Fixture.ClassBasedPropertiesClass<int[]>), typeof(object))]
        [TestCase(typeof(Fixture.ClassBasedPropertiesClass<List<int>>), typeof(object))]
        [TestCase(typeof(Fixture.ClassBasedPropertiesClass<Dictionary<int, int>>), typeof(object))]
        [TestCase(typeof(Fixture.ClassBasedPropertiesClass<HashSet<int>>), typeof(object))]
        [TestCase(typeof(Fixture.ClassBasedPropertiesClass<Fixture.CustomType>), typeof(object))]
        public void Build_OnClassBasedProperties_Should_BuildDescription(Type classWithProperies, Type expectedPropertyType)
        {
            var act = KustoColumnMappingsBuilder.Build(classWithProperies);

            using (new AssertionScope())
            {
                act.Columns["property"].Name.Should().Be("property");
                act.Columns["property"].SourcePropertyName.Should().Be(nameof(Fixture.ClassBasedPropertiesClass<object>.Property));
                act.Columns["property"].Type.Should().Be(expectedPropertyType);
            }
        }

        [Test]
        public void Build_OnUnnamedKustoColumn_Should_ReturnPropertyName()
        {
            var act = KustoColumnMappingsBuilder.Build<Fixture.UnnamedKustoColumn>();
            act.Columns.Should().ContainKey(nameof(Fixture.UnnamedKustoColumn.Property));
        }

        [Test]
        public void Build_OnDuplicatedColumName_Should_ThrowException()
        {
            var _ = $"{nameof(Fixture.DuplicatedNameKustoColum.Property1)}" +
                    $"_{nameof(Fixture.DuplicatedNameKustoColum.Property2)}";

            Action act = () => KustoColumnMappingsBuilder.Build<Fixture.DuplicatedNameKustoColum>();

            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Column name `*` already exist.");
        }

        [Test]
        public void Build_OnUnnamedKustoTable_Should_ReturnClassName()
        {
            var act = KustoColumnMappingsBuilder.Build<Fixture.UnnamedKustoTable>();
            act.TableName.Should().Be(nameof(Fixture.UnnamedKustoTable));
        }

        [Test]
        public void Build_NamedKustoTable_Should_ReturnNameFromAttribute()
        {
            var act = KustoColumnMappingsBuilder.Build<Fixture.NamedKustoTable>();
            act.TableName.Should().Be("named_kusto_table");
        }

        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private static class Fixture
        {
            public abstract class StructBasedPropertiesClass<T> where T : struct
            {
                [KustoColumn("property")] public T Property { get; } = default;
                [KustoColumn("nullable_property")] public T? NullableProperty { get; } = default;
            }

            public abstract class ClassBasedPropertiesClass<T> where T : class
            {
                [KustoColumn("property")] public T Property { get; } = default;
            }

            public abstract class UnnamedKustoColumn
            {
                [KustoColumn] public int Property { get; } = default;
            }

            public abstract class DuplicatedNameKustoColum
            {
                [KustoColumn("column")] public object Property1 { get; } = default;
                [KustoColumn("column")] public object Property2 { get; } = default;
            }

            public abstract class CustomType { }

            public abstract class UnnamedKustoTable { }

            [KustoTable("named_kusto_table")]
            public abstract class NamedKustoTable { }
        }
    }
}
