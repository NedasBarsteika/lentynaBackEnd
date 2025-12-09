using System.Text;
using DotNetEnv;
using lentynaBackEnd.Data;
using lentynaBackEnd.Helpers;
using lentynaBackEnd.Middleware;
using lentynaBackEnd.Repositories.Implementations;
using lentynaBackEnd.Repositories.Interfaces;
using lentynaBackEnd.Services.Implementations;
using lentynaBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

// Load .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Išsaugoti originalius property pavadinimus (nekonvertuoti į camelCase)
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddEndpointsApiExplorer();

// Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Lentyna API",
        Version = "v1",
        Description = "Knygų vertinimo sistemos API"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database Context
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// JWT Authentication
var jwtKey = configuration["Jwt:SecretKey"]!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("ModeratorOrAdmin", policy => policy.RequireRole("moderatorius", "admin"));
    options.AddPolicy("EditorOrAdmin", policy => policy.RequireRole("redaktorius", "admin"));
    options.AddPolicy("Authenticated", policy => policy.RequireAuthenticatedUser());
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Helpers
builder.Services.AddScoped<JwtHelper>();

// Repositories
builder.Services.AddScoped<INaudotojasRepository, NaudotojasRepository>();
builder.Services.AddScoped<IAutoriusRepository, AutoriusRepository>();
builder.Services.AddScoped<IKnygaRepository, KnygaRepository>();
builder.Services.AddScoped<IZanrasRepository, ZanrasRepository>();
builder.Services.AddScoped<INuotaikaRepository, NuotaikaRepository>();
builder.Services.AddScoped<IKomentarasRepository, KomentarasRepository>();
builder.Services.AddScoped<IDIKomentarasRepository, DIKomentarasRepository>();
builder.Services.AddScoped<IIrasasRepository, IrasasRepository>();
builder.Services.AddScoped<INuomoniuForumasRepository, NuomoniuForumasRepository>();
builder.Services.AddScoped<IKnyguKlubasRepository, KnyguKlubasRepository>();
builder.Services.AddScoped<ISekimasRepository, SekimasRepository>();
builder.Services.AddScoped<ICitataRepository, CitataRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAutoriusService, AutoriusService>();
builder.Services.AddScoped<IKnygaService, KnygaService>();
builder.Services.AddScoped<IZanrasService, ZanrasService>();
builder.Services.AddScoped<INuotaikaService, NuotaikaService>();
builder.Services.AddScoped<IKomentarasService, KomentarasService>();
builder.Services.AddScoped<IIrasasService, IrasasService>();
builder.Services.AddScoped<INuomoniuForumasService, NuomoniuForumasService>();
builder.Services.AddScoped<IKnyguKlubasService, KnyguKlubasService>();
builder.Services.AddScoped<ISekimasService, SekimasService>();
builder.Services.AddScoped<ICitataService, CitataService>();
builder.Services.AddSingleton<IOpenAIService, OpenAIService>();
builder.Services.AddHttpClient<IMeteoService, MeteoService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
    await DataSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// Serve static files (images, etc.) from wwwroot
app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
