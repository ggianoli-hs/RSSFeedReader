<!--
Sync Impact Report
Version change: unratified scaffold → 1.0.0
Modified principles: None; this establishes the initial project principles.
Added sections: Security & Technology Constraints; Development & Quality Workflow.
Removed sections: None.
Follow-up TODOs: Confirm the original ratification date.
-->

# RSS Feed Reader Constitution

## Core Principles

### I. MVP Scope and Incremental Delivery
The MVP MUST support adding a subscription URL and displaying the subscription list.
It MUST NOT fetch or parse feeds, persist data, or add unapproved features in the MVP.
Later-stage features MUST be implemented only when their scope is explicitly approved.
This keeps the proof of concept focused and its completion criteria verifiable.

### II. Secure Input and Network Boundaries
All user-provided values MUST be treated as untrusted. The MVP MAY accept URLs without
validation because it does not use them to make network requests. Before feed fetching is
introduced, the backend MUST validate allowed URL schemes and destinations, prevent
requests to loopback, private, link-local, and other non-public addresses, enforce
timeouts and response-size limits, and safely handle redirects. Feed XML parsing MUST
disable external entity resolution, and feed-provided HTML MUST be sanitized before
rendering. These controls prevent the reader from becoming a server-side request or
content-injection vector.

### III. Maintainable Separation of Concerns
The ASP.NET Core Web API MUST own subscription and future feed-operation behavior;
the Blazor WebAssembly UI MUST own presentation and user interaction. Frontend API
addresses and allowed CORS origins MUST come from environment-appropriate configuration,
not scattered hard-coded values. Changes MUST follow existing project conventions and
avoid abstractions that do not serve an approved requirement.

### IV. Verifiable Quality
Changes MUST include focused tests for new or changed behavior, especially subscription
addition, list updates, API contracts, and security-sensitive feed operations. Before a
change is considered complete, the affected projects MUST build and the relevant tests
MUST pass. Test cases MUST verify observable behavior rather than duplicate implementation
details.

### V. Simplicity and Explicit Failure
Implementations MUST use the simplest design that meets the approved scope and MUST avoid
speculative functionality. Failures MUST be surfaced through the established API or UI
error path; code MUST NOT silently swallow errors or report success after an operation
fails. Logs MUST NOT contain secrets or unnecessary user-provided content.

## Security & Technology Constraints

- The application MUST retain the ASP.NET Core Web API and Blazor WebAssembly architecture
  unless an approved amendment changes the technology direction.
- CORS MUST allow only the configured frontend origins required for the current environment.
- Secrets MUST NOT be committed to source control or exposed to the WebAssembly client.
- Feed retrieval and parsing safeguards in Principle II are mandatory before any
  network-based feed feature is released.
- In-memory subscription storage is acceptable for the MVP; persistence is out of scope
  until approved as a later-stage feature.

## Development & Quality Workflow

- Requirements and acceptance criteria MUST identify whether a feature belongs to the MVP,
  Extended-MVP, or a later phase.
- Developers MUST verify the relevant backend and frontend builds and run focused tests
  before considering a change complete.
- Reviews MUST check scope, security boundaries, maintainability, and test evidence against
  this constitution.
- Documentation describing behavior or setup MUST be updated when a change makes it
  inaccurate.

## Governance

This constitution governs project implementation and review. Amendments MUST be proposed
with their rationale and impact, reviewed and approved by the project stakeholders, and
reflected in this document before dependent work proceeds. Versioning follows semantic
versioning: MAJOR for incompatible principle or governance changes, MINOR for new
principles or materially expanded requirements, and PATCH for clarifications that do not
change obligations. Every amendment MUST update the last-amended date and include a
compliance review of affected requirements, plans, tasks, and code. Reviews MUST identify
and resolve conflicts with this constitution before approval.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): confirm original adoption date | **Last Amended**: 2026-11-23
