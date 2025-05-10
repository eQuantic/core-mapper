using System.Diagnostics.CodeAnalysis;

namespace eQuantic.Mapper.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class)]
public class MapperAttribute : Attribute
{
    public Type SourceType { get; }
    public Type DestinationType { get; }
    public Type? Context { get; }
    public bool VerifyNullability { get; set; }
    public bool OmitConstructor { get; set; }
    
    public MapperAttribute(Type sourceType, Type destinationType)
    {
        SourceType = sourceType;
        DestinationType = destinationType;
    }
    
    public MapperAttribute(Type sourceType, Type destinationType, bool verifyNullability)
        : this(sourceType, destinationType)
    {
        VerifyNullability = verifyNullability;
    }
    
    public MapperAttribute(Type sourceType, Type destinationType, bool verifyNullability, bool omitConstructor)
        : this(sourceType, destinationType, verifyNullability)
    {
        OmitConstructor = omitConstructor;
    }
    
    public MapperAttribute(Type sourceType, Type destinationType, Type? context) : this(sourceType, destinationType)
    {
        Context = context;
    }
    
    public MapperAttribute(Type sourceType, Type destinationType, Type? context, bool verifyNullability) 
        : this(sourceType, destinationType, context)
    {
        VerifyNullability = verifyNullability;
    }
    
    public MapperAttribute(Type sourceType, Type destinationType, Type? context, bool verifyNullability, bool omitConstructor) 
        : this(sourceType, destinationType, context, verifyNullability)
    {
        OmitConstructor = omitConstructor;
    }
}
