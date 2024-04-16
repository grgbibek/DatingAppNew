using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors( builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#region Seeding
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try{
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync(); //runs migrations when application start or restart if there are any changes
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        await Seed.SeedUsers(userManager,roleManager);
    }catch(Exception ex){
        var logger = services.GetService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration.");
    }
#endregion


app.Run();
