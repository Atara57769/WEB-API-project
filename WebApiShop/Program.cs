using Microsoft.EntityFrameworkCore;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IPasswordServices, PasswordServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddDbContext<ApiDBContext>
    (options => options.UseSqlServer("Data Source = ATARA; Initial Catalog = ApiDB; Integrated Security = True; Trust Server Certificate=True"));
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
