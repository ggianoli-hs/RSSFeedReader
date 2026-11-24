# Implementation Plan: RSS Subscription List

**Branch**: `001-rss-subscription-list` | **Date**: 2026-11-24 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/001-rss-subscription-list/spec.md`

## Summary

Deliver the single-user MVP for adding and viewing RSS/Atom subscription URLs. A
Blazor WebAssembly page calls a small ASP.NET Core API. The API keeps an insertion-ordered,
thread-safe in-memory list, accepts duplicate non-empty values without feed URL validation,
and clears the list when its process restarts. No feed requests, parsing, or persistence
are included.

## Technical Context

**Language/Version**: C# / .NET 10 LTS (`net10.0`)

**Primary Dependencies**: ASP.NET Core Minimal APIs, Blazor WebAssembly, built-in
`HttpClient` and JSON support; no feed or persistence libraries

**Storage**: Thread-safe in-memory collection in the API process; cleared on API restart

**Testing**: xUnit API unit/integration tests; end-to-end UI scenarios in the quickstart

**Target Platform**: Local cross-platform API and browser-based WebAssembly UI

**Project Type**: Web application with separate backend and frontend projects

**Performance Goals**: All 5 of 5 acceptance URLs appear within 5 seconds each; a first-time
user completes add-and-confirm within 1 minute

**Constraints**: One local user; no feed fetching, parsing, URL-format validation,
persistence, deletion, or deduplication; ignore blank input; trim surrounding whitespace;
allow duplicates; render submitted values as text, not live links; restrict API binding and
CORS to the local UI configuration

**Scale/Scope**: One local user and one subscription-list page; acceptance checks cover at
least five entries; no production-scale or availability target

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

**Pre-design gate: PASS**

- **MVP Scope and Incremental Delivery**: The design is limited to adding and listing
  subscriptions. Feed retrieval, persistence, and other later-phase features are excluded.
- **Secure Input and Network Boundaries**: The MVP makes no outbound feed requests.
  Submitted values are rendered as encoded text, not executable markup or live links.
- **Maintainable Separation of Concerns**: The API owns list state and behavior; the
  WebAssembly UI owns input and presentation. API origin settings remain configurable.
- **Verifiable Quality**: Focused API tests and runnable acceptance scenarios cover the
  user-visible requirements and restart behavior.
- **Simplicity and Explicit Failure**: Use one page, two API operations, and one in-memory
  collection. API and network failures are surfaced to the user.

**Post-design gate: PASS**. The design introduces no persistence, feed-network behavior,
or additional product scope. The security and testing constraints above are carried into
the API contract and validation guide.

## Project Structure

### Documentation (this feature)

```text
specs/001-rss-subscription-list/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── subscriptions-api.md
└── tasks.md                 # Created by /speckit-tasks
```

### Source Code (repository root)

```text
backend/
├── RSSFeedReader.Api/
│   ├── Contracts/
│   ├── Subscriptions/
│   └── Properties/
└── RSSFeedReader.Api.Tests/

frontend/
└── RSSFeedReader.UI/
    ├── Pages/
    ├── Services/
    ├── wwwroot/
    └── Properties/
```

**Structure Decision**: Use the separate backend and frontend project layout described
in `StakeholderDocuments/TechStack.md`. Keep API contract types and the in-memory
subscription store in the backend; keep the subscription page and API client in the
frontend. Backend behavior is covered by xUnit tests, with browser-level acceptance
scenarios documented in `quickstart.md`.
