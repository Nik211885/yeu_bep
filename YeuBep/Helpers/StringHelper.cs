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

    public static string ConvertToStringValue(long value)
    {
        return value switch
        {
            < 1_000 => value.ToString(),
            < 999_500 => $"{value / 1_000d:F1}K+",
            _ => $"{value / 1_000_000d:F1}M+"
        };
    }
}