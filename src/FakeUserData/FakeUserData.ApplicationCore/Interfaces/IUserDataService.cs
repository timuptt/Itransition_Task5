using FakeUserData.ApplicationCore.Models;

namespace FakeUserData.ApplicationCore.Interfaces;

public interface IUserDataService
{
    public IEnumerable<UserData> GetUserData(int seed, double mistakeRate, string locale,  string resourcesPath);
}