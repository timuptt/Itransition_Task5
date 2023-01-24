using System.Text;
using Bogus;
using FakeUserData.ApplicationCore.Models;

namespace FakeUserData.ApplicationCore.Extensions;

public static class MistakeExtension
{
    private const string LatinChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string CyrillicChars = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
    private const string PolandChars = "AĄBCĆDEĘFGHIJKLŁMNŃOÓPQRSŚTUVWXYZŹŻaąbcćdeęfghijklłmnńoópqrsśtuvwxyzźż";
    private const string Numbers = "0123456789";

    private static string _regionChars = String.Empty;
    
    private delegate string Mistake(string text, Randomizer randomizer, string symbols);

    private static readonly Mistake[] Mistakes = 
    {
        ChangeSymbol,
        AddSymbol,
        RemoveSymbol
    };

    public static UserData MakeMistakes(this UserData data, double mistakeRate, Randomizer randomizer, string locale)
    {
        SetUpLocale(locale);

        for (int i = 0; i < (int)mistakeRate; i++)
        {
            MakeMistake(data, randomizer);
        }

        if (randomizer.Double() < mistakeRate - (int)mistakeRate)
        {
            MakeMistake(data, randomizer);
        }

        return data;
    }
    
    private static string ChangeSymbol(string text, Randomizer randomizer, string symbols)
    {
        var builder = new StringBuilder(text);

        if (builder.Length > 0)
        {
            builder[randomizer.Number(0, text.Length - 1)] = randomizer.ArrayElement(symbols.ToArray());
        }

        return builder.ToString();
    }

    private static string AddSymbol(string text, Randomizer randomizer, string symbols)
    {
        var builder = new StringBuilder(text);

        return builder.Insert(randomizer.Number(0, text.Length - 1), randomizer.ArrayElement(symbols.ToArray())).ToString();
    }

    private static string RemoveSymbol(string text, Randomizer randomizer, string symbols)
    {
        var builder = new StringBuilder(text);

        if (text.Length > 0)
        {
            builder.Remove(randomizer.Number(0, text.Length - 1), 1);
            return builder.ToString();
        }

        return builder.ToString();
    }

    private static void MakeMistake(this UserData data, Randomizer randomizer)
    {
        var properties = data.GetType().GetProperties().Where(x => x.PropertyType == typeof(string) && x.CanWrite).ToArray();
        var currentProperty = randomizer.ArrayElement(properties);
        var symbols = _regionChars;
        if (currentProperty.Name is "PhoneNumber" or "Id")
        {
            symbols = Numbers;
        }
        var value = currentProperty.GetValue(data)?.ToString();
        currentProperty.SetValue(data, randomizer.ArrayElement(Mistakes)(value, randomizer, symbols));
    }

    private static void SetUpLocale(string locale)
    {
        switch (locale)
        {
            case "ru":
                _regionChars = CyrillicChars;
                break;
            case "en_US":
                _regionChars = LatinChars;
                break;
            case "pl":
                _regionChars = PolandChars;
                break;
        }
    }
}