
using Microsoft.EntityFrameworkCore;
using MarketPlaceApi.Data.Data; 
using MarketPlaceApi.Business.Services.Interfaces;
using MarketPlaceApi.Business.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MarketPlaceApi.Data.Repositories.interfaces;
using MarketPlaceApi.Data.Repositories;
using MarketPlaceApi.Data.Repositories.Interfaces;
// using Microsoft.OpenApi.Models; // se comentará y se usará nombre totalmente calificado


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add la cadena de conexión al servicio de DbContext

var ConnectionString = builder.Configuration.GetConnectionString("MarketPlaceConnection");

// Agregar el DbContext al contenedor de servicios
builder.Services.AddDbContext<MarketplaceDbContext>(options =>
    options.UseSqlServer(ConnectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = true, 
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true, 
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true, 
            ClockSkew = TimeSpan.Zero
        };
    });

    
// builder.Services.AddSwaggerGen();
// Nota: Swagger tiene incompatibilidades con .NET 10 en versiones actuales de Swashbuckle

//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();

// services 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();


//Custum Middlewares 

app.MapControllers();


app.Run();

