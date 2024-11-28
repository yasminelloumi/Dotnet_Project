using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Modeles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var cnx = builder.Configuration.GetConnectionString("dbcon");
builder.Services.AddDbContext<Context>(
    options => options.UseSqlServer(cnx));

builder.Services.AddDbContext<Context>(
    options => options.UseSqlServer(cnx)
    );
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
