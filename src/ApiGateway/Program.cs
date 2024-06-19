using ApiGateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddSingleton<PollyPolicies>();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddRequestTimeouts(options =>
{
    options.AddPolicy("customTimeoutPolicy", TimeSpan.FromSeconds(30));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("customCorsPolicy", builder =>
    {
        builder.AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRequestTimeouts();
app.UseCors();
app.MapReverseProxy();

app.Run();
