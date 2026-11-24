# Quickstart: RSS Subscription List

## Prerequisites

- .NET 10 SDK
- A modern browser
- The API and UI configured to use local loopback addresses and the UI origin allowed by
  the API's CORS policy

The stakeholder configuration uses API port `5151` and UI port `5213`; use the active
launch settings if they differ.

## Build and test

From the repository root:

```powershell
dotnet test backend\RSSFeedReader.Api.Tests\RSSFeedReader.Api.Tests.csproj
dotnet build frontend\RSSFeedReader.UI\RSSFeedReader.UI.csproj
```

Expected: tests pass and both applications build without errors.

## Run the application

Start the API in one terminal:

```powershell
dotnet run --project backend\RSSFeedReader.Api\RSSFeedReader.Api.csproj
```

Start the UI in a second terminal:

```powershell
dotnet run --project frontend\RSSFeedReader.UI\RSSFeedReader.UI.csproj
```

Open the UI URL printed by the frontend process. The API should listen on
`http://localhost:5151` or the configured local equivalent.

## Acceptance scenarios

1. Confirm the page shows an empty subscription list.
2. Enter `https://example.com/feed.xml` and choose **Add**. Confirm the value appears
   within 5 seconds without reloading the page.
3. Add the same value again. Confirm two entries appear in insertion order.
4. Submit blank or whitespace-only input. Confirm no entry is added and no blank item
   appears.
5. Submit non-empty text such as `not a feed URL`. Confirm it appears as text and that no
   feed content is fetched or displayed.
6. Stop both local processes, start them again, and confirm the list is empty.
7. Force an API request failure during a UI add. Confirm an inline error is shown and the
   entered text remains available for retry.

The acceptance URLs are display values only; these scenarios do not require any outbound
feed request.
