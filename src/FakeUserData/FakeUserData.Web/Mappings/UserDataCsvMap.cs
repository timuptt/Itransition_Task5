using CsvHelper.Configuration;
using FakeUserData.ApplicationCore.Models;

namespace FakeUserData.Web.Mappings;

public class UserDataCsvMap : ClassMap<UserData>
{
    public UserDataCsvMap()
    {
        Map(u => u.Id);
        Map(u => u.FullName);
        Map(u => u.AddressString);
        Map(u => u.PhoneNumber);
    }
}