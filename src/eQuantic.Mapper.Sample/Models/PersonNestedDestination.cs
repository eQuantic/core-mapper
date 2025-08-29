using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Enums;

namespace eQuantic.Mapper.Sample.Models;

public class PersonNestedDestination
{
    // Simple nested property mapping
    [MapFrom(typeof(PersonNestedSource), "Address.Street")]
    public string AddressStreet { get; set; } = string.Empty;

    // Nested property mapping with multiple levels
    [MapFrom(typeof(PersonNestedSource), "Address.Country.Name")]
    public string CountryName { get; set; } = string.Empty;

    // Deep nested property mapping (3 levels)
    [MapFrom(typeof(PersonNestedSource), "Contact.Social.LinkedIn")]
    public string LinkedInProfile { get; set; } = string.Empty;

    // Multiple nested properties with aggregation - concatenation
    [MapFrom(typeof(PersonNestedSource), 
             new[] { "Address.Street", "Address.Number", "Address.City" }, 
             MapperPropertyAggregation.ConcatenateWithComma)]
    public string FullAddress { get; set; } = string.Empty;

    // Mixed simple and nested properties with aggregation
    [MapFrom(typeof(PersonNestedSource), 
             new[] { "FirstName", "LastName", "Address.Country.Code" }, 
             MapperPropertyAggregation.ConcatenateWithSeparator, " | ")]
    public string PersonSummary { get; set; } = string.Empty;

    // Numeric aggregation with nested properties
    [MapFrom(typeof(PersonNestedSource), 
             new[] { "Salary", "Bonus" }, 
             MapperPropertyAggregation.Sum)]
    public decimal TotalIncome { get; set; }

    // Multiple nested contacts
    [MapFrom(typeof(PersonNestedSource), 
             new[] { "Contact.Email", "Contact.Phone", "Contact.Social.Twitter" }, 
             MapperPropertyAggregation.FirstNonEmpty)]
    public string PrimaryContact { get; set; } = string.Empty;

    // Auto-mapped properties (by name)
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}