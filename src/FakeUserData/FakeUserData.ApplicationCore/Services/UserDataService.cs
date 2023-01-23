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
        var randomiser = new Randomizer(seed);

        var fakeAddresses = new Faker<Address>(locale).UseSeed(seed)
            .RuleFor(a => a.State, f => f.Address.State())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            .RuleFor(a => a.SecondAddress, f => f.Address.SecondaryAddress());

        var fakeUserData = new Faker<UserData>(locale).UseSeed(seed)
            .RuleFor(u => u.Id, f => f.Finance.Account())
            .RuleFor(u => u.Gender, f => (int)f.PickRandom<Name.Gender>())
            .RuleFor(u => u.FirstName, f => f.Person.FirstName)
            .RuleFor(u => u.MiddleName, f => f.Person.GetGenderisedMiddleName(locale, randomiser))
            .RuleFor(u => u.LastName, f => f.Person.LastName)
            .RuleFor(u => u.AddressString, f => fakeAddresses.Generate().GetRandomisedAddressString(randomiser))
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumberFormat())
            .FinishWith((f, p) => p.MakeMistakes(mistakeRate, randomiser, locale))
            .GenerateForever();

        return fakeUserData;
    }
}