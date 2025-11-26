using Microsoft.EntityFrameworkCore;
using YeuBep.Entities;

namespace YeuBep.Extensions;

public static class DbSetExtensions
{
    public static async Task<string> GenerateUniqueUserNameByEmailAddressAsync(this DbSet<User> users, string emailAddress, int randomSuffixLength = 4, CancellationToken cancellationToken = default)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        var normalized = emailAddress.Trim().ToLower().Split("@")[0];
        var existingUsernames = await users
            .Where(u => u.UserName != null && u.UserName.StartsWith(normalized))
            .AsNoTracking()
            .Select(u => u.UserName)
            .ToListAsync(cancellationToken);
        if (!existingUsernames.Contains(normalized))
        {
            return normalized;
        }
        var random = new Random();
        string candidate;
        do
        {
            var suffix = new string([.. Enumerable.Repeat(chars, randomSuffixLength).Select(s => s[random.Next(s.Length)])]);
            candidate = $"{normalized}{suffix}";
        }
        while (existingUsernames.Contains(candidate)); 

        return candidate;
    }
}