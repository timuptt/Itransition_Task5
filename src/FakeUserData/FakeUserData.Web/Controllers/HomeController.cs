using System.Diagnostics;
using Bogus;
using FakeUserData.ApplicationCore.Interfaces;
using FakeUserData.ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using FakeUserData.Web.Models;

namespace FakeUserData.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserDataService _userDataService;

    public HomeController(ILogger<HomeController> logger, IUserDataService userDataService)
    {
        _logger = logger;
        _userDataService = userDataService;
    }

    public IActionResult Index()
    {
        var fakeAddreses = new Faker<Address>()
        
        var fakeUserData = new Faker<UserData>("ru").UseSeed(0)
            .RuleFor(x => x.Id, x => x.Finance.Account())
            .RuleFor(x => x.FullName, x => x.Person.FullName)
            .RuleFor(x => x.Address, x => x.Address.FullAddress())
            .RuleFor(x => x.PhoneNumber, x => x.Phone.PhoneNumberFormat())
            .GenerateForever();

        return View(.Take(20));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}