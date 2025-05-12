using CallCenterAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// CORS Ayarý
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<CallCenterSimulator>();
builder.Services.AddSingleton<GraphSimulator>();

var app = builder.Build();

// Middleware pipeline
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Simülasyon thread’i
var sim = app.Services.GetRequiredService<CallCenterSimulator>();

var simulationThread = new Thread(() =>
{
    while (true)
    {
        sim.AssignCustomers(); // müþteri atamasý
        Thread.Sleep(1000);    // her 1 saniyede bir
    }
});
simulationThread.IsBackground = true;
simulationThread.Start();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
