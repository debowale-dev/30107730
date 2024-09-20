using System;
using System.Diagnostics;
using System.IO;
using Python.Runtime;
using FoodApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext with the connection string from appsettings.json
builder.Services.AddDbContext<RecipeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<RDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<EmbeddingService>(provider =>
{
    // Ensure the correct Python environment is used
    string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "generate_embeddings.py");
    string workingDirectory = @"C:\Users\debow\OneDrive\Desktop\New folder"; // Set this to your IIS application path
    return new EmbeddingService(scriptPath, workingDirectory);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
