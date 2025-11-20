using System.Text;
using API.Data;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors();//Angular talk to API
builder.Services.AddScoped<ITokenService, TokenService>();//Dependency Injection//Lifetime = Scoped(new request instance)
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    var tokenKey = builder.Configuration["TokenKey"]
    ?? throw new Exception("Token Key Not Found - Program.cs");
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,//الـ API هيتأكد إن التوكن اتوقّع بالمفتاح الصحيح.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),//اتولّد بيه التوكن في TokenServic
                                                                                      //وهايتقارن هنا للتأكد إنه صح//لازم يكون نفس الـ tokenKey.
        ValidateIssuer = false,//مش هنشيّك مين أصدر التوكن
        ValidateAudience = false,//ولا هنشيّك مين هيستخدم التوكن
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod()//any header valid and any method valid
.WithOrigins("https://localhost:4200", "http://localhost:4200"));
//لازم الترتيب 
app.UseAuthentication();//1. Authentication → هل فيه Token صح ولا؟
app.UseAuthorization();//🔹 2. Authorization → هل له صلاحيات؟

app.MapControllers();//تسجيل نهايات الـ API

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUser(context);
}
catch (System.Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during Migrations");
}
app.Run();
