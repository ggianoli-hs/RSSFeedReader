using RSSFeedReader.Api.Contracts;
using RSSFeedReader.Api.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (allowedOrigins is null || allowedOrigins.Length == 0)
{
    throw new InvalidOperationException("Configure at least one allowed frontend origin in Cors:AllowedOrigins.");
}

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddSingleton<SubscriptionStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors();

app.MapGet("/api/subscriptions", (SubscriptionStore store) =>
    Results.Ok(store.GetAll()));

app.MapPost("/api/subscriptions", (AddSubscriptionRequest request, SubscriptionStore store) =>
{
    var url = request.Url;
    if (string.IsNullOrWhiteSpace(url))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            ["url"] = ["A non-empty subscription value is required."]
        });
    }

    return Results.Ok(store.Add(url));
});

app.Run();

public partial class Program
{
}
