using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Sample.Models.SameNameExample;

namespace eQuantic.Mapper.Sample.Mappers;

[Mapper(typeof(SameNameExample), typeof(SameNameExample2))]
public partial class SameNameExampleMapper : IMapper
{
    
}