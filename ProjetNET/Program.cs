using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetNET.Modeles;
using ProjetNET.Repositories;
using ProjetNET.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuration de la base de données
var cnx = builder.Configuration.GetConnectionString("dbcon");
builder.Services.AddDbContext<Context>(options => options.UseSqlServer(cnx));

// Configuration d'Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

// Injection des dépendances
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configuration JWT pour l'authentification
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
        ClockSkew = TimeSpan.Zero // Réduit le temps de tolérance pour les tokens expirés
    };
});

// Ajout des services MVC
builder.Services.AddControllers();

// Swagger pour la documentation API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuration du pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ajout des middlewares
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
