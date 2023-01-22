namespace FakeUserData.ApplicationCore.Models;

public class Address
{
    public string Country { get; set; }
    public string State { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string FlatNumber { get; set; }

    public string[] GetAddressStringVariants()
    {
        return new string[]
        {
            $"{State}, {City}, {Street} {HouseNumber}, {FlatNumber}",
            $"{City}, {Street} {HouseNumber}",
            $"{City}, {Street} {HouseNumber}, {FlatNumber}",
            $"{State}, {City}, {Street} {HouseNumber}"
        };
    }
}