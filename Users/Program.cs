using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Users.BL;
using Users.DAL;

var builder = WebApplication.CreateBuilder(args);

#region Default
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region AspIdentity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<UsersContext>();

#endregion

#region Authentication
builder.Services
.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JWT";
    options.DefaultAuthenticateScheme = "JWT";
})
.AddJwtBearer("JWT", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = Helpers.SecretKeyBuilder(builder.Configuration),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});
#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
});

#endregion

#region Database
var CS = builder.Configuration.GetConnectionString("Users");
builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(CS));
#endregion

#region Repos
builder.Services.AddScoped<IUserRepo, UserRepo>();
#endregion

#region UnitOWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion

#region managers
builder.Services.AddScoped<IUserManager, UserManager>();
#endregion

#region CORS

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
