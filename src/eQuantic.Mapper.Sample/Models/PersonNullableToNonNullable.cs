using eQuantic.Mapper.Attributes;

namespace eQuantic.Mapper.Sample.Models;

// Source with nullable nested properties
public class PersonNullableSource
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public AddressNullableSource? Address { get; set; }
    public int? Age { get; set; }
    public decimal? Salary { get; set; }
}

public class AddressNullableSource
{
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? City { get; set; }
    public CountryNullableSource? Country { get; set; }
}

public class CountryNullableSource
{
    public string? Name { get; set; }
    public string? Code { get; set; }
}

// Destination with non-nullable properties
public class PersonNonNullableDestination
{
    // Direct nested property mapping - nullable to non-nullable
    [MapFrom(typeof(PersonNullableSource), "Address.Street")]
    public string AddressStreet { get; set; } = string.Empty;

    // Deep nested property mapping - nullable to non-nullable
    [MapFrom(typeof(PersonNullableSource), "Address.Country.Name")]
    public string CountryName { get; set; } = string.Empty;

    // Numeric nullable to non-nullable
    [MapFrom(typeof(PersonNullableSource), "Age")]
    public int PersonAge { get; set; }

    // Decimal nullable to non-nullable
    [MapFrom(typeof(PersonNullableSource), "Salary")]
    public decimal PersonSalary { get; set; }

    // Auto-mapped properties
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}

// Mapper
[Mapper(typeof(PersonNullableSource), typeof(PersonNonNullableDestination))]
public partial class PersonNullableToNonNullableMapper : IMapper
{
}