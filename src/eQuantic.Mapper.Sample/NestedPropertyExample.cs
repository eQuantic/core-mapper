using eQuantic.Mapper.Sample.Models;
using PersonNestedMapper = eQuantic.Mapper.Sample.Mappers.PersonNestedMapper;

namespace eQuantic.Mapper.Sample;

public static class NestedPropertyExample
{
    public static void RunExample()
    {
        Console.WriteLine("=== Nested Property Mapping Example ===");
        
        // Create source object with nested properties
        var source = new PersonNestedSource
        {
            FirstName = "John",
            LastName = "Doe",
            Salary = 5000,
            Bonus = 1000,
            Address = new AddressSource
            {
                Street = "Main St",
                Number = "123",
                City = "New York",
                ZipCode = "10001",
                Country = new CountrySource
                {
                    Name = "United States",
                    Code = "US"
                }
            },
            Contact = new ContactSource
            {
                Email = "john.doe@example.com",
                Phone = "+1-555-0123",
                Social = new SocialMediaSource
                {
                    LinkedIn = "linkedin.com/in/johndoe",
                    Twitter = "@johndoe"
                }
            }
        };

        // Map using the generated mapper
        var mapper = new PersonNestedMapper(null!);
        var destination = mapper.Map(source);

        if (destination != null)
        {
            Console.WriteLine($"Simple nested: AddressStreet = '{destination.AddressStreet}'");
            Console.WriteLine($"Multi-level nested: CountryName = '{destination.CountryName}'");
            Console.WriteLine($"Deep nested: LinkedInProfile = '{destination.LinkedInProfile}'");
            Console.WriteLine($"Aggregated address: FullAddress = '{destination.FullAddress}'");
            Console.WriteLine($"Mixed aggregation: PersonSummary = '{destination.PersonSummary}'");
            Console.WriteLine($"Numeric aggregation: TotalIncome = {destination.TotalIncome}");
            Console.WriteLine($"First non-empty contact: PrimaryContact = '{destination.PrimaryContact}'");
            Console.WriteLine($"Auto-mapped: FirstName = '{destination.FirstName}', LastName = '{destination.LastName}'");
        }
        
        Console.WriteLine("=== End Example ===\n");
    }
}