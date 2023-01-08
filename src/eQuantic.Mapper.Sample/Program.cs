using eQuantic.Mapper;
using eQuantic.Mapper.Sample.Mappers;
using eQuantic.Mapper.Sample.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IMapperFactory, MapperFactory>();
builder.Services.AddTransient(typeof(IMapper<ExampleA, ExampleB>), typeof(ExampleMapper));
var app = builder.Build();

app.MapGet("/", (IMapperFactory mapperFactory) =>
{
    var mapper = mapperFactory.GetMapper<ExampleA, ExampleB>()!;
    var exampleA = new ExampleA
    {
        Id = "1",
        Date = "2023-01-01"
    };
    var exampleB = mapper.Map(exampleA);
    return exampleB;
});

app.Run();
