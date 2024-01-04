using BusinessObjects.Mapping;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories.IRepositories;
using Repositories.Repositories;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
//DI for Category
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>()
    .AddDbContext<Prn231AsmTaskManagementContext>(opt =>
    builder.Configuration.GetConnectionString("MyDB"));
//DI for Project
builder.Services.AddTransient<IProjectRepository, ProjectRepository>()
    .AddDbContext<Prn231AsmTaskManagementContext>(opt =>
builder.Configuration.GetConnectionString("MyDB"));
//DT for Team
builder.Services.AddTransient<ITeamRepository, TeamRepository>()
    .AddDbContext<Prn231AsmTaskManagementContext>(opt =>
builder.Configuration.GetConnectionString("MyDB"));
//DI for Member
builder.Services.AddTransient<IMemberRepository, MemberRepository>()
    .AddDbContext<Prn231AsmTaskManagementContext>(opt =>
builder.Configuration.GetConnectionString("MyDB"));
//DI for Task
builder.Services.AddTransient<ITaskRepository, TaskRepository>()
    .AddDbContext<Prn231AsmTaskManagementContext>(opt =>
builder.Configuration.GetConnectionString("MyDB"));
//Auth Configure
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
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            RoleClaimType = ClaimTypes.Role
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
