using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Converters;

namespace YeuBep.Entities;

public class User : IdentityUser
{
    public string Avatar { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<string> Majors { get; set; } = [];
}
[JsonConverter(typeof(StringEnumConverter))]
public enum Role
{
    Default,
    Admin
}

