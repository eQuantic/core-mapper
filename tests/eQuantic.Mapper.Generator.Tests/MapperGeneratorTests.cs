﻿using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace eQuantic.Mapper.Generator.Tests;

public class MapperGeneratorTests
{
    [Test]
    public void MapperGenerator_Execute_Successfully()
    {
        var inputCompilation = CreateCompilation(@"
using System;
using System.Collections.Generic;
using eQuantic.Mapper;
using eQuantic.Mapper.Attributes;
using MyCode.Models;

namespace MyCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }

    public enum ExampleEnumA
    {
        Item1,
        Item2
    }

    public enum ExampleEnumB
    {
        Item1,
        Item2
    }

    public class ExampleA
    {
        public string? Name { get; set; }

        public string? NullableStringToDate { get; set; }
        public string? StringToDate { get; set; }

        public string? NullableStringToInteger { get; set; }
        public string? StringToInteger { get; set; }

        public int? NullableToNonNullableInteger { get; set; }

        public ICollection<ExampleModelA> Models { get; set; }
        public ExampleModelA Model { get; set; }

        public ExampleEnumA Enum1 { get; set; }
        public int Enum2 { get; set; }
        public ExampleEnumA Enum3 { get; set; }

        public ICollection<int> IntegerList { get; set; }
        public ICollection<Guid> GuidList { get; set; }
    }

    public class ExampleB
    {
        public string? Name { get; set; }
        public DateTime? NullableStringToDate { get; set; }
        public DateTime StringToDate { get; set; }

        public int? NullableStringToInteger { get; set; }
        public int StringToInteger { get; set; }

        public int NullableToNonNullableInteger { get; set; }

        public IEnumerable<ExampleModelB> Models { get; set; }
        public ExampleModelB Model { get; set; }

        public ExampleEnumB Enum1 { get; set; }
        public ExampleEnumB Enum2 { get; set; }
        public int Enum3 { get; set; }

        public ICollection<int> IntegerList { get; set; }
        public ICollection<Guid> GuidList { get; set; }
    }

    [Mapper(typeof(MyCode.ExampleA), typeof(MyCode.ExampleB))]
    public partial class ExampleMapper : IMapper
    {
    }
}

namespace MyCode.Models
{
    public class ExampleModelA
    {
        public string? Name { get; set; }
    }

    public class ExampleModelB
    {
        public string? Name { get; set; }
    }
}
");

        var generator = new MapperGenerator();
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
        driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation,
            out var diagnostics);

        const string expectedCode = @"//This file was generated by eQuantic.Mapper.Generator
using System;
using System.Collections.Generic;
using System.Linq;
using eQuantic.Mapper;
using MyCode;
using MyCode.Models;

namespace MyCode
{
    /// <summary>
    /// Mapper implementation for converting ExampleA to ExampleB
    /// </summary>
    public partial class ExampleMapper : IMapper<ExampleA, ExampleB>
    {
        #nullable enable
        /// <summary>
        /// The event before map
        /// </summary>
        public event OnMapHandler<ExampleA, ExampleB>? OnBeforeMap;

        /// <summary>
        /// The event after map
        /// </summary>
        public event OnMapHandler<ExampleA, ExampleB>? OnAfterMap;

        /// <summary>
        /// The mapper factory
        /// </summary>
        protected readonly IMapperFactory? MapperFactory;
        #nullable disable

        /// <summary>
        /// Initializes a new instance of the ExampleMapper class
        /// </summary>
        /// <param name=""mapperFactory"">The mapper factory for creating nested mappers</param>
        public ExampleMapper(IMapperFactory mapperFactory)
        {
            MapperFactory = mapperFactory;
            AfterConstructor();
        }

        #nullable enable
        /// <summary>
        /// Maps a ExampleA to a new ExampleB
        /// </summary>
        /// <param name=""source"">The source object to map from</param>
        /// <returns>A new ExampleB with mapped values</returns>
        public virtual ExampleB? Map(ExampleA? source)
        {
            return Map(source, new ExampleB());
        }
        #nullable disable

