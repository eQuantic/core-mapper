using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample.Mappers;

[Mapper(typeof(PersonSource), typeof(PersonDestination))]
public partial class PersonAggregationMapper : IMapper
{
}