namespace eQuantic.Mapper.Sample.Models;

public class AddressSource
{
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public CountrySource Country { get; set; } = new();
}

public class CountrySource
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}