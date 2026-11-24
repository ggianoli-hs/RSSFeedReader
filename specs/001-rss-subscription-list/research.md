# Research: RSS Subscription List

## Decisions

### Runtime and framework

- **Decision**: Target .NET 10 LTS (`net10.0`) for both projects.
- **Rationale**: As of 2026-11-24, .NET 10 is supported LTS through 2028-11-14.
  .NET 11 is also supported STS through the same date, but its newer features are not
  needed for this MVP. .NET 10 was released a year earlier and uses the LTS channel.
  .NET 8 and .NET 9 reached end of support on 2026-11-10.
- **Alternatives considered**: .NET 11 STS; rejected because it offers no MVP capability
  needed to justify moving to the newer release when both versions currently share the
  same published support end date.

### API shape and server-side state

- **Decision**: Use ASP.NET Core Minimal APIs with `GET /api/subscriptions` and
  `POST /api/subscriptions`. Keep an insertion-ordered list in one thread-safe,
  process-lifetime service. Return snapshots when listing; append each accepted entry,
  including duplicates. A backend process restart clears the list.
- **Rationale**: Two operations satisfy the specified add/list behavior with no database,
  file storage, item IDs, deletion route, or feed-related dependency. Synchronization
  protects the shared list from concurrent HTTP requests while preserving insertion order.
- **Alternatives considered**: MVC controllers add structure without a need for multiple
  endpoints; persistent storage contradicts the session-only requirement; keyed or
  concurrent unordered collections add complexity or alter duplicate/order behavior.

### Browser interaction

- **Decision**: Use one page with a labeled URL input, an Add submit button, and a semantic
  list. Load the current list from the API on page initialization. For non-blank input,
  submit to the API and update the displayed list from the successful response. Trim
  surrounding whitespace, keep duplicate entries, and clear the input after success.
- **Rationale**: The API remains the source of truth, and a returned item avoids a second
  request after each add. Blank values are ignored without a request. The UI presents an
  empty state and announces results/errors accessibly.
- **Alternatives considered**: Re-fetching the full list after every add is correct but
  performs an unnecessary request. Client-only list state would diverge from the API.

### Security and local operation

- **Decision**: Do not parse, fetch, or link submitted URLs. Render values as text so the
  UI framework encodes them. Bind the API to localhost and allow only the configured local
  UI origin through CORS. Show API failures inline and preserve the user's input.
- **Rationale**: With no outbound request there is no MVP SSRF path. Treating values as
  text avoids treating user input as HTML or a navigable link. A local single-user app
  does not need authentication, but it should not be exposed on a public interface.
- **Alternatives considered**: URL format validation and clickable links are excluded by
  the specification. Authentication and public-host deployment are outside the MVP.

### Verification

- **Decision**: Use focused xUnit tests for API behavior and the documented browser
  scenarios for end-to-end verification.
- **Rationale**: Tests cover blank input, acceptance of non-URL text, duplicates,
  insertion order, and empty state after restarting the API. The quickstart validates the
  integrated UI/API flow without adding a separate frontend testing framework.
- **Alternatives considered**: A browser automation or component-test stack is not needed
  to verify this small MVP and would add setup beyond the current project scope.

## References

- [.NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-10.0)
- [Blazor forms](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/?view=aspnetcore-10.0)
- [Call a web API from Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-10.0)
- [ASP.NET Core dependency injection](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0)
- [ASP.NET Core CORS](https://learn.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-10.0)
- [Prevent cross-site scripting (XSS)](https://learn.microsoft.com/en-us/aspnet/core/security/cross-site-scripting?view=aspnetcore-10.0)
