using System.Net.Http.Json;

namespace RSSFeedReader.UI.Services;

public sealed class SubscriptionsApiClient(HttpClient httpClient)
{
    private const string CollectionPath = "subscriptions";

    public async Task<IReadOnlyList<SubscriptionDto>> GetAllAsync()
    {
        var subscriptions = await httpClient.GetFromJsonAsync<SubscriptionDto[]>(CollectionPath);
        return subscriptions ?? throw new InvalidDataException("The subscription list response was empty.");
    }

    public async Task<SubscriptionDto> AddAsync(string url)
    {
        using var response = await httpClient.PostAsJsonAsync(CollectionPath, new AddSubscriptionRequest(url));
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<SubscriptionDto>()
            ?? throw new InvalidDataException("The add-subscription response was empty.");
    }

    private sealed record AddSubscriptionRequest(string Url);
}

public sealed record SubscriptionDto(string Url);
