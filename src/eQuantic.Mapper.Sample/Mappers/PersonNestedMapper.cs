using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample.Mappers;

[Mapper(typeof(PersonNestedSource), typeof(PersonNestedDestination))]
public partial class PersonNestedMapper : IMapper
{
}