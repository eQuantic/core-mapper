using eQuantic.Mapper.Sample.Mappers;
using eQuantic.Mapper.Sample.Models;

namespace eQuantic.Mapper.Sample;

public static class ConditionalMappingExample
{
    public static void RunExample()
    {
        Console.WriteLine("\n=== eQuantic.Mapper Conditional Mapping Demo ===");

        // Setup test data
        var userSource = new UserSource
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Salary = 75000m,
            Age = 25,
            IsEmailVisible = true,
            HasPremiumAccount = false,
            SensitiveData = "SSN: 123-45-6789",
            PhoneNumber = "+1-555-0123"
        };

        // Test 1: Simple conditional mapper (without context)
        Console.WriteLine("\n1. Testing ConditionalUserMapper (without context):");
        var simpleMapper = new ConditionalUserMapper(null!);
        var result1 = simpleMapper.Map(userSource);

        Console.WriteLine($"   Email visible (IsEmailVisible=true): {result1?.PublicEmail}");
        Console.WriteLine($"   Phone (HasPremiumAccount=false): {result1?.Phone ?? "null"}");
        Console.WriteLine($"   Age displayed (Age=25>=18): {result1?.DisplayAge}");
        Console.WriteLine($"   Salary (Context dependent, skipped): {result1?.Salary ?? 0}");
        Console.WriteLine($"   Restricted info (Context dependent, skipped): {result1?.RestrictedInfo ?? "null"}");

        // Test 2: Context-aware conditional mapper
        Console.WriteLine("\n2. Testing ConditionalUserWithContextMapper (with context):");

        // Test 2a: Normal user context
        var contextMapper = new ConditionalUserWithContextMapper(null!);
        contextMapper.Context = new UserContext 
        { 
            IncludeSensitiveData = false,
            ShowContactInfo = false,
            UserRole = "User"
        };

        var result2a = contextMapper.Map(userSource);
        Console.WriteLine("   2a. Normal user context (no sensitive access):");
        Console.WriteLine($"      Email visible: {result2a?.PublicEmail}");
        Console.WriteLine($"      Salary (IncludeSensitiveData=false): {result2a?.Salary?.ToString() ?? "null"}");
        Console.WriteLine($"      Full name (ShowContactInfo=false): {result2a?.FullName ?? "null"}");
        Console.WriteLine($"      Restricted info (UserRole=User): {result2a?.RestrictedInfo ?? "null"}");

        // Test 2b: Admin user context with full access
        Console.WriteLine("\n   2b. Admin context (full access):");
        contextMapper.Context = new UserContext 
        { 
            IncludeSensitiveData = true,
            ShowContactInfo = true,
            UserRole = "Admin"
        };

        var result2b = contextMapper.Map(userSource);
        Console.WriteLine($"      Email visible: {result2b?.PublicEmail}");
        Console.WriteLine($"      Salary (IncludeSensitiveData=true): ${result2b?.Salary}");
        Console.WriteLine($"      Full name (ShowContactInfo=true): {result2b?.FullName}");
        Console.WriteLine($"      Restricted info (UserRole=Admin): {result2b?.RestrictedInfo}");

        // Test 3: Test premium account access
        Console.WriteLine("\n3. Testing premium account phone access:");
        userSource.HasPremiumAccount = true;
        var result3 = simpleMapper.Map(userSource);
        Console.WriteLine($"   Phone (HasPremiumAccount=true): {result3?.Phone}");

        // Test 4: Test age restriction
        Console.WriteLine("\n4. Testing age restriction:");
        userSource.Age = 16;
        var result4 = simpleMapper.Map(userSource);
        Console.WriteLine($"   Age displayed (Age=16<18): {result4?.DisplayAge?.ToString() ?? "null"}");

        Console.WriteLine("\n=== Conditional Mapping Demo completed ===\n");
    }
}