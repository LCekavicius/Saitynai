using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApi.Auth;
using WebApi.Auth.Model;
using WebApi.Data;
using WebApi.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddControllers();

builder.Services.AddIdentity<ERPUser, IdentityRole>()
    .AddEntityFrameworkStores<LaucekERPDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<LaucekERPDbContext>();

builder.Services.AddAuthentication(configureOptions: options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters.ValidAudience = builder.Configuration["JWT:ValidAudience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["JWT:ValidIssuer"];
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
    });

builder.Services.AddTransient<ICompaniesRepository, CompaniesRepository>();
builder.Services.AddTransient<IProductionOrdersRepository, ProductionOrdersRepository>();
builder.Services.AddTransient<IWorksRepository, WorksRepository>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<AuthDbSeeder>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyNames.ResourceOwner, policy => policy.Requirements.Add(new ResourceOwnerRequirement()));
    options.AddPolicy(PolicyNames.CompanyEmployee, policy => policy.Requirements.Add(new ComplayeEmployeeRequirement()));
});

builder.Services.AddSingleton< IAuthorizationHandler, ResourceOwnerAuthorizationHandler>();
builder.Services.AddSingleton< IAuthorizationHandler, CompanyEmployeeAuthorizationHandler>();
var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

var dbSeeder = app.Services.CreateScope().ServiceProvider.GetRequiredService<AuthDbSeeder>();
await dbSeeder.SeedAsync();

app.Run();
