using System.Diagnostics.CodeAnalysis;
using eQuantic.Mapper.Generator.Extensions;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator;

[ExcludeFromCodeCoverage]
internal class MapperInfo(
    INamedTypeSymbol mapperClass,
    INamedTypeSymbol sourceClass,
    INamedTypeSymbol destinationClass,
    INamedTypeSymbol? contextClass,
    bool verifyNullability)
{
    public INamedTypeSymbol MapperClass { get; } = mapperClass;

    public INamedTypeSymbol SourceClass { get; } = sourceClass;
    public INamedTypeSymbol DestinationClass { get; } = destinationClass;
    public INamedTypeSymbol? ContextClass { get; } = contextClass;
    public bool VerifyNullability { get; } = verifyNullability;

    public bool IsAsyncMode() => MapperClass.AllInterfaces.Any(o => o.FullName() == "eQuantic.Mapper.IAsyncMapper");
}
