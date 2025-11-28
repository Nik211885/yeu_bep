using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YeuBep.Entities;

namespace YeuBep.Data.Seeder;

public static class DefaultSettingSeeder
{
    extension(YeuBepDbContext context){

        public async Task SeederDefaultAsync(IPasswordHasher<User> passwordHasher)
        {
            string roleAdminId = Guid.CreateVersion7().ToString();
            // seeder role 
            if (!await context.Roles.AnyAsync())
            {
                var roles = new List<IdentityRole>()
                {
                    new()
                    {
                        Id = Guid.CreateVersion7().ToString(),
                        Name = nameof(Role.Default),
                        NormalizedName = nameof(Role.Default).ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    },
                    new()
                    {
                        Id = roleAdminId,
                        Name = nameof(Role.Admin),
                        NormalizedName = nameof(Role.Admin).ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    }
                };
                context.Roles.AddRange(roles);
            }

            if (!await context.Users.AnyAsync(x => x.UserName == "admin"))
            {
                var admin = new User()
                {
                    Id = Guid.CreateVersion7().ToString(),
                    Avatar =
                        "https://anhvienmimosa.com.vn/wp-content/uploads/2021/09/cach-tao-dang-chup-anh-ngoai-canh-72-600x800.jpg",
                    Bio = string.Empty,
                    Email = "admin@yeubep.vn",
                    FullName = "Administrator",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    AccessFailedCount = 0,
                    ConcurrencyStamp = Guid.CreateVersion7().ToString(),
                    EmailConfirmed = true,
                    NormalizedEmail = "ADMIN@YEUBEP.COM",
                    PhoneNumber = "0999999999",
                    SecurityStamp = Guid.CreateVersion7().ToString(),
                    PhoneNumberConfirmed = true,
                };
                admin.PasswordHash = passwordHasher.HashPassword(admin, "yeubep@2025");
                var adminRoleInstance = await context.Roles.FirstOrDefaultAsync(x=>x.Name == nameof(Role.Admin));
                roleAdminId = adminRoleInstance is not null ? adminRoleInstance.Id : roleAdminId;
                var adminRole = new IdentityUserRole<string>()
                {
                    RoleId = roleAdminId,
                    UserId = admin.Id,
                };
                context.UserRoles.Add(adminRole);
                context.Users.Add(admin);
            }

            await context.SaveChangesAsync();
        }
    }

}