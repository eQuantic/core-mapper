using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace eQuantic.Mapper.Options;

/// <summary>
/// Configuration options for mapper registration.
/// </summary>
[ExcludeFromCodeCoverage]
public class MapperOptions
{
    private readonly List<Assembly> _assemblies = new();
    private static IEnumerable<Assembly> AllAssemblies => AppDomain.CurrentDomain.GetAssemblies();
    
    /// <summary>
    /// Initializes a new instance of the MapperOptions class.
    /// </summary>
    public MapperOptions()
    {
        
    }
    
    /// <summary>
    /// Configures the mapper to scan all assemblies in the current domain.
    /// </summary>
    /// <returns>The current MapperOptions instance for method chaining.</returns>
    public MapperOptions FromAllAssemblies()
    {
        _assemblies.AddRange(AllAssemblies);
        return this;
    }
    
    /// <summary>
    /// Configures the mapper to scan the specified assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to scan for mappers.</param>
    /// <returns>The current MapperOptions instance for method chaining.</returns>
    public MapperOptions FromAssemblies(IEnumerable<Assembly> assemblies)
    {
        _assemblies.AddRange(assemblies);
        return this;
    }
    
    /// <summary>
    /// Configures the mapper to scan the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for mappers.</param>
    /// <returns>The current MapperOptions instance for method chaining.</returns>
    public MapperOptions FromAssembly(Assembly assembly)
    {
        _assemblies.Add(assembly);
        return this;
    }
    
    /// <summary>
    /// Gets the list of assemblies to scan for mappers.
    /// </summary>
    /// <returns>The list of assemblies, or all assemblies if none were specified.</returns>
    internal List<Assembly> GetAssemblies() => _assemblies.Any() ? _assemblies : AllAssemblies.ToList();
}
