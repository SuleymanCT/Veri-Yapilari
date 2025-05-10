using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CallCenterWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages servisini ekle
builder.Services.AddRazorPages();

var app = builder.Build();

// Ortam kontrolü
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
