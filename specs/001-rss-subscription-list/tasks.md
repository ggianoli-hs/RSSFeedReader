# Tasks: RSS Subscription List

**Input**: Design documents from `/specs/001-rss-subscription-list/`

**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/](./contracts/), [quickstart.md](./quickstart.md)

**Tests**: Focused API tests are included because the project constitution requires tests for changed behavior. Browser acceptance scenarios are documented in `quickstart.md`.

**Organization**: Tasks are grouped by user story so the MVP can be implemented and verified as an independent increment.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel with another task in the phase because it edits separate files and has no dependency on incomplete work.
- **[Story]**: Identifies the user story for story-phase tasks.
- Each task names the file or project path to create, change, or verify.

## Phase 1: Setup

**Purpose**: Create the API, UI, and API test project skeletons.

- [ ] T001 [P] Create the .NET 10 ASP.NET Core API project at `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`.
- [ ] T002 [P] Create the standalone .NET 10 Blazor WebAssembly project at `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj`.
- [ ] T003 Create the xUnit test project at `backend/RSSFeedReader.Api.Tests/RSSFeedReader.Api.Tests.csproj`, reference `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj`, and add the ASP.NET Core test host dependency.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Configure local API/UI connectivity and remove template routing conflicts before feature implementation.

**⚠️ CRITICAL**: Complete this phase before beginning User Story 1.

- [ ] T004 [P] Configure the API launch profile to bind to the documented loopback address and port `5151` in `backend/RSSFeedReader.Api/Properties/launchSettings.json`.
- [ ] T005 [P] Configure the UI launch profile for port `5213` and set `ApiBaseUrl` to the API's configured local address in `frontend/RSSFeedReader.UI/Properties/launchSettings.json` and `frontend/RSSFeedReader.UI/wwwroot/appsettings.json`.
- [ ] T006 Configure CORS to allow only the configured local UI origin in `backend/RSSFeedReader.Api/Program.cs`.
- [ ] T007 [P] Remove the generated `Home.razor`, `Counter.razor`, and `Weather.razor` pages and update links and landing-page text in `frontend/RSSFeedReader.UI/Pages/` and `frontend/RSSFeedReader.UI/Layout/NavMenu.razor`.
- [ ] T008 Build the API and cleaned UI projects and verify the template pages are absent before feature-page work using `backend/RSSFeedReader.Api/RSSFeedReader.Api.csproj` and `frontend/RSSFeedReader.UI/RSSFeedReader.UI.csproj`.

**Checkpoint**: The projects build, local origins are configured, and the template root-route conflict is removed.

---

## Phase 3: User Story 1 - Add and View Subscriptions (Priority: P1) 🎯 MVP

**Goal**: Let the local user add non-empty subscription values and see the ordered list update immediately.

**Independent Test**: Start with an empty list; add five distinct non-empty values and confirm each appears within five seconds without reloading. Confirm duplicates appear as separate rows, blank input adds nothing, non-URL text is displayed without fetching feed content, and restarting both local processes leaves the list empty.

### Tests for User Story 1

- [ ] T009 [US1] Add API integration tests for the `GET` and `POST /api/subscriptions` contract, including an initially empty list, trimming and blank rejection, acceptance of non-URL text, duplicate entries in insertion order, and empty state after a fresh API process, in `backend/RSSFeedReader.Api.Tests/SubscriptionsApiTests.cs`.

### Implementation for User Story 1

- [ ] T010 [P] [US1] Define the `Subscription` model in `backend/RSSFeedReader.Api/Subscriptions/Subscription.cs`; follow the canonical `Subscription.url` field definition in [data-model.md](./data-model.md#subscription).
- [ ] T011 [US1] Implement the insertion-ordered, thread-safe in-memory store in `backend/RSSFeedReader.Api/Subscriptions/SubscriptionStore.cs`; apply the canonical `Subscription.url` constraints in [data-model.md](./data-model.md#subscription), preserve duplicates, and keep state process-local.
- [ ] T012 [US1] Implement `GET /api/subscriptions` and `POST /api/subscriptions` in `backend/RSSFeedReader.Api/Program.cs` to match [subscriptions-api.md](./contracts/subscriptions-api.md), return a clear `400` problem response for blank values, and return the added item on success.
- [ ] T013 [P] [US1] Implement the configured-`HttpClient` API client for loading and adding subscriptions in `frontend/RSSFeedReader.UI/Services/SubscriptionsApiClient.cs`; use the request and response shapes in [subscriptions-api.md](./contracts/subscriptions-api.md) and surface request failures to callers.
- [ ] T014 [US1] Implement the root subscription page in `frontend/RSSFeedReader.UI/Pages/Subscriptions.razor` with a labeled input, keyboard-submittable Add action, empty/list states, accessible status and error feedback, blank-input suppression, and encoded plain-text URL display; clear input after success and retain it after failure.

**Checkpoint**: User Story 1 works independently through the UI and API and satisfies the scenarios in `spec.md`.

---

## Phase 4: Polish & Cross-Cutting Concerns

**Purpose**: Verify the integrated MVP and its security and restart boundaries.

- [ ] T015 Run the API tests and every acceptance scenario in `specs/001-rss-subscription-list/quickstart.md`; resolve failures in the affected API, UI, or test files before marking the MVP complete.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No phase dependencies. T001 and T002 can start in parallel; T003 depends on T001 for its project reference.
- **Foundational (Phase 2)**: Depends on Setup. T004, T005, and T007 can start in parallel; T006 depends on the configured UI origin from T005; T008 depends on T004-T007.
- **User Story 1 (Phase 3)**: Depends on the complete Foundational phase. T009 defines the API behavior before implementation. After T009, T010 and T013 can proceed in parallel; T011 depends on T010; T012 depends on T011 and T006; T014 depends on T013.
- **Polish (Phase 4)**: T015 depends on T009-T014 being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Phase 2. It is the only MVP story and has no dependency on another user story.

### Parallel Opportunities

- **Setup**: T001 and T002 are independent project creations.
- **Foundational**: T004, T005, and T007 touch separate project files. T006 follows T005 because it must match the configured UI origin.
- **User Story 1**: After T009, T010 and T013 can run in parallel because they edit backend model and frontend client files respectively. T014 can follow T013 while T011/T012 complete.

### Parallel Example: User Story 1

```text
After T009:
Task: T010 Define Subscription model in backend/RSSFeedReader.Api/Subscriptions/Subscription.cs
Task: T013 Implement API client in frontend/RSSFeedReader.UI/Services/SubscriptionsApiClient.cs

Then:
Task: T011 Implement store after T010
Task: T014 Implement page after T013
Task: T012 Implement endpoints after T011 and T006
```

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 project setup.
2. Complete Phase 2 local configuration and template cleanup.
3. Implement and test User Story 1.
4. Run T015 and verify the independent test criteria.
5. Stop at this MVP; feed fetching, persistence, deletion, and other features remain out of scope.

### Incremental Delivery

There is one user story in this feature. Deliver that story as the complete MVP rather than introducing deferred feed-reader capabilities.

## Notes

- Every task uses the required checkbox, sequential ID, optional parallel/story labels, and explicit file or project paths.
- `Subscription.url` constraints are cited at the model and store implementation tasks.
- `[P]` is reserved for tasks that can work in separate files without waiting on incomplete dependencies.
