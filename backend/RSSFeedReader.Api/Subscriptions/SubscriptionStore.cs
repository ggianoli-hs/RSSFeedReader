namespace RSSFeedReader.Api.Subscriptions;

public sealed class SubscriptionStore
{
    private readonly object _sync = new();
    private readonly List<Subscription> _subscriptions = [];

    public IReadOnlyList<Subscription> GetAll()
    {
        lock (_sync)
        {
            return _subscriptions.ToArray();
        }
    }

    public Subscription Add(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("A subscription URL cannot be empty.", nameof(url));
        }

        var subscription = new Subscription(url.Trim());

        lock (_sync)
        {
            _subscriptions.Add(subscription);
        }

        return subscription;
    }
}
