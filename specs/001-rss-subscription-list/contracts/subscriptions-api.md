# Subscriptions API Contract

Base path: `/api/subscriptions`

The API is used by the local Blazor WebAssembly UI. Responses use JSON. The API is the
source of truth for the active list.

## `GET /api/subscriptions`

Returns all subscriptions in insertion order.

**Success: `200 OK`**

```json
[
  { "url": "https://example.com/feed.xml" },
  { "url": "https://example.com/feed.xml" }
]
```

An empty collection is returned as `[]`. Duplicate values appear as separate entries.

## `POST /api/subscriptions`

Adds one subscription. The body is a JSON object:

```json
{ "url": "https://example.com/feed.xml" }
```

Leading and trailing whitespace is removed. The API rejects an empty or whitespace-only
value, but MUST NOT check whether a non-empty value is a valid or reachable feed URL. It
does not retrieve or parse feed content.

**Success: `200 OK`**

Returns the added subscription:

```json
{ "url": "https://example.com/feed.xml" }
```

**Invalid empty value: `400 Bad Request`**

The response uses `application/problem+json` and identifies the `url` value as required.
The UI normally filters blank input before making this request.

## Operational and security boundaries

- The in-memory collection is cleared when the API process stops or restarts.
- The API is for local use and binds to loopback addresses only.
- CORS allows only the configured local UI origin.
- Subscription values are displayed as encoded text, not rendered as HTML or clickable
  links.
- No authentication, persistence, delete operation, URL-format validation, or feed
  retrieval is part of this contract.
