using Microsoft.EntityFrameworkCore;
using TestTask2;
using TestTask2.Models;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FileUpload}/{action=FileUpload}/{id?}");

app.Run();
