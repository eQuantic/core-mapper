using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;

namespace eQuantic.Mapper.Generator;

[ExcludeFromCodeCoverage]
internal class MapperInfo
{
    public INamedTypeSymbol MapperClass { get; }

    public INamedTypeSymbol SourceClass { get; }
    public INamedTypeSymbol DestinationClass { get; }
    public INamedTypeSymbol? ContextClass { get; }
    
    public MapperInfo(INamedTypeSymbol mapperClass, INamedTypeSymbol sourceClass, INamedTypeSymbol destinationClass, INamedTypeSymbol? contextClass)
    {
        MapperClass = mapperClass;
        SourceClass = sourceClass;
        DestinationClass = destinationClass;
        ContextClass = contextClass;
    }
}
