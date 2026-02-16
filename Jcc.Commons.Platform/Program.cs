using Jcc.Commons.Platform.Configuration;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Add Commons services (QueryHandlers, CommandHandlers, gRPC)
builder.Services.AddCommonsServices();

// Configure logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var app = builder.Build();

// Configure the HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Map gRPC services
app.MapGrpcServices();

app.UseHttpsRedirection();
app.UseRouting();

await app.RunAsync();
