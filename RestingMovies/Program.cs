using RestingMovies.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adds the IEndpointConfiguration Services to the container.
builder.Services.AddEndpointConfigurations();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adds the Endpoints to the application.
app.UseEndpointConfigurations();

app.Run();