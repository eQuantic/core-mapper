using eQuantic.Mapper;
using eQuantic.Mapper.Sample.Mappers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var mapper = new ExampleMapper(new MapperFactory(app.Services));

app.Run();
