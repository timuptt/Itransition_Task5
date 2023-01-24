using Bogus;
using FakeUserData.ApplicationCore.Models;

namespace FakeUserData.ApplicationCore.Extensions;

public static class BogusAddressLineExtension
{
    public static string GetRandomisedAddressString(this Address address, Randomizer randomizer)
    {
        var addresses = new[]
        {
            $"{address.State}, {address.City}, {address.Street}, {address.SecondAddress}",
            $"{address.City}, {address.Street}",
            $"{address.City}, {address.Street}, {address.SecondAddress}",
            $"{address.State}, {address.City}, {address.Street}"
        };
        
        return randomizer.ArrayElement(addresses);
    }
}