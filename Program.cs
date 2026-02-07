using GrabExpressApi.SDK;
using GrabExpressApi.SDK.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Grab Express SDK
var grabConfig = new GrabExpressConfig
{
    ClientId = builder.Configuration["GrabExpress:ClientId"] ?? "",
    ClientSecret = builder.Configuration["GrabExpress:ClientSecret"] ?? "",
    Environment = builder.Configuration["GrabExpress:Environment"] ?? "staging"
};

builder.Services.AddSingleton(grabConfig);
builder.Services.AddSingleton<GrabExpressClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
