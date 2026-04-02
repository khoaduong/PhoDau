using Backend.Data;
using Backend.Features.Auth;
using Backend.Features.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI Registration
builder.Services.AddSingleton<JwtTokenProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TasksService>();

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

// Serve static files from Frontend folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "..", "Frontend")),
    RequestPath = ""
});

// Fallback to index.html for SPA routing
app.MapFallbackToFile("index.html", new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "..", "Frontend"))
});

// Feature mappings (API routes come after static files)
app.MapAuthEndpoints();
app.MapTasksEndpoints();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();

    if (!db.Users.Any())
    {
        // seed demo users
        authService.CreateUserAsync("admin", "admin123", "Admin").GetAwaiter().GetResult();
        authService.CreateUserAsync("demo", "password123", "User").GetAwaiter().GetResult();
    }
}

app.Run();

// Required for WebApplicationFactory integration tests
public partial class Program { }

