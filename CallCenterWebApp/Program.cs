using CallCenterWebApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages servisini ekle (eksik olan satır buydu!)
builder.Services.AddRazorPages();

var app = builder.Build();

app.MapRazorPages(); // <-- Bu zaten doğru

app.Run();
