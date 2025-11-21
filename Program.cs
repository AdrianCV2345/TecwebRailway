// --- NUEVAS LÍNEAS PARA CARGAR .ENV ---
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentsCRUD.Data;
using StudentsCRUD.Services;
using System.Security.Claims; // Añadido para ClaimTypes
using System.Text;
using System.Threading.RateLimiting;

Env.Load(); // Carga las variables del archivo .env al Environment

// --- LECTURA EXPLÍCITA DEL PUERTO (OPCIONAL PERO RECOMENDADO) ---
var port = Environment.GetEnvironmentVariable("PORT");


var builder = WebApplication.CreateBuilder(args);

// Si se define el puerto en .env, lo aplicamos para escuchar en 0.0.0.0
if (!string.IsNullOrEmpty(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}


// --- LECTURA EXPLÍCITA DE VARIABLES DE ENTORNO PARA JWT ---
// Ahora leemos directamente del ambiente, cargado por DotNetEnv
var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
// --- INYECCIÓN DE DEPENDENCIAS (DI) ---
builder.Services.AddScoped<IAuthService, AuthService>();
// (Asegúrate de agregar también IStudentRepository y StudentRepository)
builder.Services.AddScoped<IStudentService, StudentService>();

// Configuración del DbContext (usando InMemory para simplificar, pero en producción usarías una cadena de conexión leída de .env)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
              
    });
});

// --- Swagger/OpenAPI ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- JWT (Autenticación) ---
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        // --- USAMOS LAS VARIABLES LEÍDAS DEL .ENV ---
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),

        // Permitir que el claim de rol sea leído correctamente
        RoleClaimType = ClaimTypes.Role
    };
});

// --- Rate Limit ---
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10;
        opt.Window = TimeSpan.FromSeconds(10);
    });
    options.RejectionStatusCode = 429;
});

var app = builder.Build();

// --- Middlewares y Arranque ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting(); // Añadir UseRouting si no está implícito

app.UseCors(); // Aplicar CORS

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("fixed");

app.Run();