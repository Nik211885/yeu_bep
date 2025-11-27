using System.Security.Cryptography;

namespace YeuBep.Helpers;

public static class StringHelper
{
    public static string Split20Word(string source)
    {
        var words = source.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length > 20)
        {
            return string.Join(' ', words.Take(20)) + "...";
        }
        else
        {
            return source;
        }
    }
    public static string GeneratorRandomStringBase64(int bytes)
    {
        var rand = new byte[bytes];
        using (var random = RandomNumberGenerator.Create())
        {
            random.GetBytes(rand);
        }
        return Convert.ToBase64String(rand);
    }
}