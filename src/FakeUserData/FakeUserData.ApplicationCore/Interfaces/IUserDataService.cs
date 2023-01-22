using FakeUserData.ApplicationCore.Models;

namespace FakeUserData.ApplicationCore.Interfaces;

public interface IUserDataService
{
    public Task<IEnumerable<UserData>> GetUserData(int seed, double issueRate, string locale);
}