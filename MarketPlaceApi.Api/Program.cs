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
using MarketPlaceApi.Api.Handlers;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add la cadena de conexión al servicio de DbContext

var ConnectionString = builder.Configuration.GetConnectionString("MarketPlaceConnection");

//  DbContext services
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


//repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISellerRepository, SellerRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();

// services 
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBankAccountService, BankAccountService>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();


app.MapControllers();


app.Run();

