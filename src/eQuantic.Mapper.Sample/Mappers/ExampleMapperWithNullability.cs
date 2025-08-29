using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample.Mappers;

[Mapper(typeof(ExampleA), typeof(ExampleB), true)]
public partial class ExampleMapperWithNullability : IMapper
{
}