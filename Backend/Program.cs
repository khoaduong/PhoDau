using Backend.Data;
using Backend.Features.Auth;
using Backend.Features.Hello;
using Backend.Features.Menu;
using Backend.Features.Status;
using Backend.Features.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI Registration
builder.Services.AddSingleton<JwtTokenProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TasksService>();
builder.Services.AddScoped<MenuService>();

// JWT Authentication
var secret = builder.Configuration["JwtSettings:Secret"] ?? "your-super-secret-key-change-this-in-production-12345";
var secretKey = Encoding.UTF8.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User", "Admin"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// CORS for Frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

// Authentication & Authorization middleware (must be before MapEndpoints)
app.UseAuthentication();
app.UseAuthorization();

// Feature mappings
app.MapAuthEndpoints();
app.MapTasksEndpoints();
app.MapHelloEndpoint();
app.MapStatusEndpoint();
app.MapMenuEndpoints();

// Serve built Vite files when Frontend/dist exists.
var frontendDistPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Frontend", "dist");
if (Directory.Exists(frontendDistPath))
{
    var frontendDistProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(frontendDistPath);

    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = frontendDistProvider
    });

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = frontendDistProvider,
        RequestPath = ""
    });

    app.MapFallbackToFile("index.html", new StaticFileOptions
    {
        FileProvider = frontendDistProvider
    });
}

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

    if (!db.Users.Any())
    {
        // seed demo users
        authService.CreateUserAsync("admin", "admin123", "Admin").GetAwaiter().GetResult();
        authService.CreateUserAsync("demo", "password123", "User").GetAwaiter().GetResult();
    }
}

// Development-only menu seed
if (app.Environment.IsDevelopment())
{
    using var menuSeedScope = app.Services.CreateScope();
    var menuSeedDb = menuSeedScope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!menuSeedDb.MenuCategories.Any())
    {
        var phoId = Guid.NewGuid().ToString();
        var pho = new MenuCategory
        {
            Id = phoId,
            Name = "Pho",
            Items =
            [
                new MenuItem { Id = Guid.NewGuid().ToString(), CategoryId = phoId, Name = "Pho Tai", Description = "Rare beef noodle soup", Price = 15.5m },
                new MenuItem { Id = Guid.NewGuid().ToString(), CategoryId = phoId, Name = "Pho Chin", Description = "Well-done beef noodle soup", Price = 16.0m }
            ]
        };

        menuSeedDb.MenuCategories.Add(pho);
        menuSeedDb.SaveChanges();
    }
}

app.Run();

// Required for WebApplicationFactory integration tests
public partial class Program { }

