using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample.Configs;

public class ExampleConfig : MapperConfig<ExampleA, ExampleB>
{
    public ExampleConfig()
    {
        For(dest => dest.Name).Use(src => src.Name);
    }
}