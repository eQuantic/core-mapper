using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample.Mappers;

[Mapper(typeof(PersonNestedSource), typeof(PersonNestedDestination), true)] // VerifyNullability = true
public partial class PersonNullabilityMapper : IMapper
{
}