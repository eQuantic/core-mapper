using eQuantic.Mapper;
using eQuantic.Mapper.Sample.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMappers();
var app = builder.Build();

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

app.Run();