        #nullable enable
        /// <summary>
        /// Maps a ExampleA to an existing ExampleB
        /// </summary>
        /// <param name=""source"">The source object to map from</param>
        /// <param name=""destination"">The destination object to map to</param>
        /// <returns>The updated destination object</returns>
        public virtual ExampleB? Map(ExampleA? source, ExampleB? destination)
        {
            if (source == null)
            {
                return null;
            }

            if (destination == null)
            {
                return Map(source);
            }
            InvokeHandler(OnBeforeMap, new MapEventArgs<ExampleA, ExampleB>(source, destination));

            destination.Name = source.Name;
            destination.NullableStringToDate = !string.IsNullOrEmpty(source.NullableStringToDate) ? DateTime.Parse(source.NullableStringToDate) : null;
            destination.StringToDate = DateTime.Parse(source.StringToDate);
            destination.NullableStringToInteger = !string.IsNullOrEmpty(source.NullableStringToInteger) ? Convert.ToInt32(source.NullableStringToInteger) : null;
            destination.StringToInteger = Convert.ToInt32(source.StringToInteger);
            destination.NullableToNonNullableInteger = source.NullableToNonNullableInteger ?? 0;
            if (source.Models?.Any() == true)
            {
                var mapper = MapperFactory?.GetAnyMapper<ExampleModelA, ExampleModelB>();
                if (mapper != null)
                {
                    destination.Models = source.Models.Select(o => mapper.Map(o)!);
                }
            }
            if (source.Model != null)
            {
                var mapper = MapperFactory?.GetAnyMapper<ExampleModelA, ExampleModelB>();
                if (mapper != null)
                {
                    var mappedModel = mapper.Map(source.Model);
                    destination.Model = mappedModel!;
                }
            }
            destination.Enum1 = (ExampleEnumB)(int)source.Enum1;
            destination.Enum2 = (ExampleEnumB)source.Enum2;
            destination.Enum3 = (Int32)source.Enum3;
            destination.IntegerList = source.IntegerList;
            destination.GuidList = source.GuidList;
            InvokeHandler(OnAfterMap, new MapEventArgs<ExampleA, ExampleB>(source, destination));

            return destination;
        }
        #nullable disable

        /// <summary>
        /// Called after the constructor completes to allow custom
        /// initialization
        /// </summary>
        partial void AfterConstructor();

        #nullable enable
        private void InvokeHandler(OnMapHandler<ExampleA, ExampleB>? handler, MapEventArgs<ExampleA, ExampleB> eventArgs)
        {
            if (handler == null)
            {
                return;
            }

            handler(this, eventArgs);
        }
        #nullable disable
    }
}";
        var runResult = driver.GetRunResult();
        var actualCode = runResult.Results[0].GeneratedSources[0].SourceText.ToString().Replace("\r", "");
        Assert.Multiple(() =>
        {
            Assert.That(diagnostics.IsEmpty, Is.True);
            Assert.That(outputCompilation.SyntaxTrees.Count(), Is.EqualTo(2));
            Assert.That(runResult, Is.Not.Null);
            Assert.That(runResult.GeneratedTrees, Has.Length.EqualTo(1));
            Assert.That(runResult.Diagnostics.IsEmpty, Is.True);
            Assert.That(runResult.Results[0].Generator, Is.EqualTo(generator));
            Assert.That(actualCode, Is.EqualTo(expectedCode.Replace("\r", "")));
        });
    }

    private static Compilation CreateCompilation(string source)
        => CSharpCompilation.Create("compilation",
            new[] { CSharpSyntaxTree.ParseText(source) },
            GetReferences(),
            new CSharpCompilationOptions(OutputKind.ConsoleApplication));

    private static IEnumerable<MetadataReference> GetReferences()
    {
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Attribute).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IMapper<,>).GetTypeInfo().Assembly.Location)
        };
        references.AddRange(Assembly.GetEntryAssembly()!.GetReferencedAssemblies().Select(a => MetadataReference.CreateFromFile(Assembly.Load(a).Location)));
        return references;
    }
}
