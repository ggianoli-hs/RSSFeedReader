using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RSSFeedReader.Api.Tests;

public sealed class SubscriptionsApiTests
{
    [Fact]
    public async Task Get_returns_empty_list_for_new_process()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/subscriptions");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var subscriptions = await response.Content.ReadFromJsonAsync<SubscriptionResponse[]>();
        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }

    [Fact]
    public async Task Post_trims_value_and_get_returns_it()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient();

        var response = await Add(client, "  https://example.com/feed.xml  ");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("https://example.com/feed.xml",
            (await response.Content.ReadFromJsonAsync<SubscriptionResponse>())?.Url);

        var subscriptions = await client.GetFromJsonAsync<SubscriptionResponse[]>("/api/subscriptions");
        Assert.Equal("https://example.com/feed.xml", Assert.Single(subscriptions!).Url);
    }

    [Fact]
    public async Task Post_rejects_blank_value_without_adding_an_entry()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient();

        var response = await Add(client, "  \t ");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var subscriptions = await client.GetFromJsonAsync<SubscriptionResponse[]>("/api/subscriptions");
        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }

    [Fact]
    public async Task Post_accepts_non_url_text_and_preserves_duplicate_entries()
    {
        using var factory = CreateFactory();
        using var client = factory.CreateClient();

        Assert.Equal(HttpStatusCode.OK, (await Add(client, "not a feed URL")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Add(client, "not a feed URL")).StatusCode);

        var subscriptions = await client.GetFromJsonAsync<SubscriptionResponse[]>("/api/subscriptions");

        Assert.Equal(new[] { "not a feed URL", "not a feed URL" },
            subscriptions!.Select(subscription => subscription.Url));
    }

    [Fact]
    public async Task New_process_starts_with_an_empty_list()
    {
        using (var firstFactory = CreateFactory())
        using (var firstClient = firstFactory.CreateClient())
        {
            Assert.Equal(HttpStatusCode.OK, (await Add(firstClient, "https://example.com/feed.xml")).StatusCode);
        }

        using var secondFactory = CreateFactory();
        using var secondClient = secondFactory.CreateClient();

        var subscriptions = await secondClient.GetFromJsonAsync<SubscriptionResponse[]>("/api/subscriptions");
        Assert.NotNull(subscriptions);
        Assert.Empty(subscriptions);
    }

    private static WebApplicationFactory<Program> CreateFactory() =>
        new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Development"));

    private static Task<HttpResponseMessage> Add(HttpClient client, string url) =>
        client.PostAsJsonAsync("/api/subscriptions", new { url });

    private sealed record SubscriptionResponse(string Url);
}
