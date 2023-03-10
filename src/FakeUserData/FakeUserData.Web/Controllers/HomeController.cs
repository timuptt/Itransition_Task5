using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using FakeUserData.ApplicationCore.Interfaces;
using FakeUserData.ApplicationCore.Models;
using FakeUserData.Web.Mappings;
using Microsoft.AspNetCore.Mvc;
using FakeUserData.Web.Models;

namespace FakeUserData.Web.Controllers;


public class HomeController : Controller
{
    private const int PageSize = 20;

    private readonly IUserDataService _userDataService;

    public HomeController(IUserDataService userDataService)
    {
        _userDataService = userDataService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult GetData(RequestDataModel request)
    {
        var data = _userDataService.GetUserData(request.Seed, request.MistakesRate, request.Region.ToString());
        return Json(data.Skip((request.PageNumber - 1) * PageSize).Take(PageSize));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCsv([FromBody] IEnumerable<UserData> persons)
    {
        var path = $"{Directory.GetCurrentDirectory()}{DateTime.Now.Ticks}.csv";

        await using var writer = new StreamWriter(path);

        await using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csvWriter.Context.RegisterClassMap<UserDataCsvMap>();
            await csvWriter.WriteRecordsAsync(persons);
        }

        return PhysicalFile(path, "text/csv");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}