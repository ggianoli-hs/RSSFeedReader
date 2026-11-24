# Data Model: RSS Subscription List

## Subscription

Represents one value submitted by the user for display in the active application
session. It does not represent a fetched or parsed feed.

| Field | Type | Constraints |
|---|---|---|
| `url` | string | Required after trimming leading/trailing whitespace; no URL-format or reachability validation |

## Collection behavior

- The API maintains an ordered collection of subscriptions for its process lifetime.
- Each accepted submission appends one entry. Duplicate values are separate entries; there
  is no uniqueness constraint.
- Empty or whitespace-only submissions do not create an entry.
- The list is cleared when the API process is restarted. No state is written to disk.
- The collection is shared by the single local user; it is not partitioned by browser or
  session identity.

## State transitions

1. **Submitted**: The user supplies text.
2. **Ignored**: If the value is empty after trimming, the collection is unchanged.
3. **Added**: Otherwise, the trimmed value is appended and returned to the caller.
4. **Listed**: The entry appears in the ordered list returned by the API.
5. **Cleared**: Restarting the API process initializes an empty collection.
