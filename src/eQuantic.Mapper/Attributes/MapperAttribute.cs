using System.Diagnostics.CodeAnalysis;
using eQuantic.Mapper.Enums;

namespace eQuantic.Mapper.Attributes;

/// <summary>
/// Attribute used to mark classes as mappers and specify their source and destination types.
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class)]
public class MapperAttribute : Attribute
{
    /// <summary>
    /// Gets the source type for the mapping.
    /// </summary>
    public Type SourceType { get; }
    
    /// <summary>
    /// Gets the destination type for the mapping.
    /// </summary>
    public Type DestinationType { get; }
    
    /// <summary>
    /// Gets the context type for the mapping, if any.
    /// </summary>
    public Type? Context { get; }
    
    public MapperDirection Direction { get; set; } = MapperDirection.Forward;
    
    /// <summary>
    /// Gets or sets a value indicating whether to verify nullability during mapping.
    /// </summary>
    public bool VerifyNullability { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether to omit the constructor in the generated mapper.
    /// </summary>
    public bool OmitConstructor { get; set; }
    
    /// <summary>
    /// Initializes a new instance of the MapperAttribute class.
    /// </summary>
    /// <param name="sourceType">The source type for the mapping.</param>
    /// <param name="destinationType">The destination type for the mapping.</param>
    public MapperAttribute(Type sourceType, Type destinationType)
    {
        SourceType = sourceType;
        DestinationType = destinationType;
    }

    public MapperAttribute(Type sourceType, Type destinationType, MapperDirection direction)
        : this(sourceType, destinationType)
    {
        Direction = direction;
    }
    
    /// <summary>
    /// Initializes a new instance of the MapperAttribute class with nullability verification option.
    /// </summary>
    /// <param name="sourceType">The source type for the mapping.</param>
    /// <param name="destinationType">The destination type for the mapping.</param>
    /// <param name="verifyNullability">Whether to verify nullability during mapping.</param>
    public MapperAttribute(Type sourceType, Type destinationType, bool verifyNullability)
        : this(sourceType, destinationType)
    {
        VerifyNullability = verifyNullability;
    }
    
    /// <summary>
    /// Initializes a new instance of the MapperAttribute class with nullability verification and constructor omission options.
    /// </summary>
    /// <param name="sourceType">The source type for the mapping.</param>
    /// <param name="destinationType">The destination type for the mapping.</param>
    /// <param name="verifyNullability">Whether to verify nullability during mapping.</param>
    /// <param name="omitConstructor">Whether to omit the constructor in the generated mapper.</param>
    public MapperAttribute(Type sourceType, Type destinationType, bool verifyNullability, bool omitConstructor)
        : this(sourceType, destinationType, verifyNullability)
    {
        OmitConstructor = omitConstructor;
    }
    
    /// <summary>
    /// Initializes a new instance of the MapperAttribute class with a context type.
    /// </summary>
    /// <param name="sourceType">The source type for the mapping.</param>
    /// <param name="destinationType">The destination type for the mapping.</param>
    /// <param name="context">The context type for the mapping.</param>
    public MapperAttribute(Type sourceType, Type destinationType, Type? context) : this(sourceType, destinationType)
    {
        Context = context;
    }
    
    /// <summary>
    /// Initializes a new instance of the MapperAttribute class with a context type and nullability verification option.
    /// </summary>
    /// <param name="sourceType">The source type for the mapping.</param>
    /// <param name="destinationType">The destination type for the mapping.</param>
    /// <param name="context">The context type for the mapping.</param>
    /// <param name="verifyNullability">Whether to verify nullability during mapping.</param>
    public MapperAttribute(Type sourceType, Type destinationType, Type? context, bool verifyNullability) 
        : this(sourceType, destinationType, context)
    {
        VerifyNullability = verifyNullability;
    }
    
    /// <summary>
    /// Initializes a new instance of the MapperAttribute class with a context type, nullability verification, and constructor omission options.
    /// </summary>
    /// <param name="sourceType">The source type for the mapping.</param>
    /// <param name="destinationType">The destination type for the mapping.</param>
    /// <param name="context">The context type for the mapping.</param>
    /// <param name="verifyNullability">Whether to verify nullability during mapping.</param>
    /// <param name="omitConstructor">Whether to omit the constructor in the generated mapper.</param>
    public MapperAttribute(Type sourceType, Type destinationType, Type? context, bool verifyNullability, bool omitConstructor) 
        : this(sourceType, destinationType, context, verifyNullability)
    {
        OmitConstructor = omitConstructor;
    }
}
