using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace eQuantic.Mapper.Options;

[ExcludeFromCodeCoverage]
public class MapperOptions
{
    private readonly List<Assembly> _assemblies = new();
    private static IEnumerable<Assembly> AllAssemblies => AppDomain.CurrentDomain.GetAssemblies();
    public MapperOptions()
    {
        
    }
    public MapperOptions FromAllAssemblies()
    {
        _assemblies.AddRange(AllAssemblies);
        return this;
    }
    
    public MapperOptions FromAssemblies(IEnumerable<Assembly> assemblies)
    {
        _assemblies.AddRange(assemblies);
        return this;
    }
    
    public MapperOptions FromAssembly(Assembly assembly)
    {
        _assemblies.Add(assembly);
        return this;
    }
    
    internal List<Assembly> GetAssemblies() => _assemblies.Any() ? _assemblies : AllAssemblies.ToList();
}
