using System.Security.Cryptography;

namespace YeuBep.Helpers;

public static class StringHelper
{
    public static string SplitWord(string source, int length = 20)
    {
        var words = source.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length > length)
        {
            return string.Join(' ', words.Take(length)) + "...";
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