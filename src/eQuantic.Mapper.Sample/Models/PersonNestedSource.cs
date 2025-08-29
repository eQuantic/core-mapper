namespace eQuantic.Mapper.Sample.Models;

public class PersonNestedSource
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public AddressSource Address { get; set; } = new();
    public ContactSource Contact { get; set; } = new();
    public decimal Salary { get; set; }
    public decimal Bonus { get; set; }
}

public class ContactSource
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public SocialMediaSource Social { get; set; } = new();
}

public class SocialMediaSource
{
    public string LinkedIn { get; set; } = string.Empty;
    public string Twitter { get; set; } = string.Empty;
}