using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using YeuBep.Helpers;

namespace YeuBep.Extensions;

public static class StringExtensions
{
    extension(string value)
    {
        public string GeneratorSlug()
        {
            string first = StringHelper.GeneratorRandomStringBase64(6)
                .Replace("/", "")
                .Replace("+", "")
                .TrimEnd('=').ToLowerInvariant();
            
            var normalized = value.Normalize(NormalizationForm.FormD);
            var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            string second = new string(chars.ToArray());
            
            second = second.Normalize(NormalizationForm.FormC).ToLowerInvariant();
            second = Regex.Replace(second, @"[^a-z0-9\s-]", "");
            second = Regex.Replace(second, @"\s+", "-").Trim('-');
            return $"{first}/{second}";
        }
    }
}