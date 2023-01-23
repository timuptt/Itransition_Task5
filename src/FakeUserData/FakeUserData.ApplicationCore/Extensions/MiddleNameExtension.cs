using System.Net.Security;
using Bogus;
using FakeUserData.ApplicationCore.Constants;

namespace FakeUserData.ApplicationCore.Extensions;

public static class MiddleNameExtension
{
    public static string GetGenderisedMiddleName(this Bogus.Person person, string locale, Randomizer randomizer,  string resourcesPath)
    {
        if (locale != Regions.ru.ToString())
            return "";
        
        return randomizer.ArrayElement(File.ReadAllLines(
            Path.Combine(resourcesPath, $"/Resources/{locale}_{person.Gender}.txt")));
    }
}