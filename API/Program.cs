using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors( builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials() //used for signalr
    .WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles(); //for reading wwwroot folder and index.html file
app.UseStaticFiles(); //for reading js

app.MapControllers();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

app.MapFallbackToController("Index", "Fallback"); //added for angular functioning

#region Seeding
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try{
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync(); //runs migrations when application start or restart if there are any changes
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
        await Seed.SeedUsers(userManager,roleManager);
    }catch(Exception ex){
        var logger = services.GetService<ILogger<Program>>();
        logger.LogError(ex, "An error occured during migration.");
    }
#endregion


app.Run();
