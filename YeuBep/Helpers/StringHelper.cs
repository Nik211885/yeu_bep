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
}