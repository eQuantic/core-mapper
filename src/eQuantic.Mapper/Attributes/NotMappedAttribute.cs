using System.Diagnostics.CodeAnalysis;

namespace eQuantic.Mapper.Attributes;

/// <summary>
/// Attribute used to mark properties that should be excluded from mapping.
/// </summary>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Property)]
public class NotMappedAttribute : Attribute
{
    
}
