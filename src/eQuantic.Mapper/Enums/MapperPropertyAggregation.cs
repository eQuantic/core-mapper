namespace eQuantic.Mapper.Enums;

/// <summary>
/// Defines the aggregation methods for combining multiple source properties into a destination property.
/// </summary>
public enum MapperPropertyAggregation
{
    /// <summary>
    /// No aggregation - used for single property mapping.
    /// </summary>
    None,
    
    /// <summary>
    /// Concatenates string values with no separator.
    /// </summary>
    Concatenate,
    
    /// <summary>
    /// Concatenates string values with space separator.
    /// </summary>
    ConcatenateWithSpace,
    
    /// <summary>
    /// Concatenates string values with comma separator.
    /// </summary>
    ConcatenateWithComma,
    
    /// <summary>
    /// Concatenates string values with custom separator (requires additional configuration).
    /// </summary>
    ConcatenateWithSeparator,
    
    /// <summary>
    /// Sums numeric values.
    /// </summary>
    Sum,
    
    /// <summary>
    /// Takes the maximum value from numeric properties.
    /// </summary>
    Max,
    
    /// <summary>
    /// Takes the minimum value from numeric properties.
    /// </summary>
    Min,
    
    /// <summary>
    /// Calculates the average of numeric values.
    /// </summary>
    Average,
    
    /// <summary>
    /// Takes the first non-null/non-empty value.
    /// </summary>
    FirstNonEmpty,
    
    /// <summary>
    /// Takes the last non-null/non-empty value.
    /// </summary>
    LastNonEmpty,
    
    /// <summary>
    /// Counts the number of non-null values.
    /// </summary>
    Count
}