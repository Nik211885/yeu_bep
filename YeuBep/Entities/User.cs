using Microsoft.AspNetCore.Identity;

namespace YeuBep.Entities;

public class User : IdentityUser
{
    public string Avatar { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}