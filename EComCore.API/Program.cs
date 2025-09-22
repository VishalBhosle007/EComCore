using AutoMapper;
using EComCore.API.Middlewares;
using EComCore.Application.Interfaces.Repository;
using EComCore.Application.Interfaces.Services;
using EComCore.Application.Mappings;
using EComCore.Infrastructure.Data;
using EComCore.Infrastructure.Identity;
using EComCore.Infrastructure.Repository;
using EComCore.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);


//Register Dbconext 
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
b => b.MigrationsAssembly("EComCore.Infrastructure")));

// Add Identity with custom ApplicationUser and default IdentityRole
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 6;

}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();

// Disable the default automatic ModelState validation response from ASP.NET Core.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add services to the container.
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

// File storage
builder.Services.AddSingleton<IFileStorageService, LocalFileStorageService>();

//Add AutoMapper
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<AutoMapperProfiles>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);



// Add Controllers
builder.Services.AddControllers();

// Add role based access
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

    });

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
//});

// Add Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EComCore.API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.  
                        Enter 'Bearer' [space] and then your token.  
                        Example: 'Bearer abcdefgh12345'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


builder.Services.AddAuthorization();

var app = builder.Build();

//Seed Roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentitySeeder.SeedRolesAsync(roleManager);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();             // ? Enable Swagger
    app.UseSwaggerUI();          // ? Enable Swagger UI
}

// Enable serving static files from wwwroot
app.UseStaticFiles(); // <-- important for images in wwwroot

app.UseHttpsRedirection();

// Add Exception Middleware here
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
