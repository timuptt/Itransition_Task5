using Bogus;
using FakeUserData.ApplicationCore.Interfaces;
using FakeUserData.ApplicationCore.Models;

namespace FakeUserData.ApplicationCore.Services;

public class UserDataService : IUserDataService
{
    public async Task<IEnumerable<UserData>> GetUserData(int seed, double issueRate, string locale, string country)
    {
        var fakeAddreses = new Faker<Address>(locale).UseSeed(seed)
            .RuleFor(x => x.Country, x => country)
            .RuleFor(x => x.State, x => x.Address.State())
            .RuleFor(x => x.Street, x => x.Address.StreetAddress())
            .RuleFor(x => x.HouseNumber, x => x.Address.BuildingNumber())
            .RuleFor(x => x.FlatNumber, x => x.Address.SecondaryAddress());

            var fakeUserData = new Faker<UserData>("ru").UseSeed(seed)
            .RuleFor(x => x.Id, x => x.Finance.Account())
            .RuleFor(x => x.FirstName, x => x.Person.FirstName)
            .RuleFor(x => x.MiddleName, x => "MiddleName")
            .RuleFor(x => x.LastName, x => x.Person.LastName)
            .RuleFor(x => x.AddressString, x => fakeAddreses.Generate().GetAddressStringVariants())
            .RuleFor(x => x.PhoneNumber, x => x.Phone.PhoneNumberFormat())
            .GenerateForever();
    }
}