using Hangfire;
using Hangfire.PostgreSql;
using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YeuBep.Data;
using YeuBep.Data.Interceptors;
using YeuBep.Entities;
using YeuBep.Extends;
using YeuBep.Pipelines.Filter;
using YeuBep.Services;
using YeuBep.ViewModels.Account;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddExtendServicesDefault(builder.Configuration);
builder.Services.AddApplicationServicesDefault();
builder.Services.AddScoped<AuditSaveChangeInterceptor>();

builder.Services.AddSwaggerGen();

TypeAdapterConfig<AuditEntity, AccountInfo>.NewConfig()
    .Map(dest => dest.UserName, src => src.CreatedBy != null ? src.CreatedBy.UserName : null)
    .Map(dest => dest.Avatar,   src => src.CreatedBy != null ? src.CreatedBy.Avatar : null)
    .Map(dest => dest.Bio,      src => src.CreatedBy != null ? src.CreatedBy.Bio : null);
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ValidateModelStateFilter>();
});
builder.Services.AddHangfire(configuration => configuration
    .UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("Postgres"));
    }));

// Thêm Hangfire Server
builder.Services.AddHangfireServer();

builder.Services.AddDistributedPostgresCache(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("Postgres");
    options.SchemaName = builder.Configuration.GetValue<string>("PostgresCache:SchemaName", "public");
    options.TableName = builder.Configuration.GetValue<string>("PostgresCache:TableName", "cache");
    options.CreateIfNotExists = builder.Configuration.GetValue<bool>("PostgresCache:CreateIfNotExists", true);
    options.UseWAL = builder.Configuration.GetValue<bool>("PostgresCache:UseWAL", false);
    var expirationInterval = builder.Configuration.GetValue<string>("PostgresCache:ExpiredItemsDeletionInterval");
    if (!string.IsNullOrEmpty(expirationInterval) && TimeSpan.TryParse(expirationInterval, out var interval)) {
        options.ExpiredItemsDeletionInterval = interval;
    }
    var slidingExpiration = builder.Configuration.GetValue<string>("PostgresCache:DefaultSlidingExpiration");
    if (!string.IsNullOrEmpty(slidingExpiration) && TimeSpan.TryParse(slidingExpiration, out var sliding)) {
        options.DefaultSlidingExpiration = sliding;
    }
});

builder.Services.AddSession(options =>
{
    var idleTimeout = builder.Configuration.GetValue<string>("Section:IdleTimeout");
    if (!string.IsNullOrEmpty(idleTimeout) && TimeSpan.TryParse(idleTimeout, out var timeout))
    {
        options.IdleTimeout = timeout;
    }
    
    options.Cookie.HttpOnly = true;
    
    options.Cookie.IsEssential = true;
    
    var cookieName = builder.Configuration.GetValue<string>("Section:CookieName");
    options.Cookie.Name = cookieName;
    
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    
    options.Cookie.SameSite = SameSiteMode.Lax;
});


builder.Services.AddDbContext<YeuBepDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
    options.AddInterceptors(sp.GetRequiredService<AuditSaveChangeInterceptor>());
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit", true);
    options.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength", 8);
    options.Password.RequireNonAlphanumeric =builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumeric", true);
    options.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase", true);
    options.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase", true);
    
    options.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("Identity:RequireUniqueEmail");
    var defaultLockoutTimeSpan = builder.Configuration.GetValue<string>("Identity:DefaultLockoutTimeSpan");
    if (!string.IsNullOrEmpty(defaultLockoutTimeSpan) && TimeSpan.TryParse(defaultLockoutTimeSpan, out var lockoutTimeSpan))
    {
        options.Lockout.DefaultLockoutTimeSpan = lockoutTimeSpan;
    }
    options.Lockout.MaxFailedAccessAttempts = builder.Configuration.GetValue<int>("Identity:MaxFailedAccessAttempts", 5);
    
    options.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedEmail", true);
})
.AddEntityFrameworkStores<YeuBepDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = builder.Configuration.GetValue<string>("CookieAuth:LoginPath");
    options.LogoutPath = builder.Configuration.GetValue<string>("CookieAuth:LogoutPath");
    options.AccessDeniedPath = builder.Configuration.GetValue<string>("CookieAuth:AccessDeniedPath");
    var expireTimeSpan = builder.Configuration.GetValue<string>("CookieAuth:ExpireTimeSpan");
    if (!string.IsNullOrEmpty(expireTimeSpan) && TimeSpan.TryParse(expireTimeSpan, out var expireTime))
    {
        options.ExpireTimeSpan = expireTime;
    }
    
    options.SlidingExpiration = builder.Configuration.GetValue<bool>("CookieAuth:SlidingExpiration", true);
    options.Cookie.Name = builder.Configuration.GetValue<string>("CookieAuth:CookieName");
    options.Cookie.HttpOnly = builder.Configuration.GetValue<bool>("CookieAuth:HttpOnly", true);
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]
            ?? throw new Exception("Missing client_id in authentication with google ");
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
                                     ?? throw new Exception("Missing client_secret in authentication with google ");
        googleOptions.CallbackPath = builder.Configuration["Authentication:Google:CallBackPath"]; 
    });

var app = builder.Build();


// migration database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<YeuBepDbContext>();
    dbContext.Database.Migrate();
}

app.UseHangfireDashboard("/hangfire");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); 
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "YeuBep API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();


app.UseAuthentication();
app.UseAuthorization(); 

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
