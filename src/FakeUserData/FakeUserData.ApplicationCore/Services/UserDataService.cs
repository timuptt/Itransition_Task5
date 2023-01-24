using Bogus;
using Bogus.DataSets;
using FakeUserData.ApplicationCore.Extensions;
using FakeUserData.ApplicationCore.Interfaces;
using FakeUserData.ApplicationCore.Models;
using Address = FakeUserData.ApplicationCore.Models.Address;

namespace FakeUserData.ApplicationCore.Services;

public class UserDataService : IUserDataService
{
    public IEnumerable<UserData> GetUserData(int seed, double mistakeRate, string locale)
    {
        return GenerateFakeUserData(seed, mistakeRate, locale).GenerateForever();
    }

    private Faker<UserData> GenerateFakeUserData(int seed, double mistakeRate, string locale)
    {
        var randomizer = new Randomizer(seed);

        var fakeUserData = new Faker<UserData>(locale).UseSeed(seed)
            .RuleFor(u => u.Id, f => f.Finance.Account())
            .RuleFor(u => u.Gender, f => (int)f.PickRandom<Name.Gender>())
            .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            .RuleFor(u => u.MiddleName, f => f.Person.GetGenderisedMiddleName(locale, randomizer))
            .RuleFor(u => u.LastName, f => f.Person.LastName)
            .RuleFor(u => u.AddressString, f => GenerateFakeAddresses(seed, locale).Generate().GetRandomisedAddressString(randomizer))
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumberFormat())
            .FinishWith((f, p) => p.MakeMistakes(mistakeRate, randomizer, locale));
            
        return fakeUserData;
    }

    private Faker<Address> GenerateFakeAddresses(int seed, string locale)
    {
        var fakeAddresses = new Faker<Address>(locale).UseSeed(seed)
            .RuleFor(a => a.State, f => f.Address.State())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            .RuleFor(a => a.SecondAddress, f => f.Address.SecondaryAddress());
        return fakeAddresses;
    }
}