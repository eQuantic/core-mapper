using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Enums;

namespace eQuantic.Mapper.Sample.Models;

public class PersonDestination
{
    // Concatenation examples
    [MapFrom(typeof(PersonSource), new[] { "FirstName", "LastName" }, MapperPropertyAggregation.ConcatenateWithSpace)]
    public string? FullName { get; set; }

    [MapFrom(typeof(PersonSource), new[] { "FirstName", "MiddleName", "LastName" }, MapperPropertyAggregation.ConcatenateWithComma)]
    public string? CompleteNameWithCommas { get; set; }

    [MapFrom(typeof(PersonSource), new[] { "Department", "Position" }, MapperPropertyAggregation.ConcatenateWithSeparator, " - ")]
    public string? JobTitle { get; set; }

    // Numeric aggregation examples
    [MapFrom(typeof(PersonSource), new[] { "Salary", "Bonus", "Commission" }, MapperPropertyAggregation.Sum)]
    public decimal TotalIncome { get; set; }

    [MapFrom(typeof(PersonSource), new[] { "Salary", "Bonus" }, MapperPropertyAggregation.Max)]
    public decimal HighestPayComponent { get; set; }

    [MapFrom(typeof(PersonSource), new[] { "WorkHours", "OvertimeHours" }, MapperPropertyAggregation.Sum)]
    public int TotalHours { get; set; }

    [MapFrom(typeof(PersonSource), new[] { "Salary", "Bonus", "Commission" }, MapperPropertyAggregation.Average)]
    public decimal AverageIncome { get; set; }

    // Other aggregation examples
    [MapFrom(typeof(PersonSource), new[] { "FirstName", "MiddleName", "LastName" }, MapperPropertyAggregation.FirstNonEmpty)]
    public string? PreferredName { get; set; }

    [MapFrom(typeof(PersonSource), new[] { "Department", "Position", "FirstName" }, MapperPropertyAggregation.Count)]
    public int NonNullFieldsCount { get; set; }
}