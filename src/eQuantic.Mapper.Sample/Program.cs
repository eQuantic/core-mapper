using eQuantic.Mapper;
using eQuantic.Mapper.Sample;
using eQuantic.Mapper.Sample.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMappers();
var app = builder.Build();

// Run examples on startup
NestedPropertyExample.RunExample();
ConditionalMappingExample.RunExample();

app.MapGet("/", (IMapperFactory mapperFactory) =>
{
    var mapper = mapperFactory.GetMapper<ExampleA, ExampleB>()!;
    var exampleA = new ExampleA
    {
        Id = "1",
        Name = "Test",
        Date = "2023-01-01"
    };
    var exampleB = mapper.Map(exampleA);
    return exampleB;
});

app.MapGet("/nested", (IMapperFactory mapperFactory) =>
{
    var mapper = mapperFactory.GetMapper<PersonNestedSource, PersonNestedDestination>()!;
    var source = new PersonNestedSource
    {
        FirstName = "Jane",
        LastName = "Smith",
        Salary = 7500,
        Bonus = 2500,
        Address = new AddressSource
        {
            Street = "Oak Avenue",
            Number = "456",
            City = "San Francisco",
            ZipCode = "94102",
            Country = new CountrySource
            {
                Name = "United States",
                Code = "US"
            }
        },
        Contact = new ContactSource
        {
            Email = "jane.smith@company.com",
            Phone = "+1-555-9876",
            Social = new SocialMediaSource
            {
                LinkedIn = "linkedin.com/in/janesmith",
                Twitter = "@janesmith"
            }
        }
    };
    
    var destination = mapper.Map(source);
    return destination;
});

app.Run();
