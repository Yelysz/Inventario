using DotNetEnv;
using InventoryApi.Data;
using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables del .env
Env.Load();

// 🔹 Connection string desde .env
var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("❌ La variable SQLSERVER_CONNECTION no está configurada.");
}

// 🔹 JWT settings desde .env
var jwtSettings = new JwtSettings
{
    Key = Environment.GetEnvironmentVariable("JWT_KEY")!,
    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!,
    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!,
    ExpireMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRE_MINUTES")!)
};

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton<JwtService>();

builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));

// Repositorios
builder.Services.AddScoped<IProductosRepository, ProductosRepository>();
builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<IMovimientosInventarioRepository, MovimientosInventarioRepository>();
builder.Services.AddScoped<ITipoMovimientoRepository, TipoMovimientoRepository>();

// 🔹 Auth JWT
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

builder.Services.AddAuthorization();

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory API", Version = "v1" });

    // 🔐 Configuración para usar Bearer en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce: Bearer {tu_token_jwt}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

// 🔹 Middleware de auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
