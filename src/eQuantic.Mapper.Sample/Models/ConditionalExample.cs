using eQuantic.Mapper.Attributes;
using eQuantic.Mapper.Enums;

namespace eQuantic.Mapper.Sample.Models;

// Source models
public class UserSource
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int Age { get; set; }
    public bool IsEmailVisible { get; set; }
    public bool IsAdult { get; set; }
    public string? PhoneNumber { get; set; }
    public bool HasPremiumAccount { get; set; }
    public string SensitiveData { get; set; } = string.Empty;
}

public class UserContext
{
    public bool IncludeSensitiveData { get; set; }
    public bool ShowContactInfo { get; set; }
    public string UserRole { get; set; } = "User";
}

// Destination model with conditional mappings
public class UserDestination
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    // Map email only if IsEmailVisible is true
    [MapFrom(typeof(UserSource), nameof(UserSource.Email))]
    [MapWhen(nameof(UserSource.IsEmailVisible))]
    public string? PublicEmail { get; set; }
    
    // Map salary only if context allows sensitive data
    [MapFrom(typeof(UserSource), nameof(UserSource.Salary))]
    [MapWhen("Context?.IncludeSensitiveData == true", true)]
    public decimal? Salary { get; set; }
    
    // Map phone only if user has premium account
    [MapFrom(typeof(UserSource), nameof(UserSource.PhoneNumber))]
    [MapWhen(nameof(UserSource.HasPremiumAccount))]
    public string? Phone { get; set; }
    
    // Map age only if adult
    [MapFrom(typeof(UserSource), nameof(UserSource.Age))]
    [MapWhen("source.Age >= 18", true)]
    public int? DisplayAge { get; set; }
    
    // Map sensitive data only if context user is admin
    [MapFrom(typeof(UserSource), nameof(UserSource.SensitiveData))]
    [MapWhen("Context?.UserRole == \"Admin\"", true)]
    public string? RestrictedInfo { get; set; }
    
    // Conditional aggregation - show full name only if contact info is allowed
    [MapFrom(typeof(UserSource), new[] { nameof(UserSource.FirstName), nameof(UserSource.LastName) }, 
             MapperPropertyAggregation.ConcatenateWithSpace)]
    [MapWhen("Context?.ShowContactInfo == true", true)]
    public string? FullName { get; set; }
}