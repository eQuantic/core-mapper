using System;

namespace eQuantic.Mapper.Attributes;

/// <summary>
/// Specifies a condition that must be met for the property mapping to occur.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MapWhenAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MapWhenAttribute"/> class with a property name condition.
    /// </summary>
    /// <param name="propertyName">The name of the boolean property on the source object to check.</param>
    public MapWhenAttribute(string propertyName)
    {
        PropertyName = propertyName;
        IsExpression = false;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MapWhenAttribute"/> class with an expression condition.
    /// </summary>
    /// <param name="expression">The C# expression to evaluate. Can reference source properties and Context.</param>
    /// <param name="isExpression">Must be true to indicate this is an expression.</param>
    public MapWhenAttribute(string expression, bool isExpression)
    {
        PropertyName = expression;
        IsExpression = isExpression;
    }

    /// <summary>
    /// Gets the property name or expression to evaluate.
    /// </summary>
    public string PropertyName { get; }
    
    /// <summary>
    /// Gets a value indicating whether PropertyName contains an expression rather than a simple property name.
    /// </summary>
    public bool IsExpression { get; }
}