# CoPilot Prompt Template — Team Boilerplate

Use this file as a copy-paste template for consistent, high-quality CoPilot prompts. Replace bracketed fields.

---

## Template

# [Title]

**Goal:**
[One-sentence outcome]

**Context:**
- Repo/files: [repo, file paths]
- Current behavior: [short summary]
- Related notes: [design decisions, links]

**Inputs:**
- [Code snippet or attach file]
- [Test output or sample data]

**Outputs (format):**
- [e.g., Diff, code file, markdown doc, JSON]

**Constraints:**
- [Style guide, no new deps, line length, security rules...]

**Steps:**
1. [Step 1: short]
2. [Step 2: short]
3. [Step 3: short]

**Tone/Role:**
[Persona and style]

**Clarifying Questions (if any):**
1. [Question 1]
2. [Question 2]

**Acceptance Criteria:**
- [Criteria 1]
- [Criteria 2]

---

## Copy-ready Markdown Template

```markdown
# [Title]

**Goal:**  
[One-sentence outcome]

**Context:**  
- Repo/files: [repo, file paths]  
- Current behavior: [short summary]  
- Related notes: [design decisions, links]

**Inputs:**  
- [Code snippet or attach file]  
- [Test output or sample data]

**Outputs (format):**  
- [e.g., Diff, code file, markdown doc, JSON]

**Constraints:**  
- [Style guide, no new deps, line length, security rules...]

**Steps:**  
1. [Step 1: short]  
2. [Step 2: short]  
3. [Step 3: short]

**Tone/Role:**  
[Persona and style]

**Clarifying Questions (if any):**  
1. [Question 1]  
2. [Question 2]

**Acceptance Criteria:**  
- [Criteria 1]  
- [Criteria 2]
```

---

## Sample Prompts

### Sample 1 — Bug Fix

# Fix failing user-auth integration test

**Goal:**
Fix the failing integration test for user login so CI passes.

**Context:**
- Repo/files: `server/auth/` and `tests/integration/auth.test.js`
- Current behavior: Test times out when calling external OAuth provider stub.
- Related notes: Feature toggles disable external calls in CI.

**Inputs:**
- `tests/integration/auth.test.js` (excerpt)
- `server/auth/index.js` (excerpt)

**Outputs (format):**
- Patch (unified diff) and a one-paragraph explanation of the fix.

**Constraints:**
- No external network calls during tests; use existing test stubs; follow existing ESLint rules.

**Steps:**
1. Identify root cause from test log.
2. Propose minimal change to `server/auth/index.js`.
3. Produce diff and update test to stub provider.

**Tone/Role:**
Senior backend engineer, pragmatic and test-first.

**Clarifying Questions (if any):**
1. Should we keep behavior identical for prod?
2. Are feature toggles editable?

**Acceptance Criteria:**
- Tests pass locally; diff is <75 lines; no new deps.

---

### Sample 2 — Feature Spec / Implementation

# Add "export CSV" to reports page

**Goal:**
Implement "Export CSV" backend endpoint and frontend button.

**Context:**
- Repo/files: `api/reports`, `web/src/components/ReportsPage.jsx`
- Current behavior: Reports page only supports on-screen filters; users requested CSV export.

**Inputs:**
- Sample response JSON for reports (1–2 items)
- Frontend UX mock: one button in header

**Outputs (format):**
- Endpoint code (diff), frontend component changes (diff), and brief migration notes.

**Constraints:**
- Use existing `csv` util; maintain pagination; authenticated users only.

**Steps:**
1. Define API route and query parameters.
2. Implement streaming CSV endpoint in `api/reports`.
3. Add frontend button calling the endpoint and handle download.
4. Add unit and integration tests.

**Tone/Role:**
Full-stack engineer, clear and secure.

**Clarifying Questions (if any):**
1. Max rows expected? 2. Use server-side pagination?

**Acceptance Criteria:**
- Endpoint paginates, uses existing utils, tests added, manual download works.

---

### Sample 3 — Code Review / Refactor

# Review and refactor `data/transform.js`

**Goal:**
Simplify `transform()` to improve readability and add edge-case tests.

**Context:**
- Repo/files: `lib/data/transform.js`
- Current behavior: Complex branching, duplicated code, and missing null handling.

**Inputs:**
- Full `lib/data/transform.js` file

**Outputs (format):**
- Suggested refactor (diff) and 3 unit tests covering edge cases.

**Constraints:**
- Preserve public API; keep performance within 10% of current.

**Steps:**
1. Summarize issues.
2. Propose refactor with small helper functions.
3. Provide diff and tests.

**Tone/Role:**
Refactoring-focused senior engineer.

**Clarifying Questions (if any):**
1. Is backward compatibility required? (yes/no)

**Acceptance Criteria:**
- Tests cover new cases; linter-clean; benchmarks unchanged within 10%.

---

### Sample 4 — Documentation

# Write README section for caching strategy

**Goal:**
Add a "Caching Strategy" section in `docs/ARCHITECTURE.md`.

**Context:**
- Repo/files: `docs/ARCHITECTURE.md`
- Current behavior: No description of cache invalidation or TTL.

**Inputs:**
- Notes: uses Redis with TTL on session keys

**Outputs (format):**
- Markdown snippet ready to insert into `docs/ARCHITECTURE.md`

**Constraints:**
- Keep to ~200 words; use existing doc tone; link to `redis/README.md`.

**Steps:**
1. Summarize current caching layers.
2. Explain TTL and invalidation.
3. Provide short examples and config hints.

**Tone/Role:**
Technical writer focused on clarity.

**Clarifying Questions (if any):**
1. Any privacy or PII-specific rules?

**Acceptance Criteria:**
- 200 words, matches doc tone, includes one code block showing config.

---

## Quick Best-Practices (bulleted)

- **Be specific:** State exact files, snippets, and expected output format.
- **Limit scope:** Smaller prompts produce better, testable artifacts.
- **Provide tests:** Whenever changing code, include tests or test instructions.
- **List constraints:** Security, performance, and dependency rules up front.
- **Ask clarifying questions:** Allow up to 3 to avoid rework.
- **Use role + tone:** Directs voice and level of detail.

---

## Next steps

- To convert this into the repository `README.md`, reply with `1`.
- To create an additional prompt set for a specific repo area, reply with `3` and name the area.
- Or open the file: `BEST PRACTICES/copilot_prompt_template.md`.

---

## Additional Sample Prompts (by area)

### Backend API — Pagination and Rate Limits

# Add cursor pagination and rate-limit headers to list endpoint

**Goal:**
Implement cursor-based pagination and expose rate-limit headers on the `/api/items` endpoint.

**Context:**
- Repo/files: `api/items`, `lib/pagination.js`
- Current behavior: Offset-based pagination and no rate-limit headers.

**Inputs:**
- Sample query response JSON (1 page)
- Existing `lib/pagination.js` snippet

**Outputs (format):**
- Patch (diff) for endpoint and helper, plus API contract example.

**Constraints:**
- Keep backward-compatible query params for 6 months; no new DB indexes.

**Steps:**
1. Define cursor token format.  
2. Update DB query and `lib/pagination.js`.  
3. Add `X-RateLimit-*` headers and docs.

**Tone/Role:**
Pragmatic backend engineer focused on stability.

**Acceptance Criteria:**
- Cursor tokens decode/encode; headers present; tests added.

---

### Frontend UI — Accessibility Fix

# Fix keyboard focus and aria labels on modal dialog

**Goal:**
Make the project modal fully accessible (keyboard focus trap and ARIA attributes).

**Context:**
- Repo/files: `web/src/components/Modal.jsx`, `web/src/styles/modal.css`
- Current behavior: Modal does not trap focus and lacks `aria-modal`.

**Inputs:**
- `Modal.jsx` source; failing axe-core output excerpt

**Outputs (format):**
- Diff for component and one integration test using `@testing-library/react`.

**Constraints:**
- Use existing focus-trap util; keep visual styles unchanged.

**Steps:**
1. Add focus trap and `aria-modal` attributes.  
2. Add keyboard escape handling.  
3. Add accessibility test.

**Tone/Role:**
Frontend engineer with accessibility expertise.

**Acceptance Criteria:**
- axe-core passes for modal; keyboard nav works.

---

### DevOps / Infra — CI Speedup

# Cache dependencies in CI and parallelize slow jobs

**Goal:**
Reduce CI run time by caching dependencies and parallelizing test suites.

**Context:**
- Repo/files: `.github/workflows/ci.yml`, `docker/Dockerfile` (if present)
- Current behavior: CI takes >20 minutes; cache not used.

**Inputs:**
- Current `ci.yml` workflow; list of long-running jobs

**Outputs (format):**
- Updated workflow (diff) and short rollout plan.

**Constraints:**
- Do not change build images; maintain matrix coverage.

**Steps:**
1. Add dependency cache steps.  
2. Split tests into parallel jobs.  
3. Validate on branch with gated runs.

**Tone/Role:**
DevOps engineer, careful and incremental.

**Acceptance Criteria:**
- CI median time reduced by >=30%; caches hit.

---

### Data Pipeline — Backfill Job

# Create a safe backfill job for user metrics

**Goal:**
Implement a backfill job to recompute `user_metrics` for a date range with idempotency.

**Context:**
- Repo/files: `jobs/backfill_metrics.py`, `db/migrations/`  
- Current behavior: Missing metrics for certain historical dates after schema change.

**Inputs:**
- Example bad records, desired output schema

**Outputs (format):**
- Backfill script (diff), runbook with dry-run steps.

**Constraints:**
- Must be idempotent; low DB impact; support dry-run mode.

**Steps:**
1. Design idempotent algorithm.  
2. Implement with batching and dry-run.  
3. Provide rollback and monitoring guidance.

**Tone/Role:**
Data engineer, safety-first.

**Acceptance Criteria:**
- Dry-run matches expected rows; production run completes within SLA.

---

### QA / Tests — Flaky Test Investigation

# Investigate and fix flaky `payment` integration test

**Goal:**
Find root cause of intermittent `payment` test failures and stabilize CI.

**Context:**
- Repo/files: `tests/integration/payment.test.js`, `services/payment/mockServer.js`
- Current behavior: Fails ~1 in 10 runs; timeout or assertion mismatch.

**Inputs:**
- Historical failure logs (last 10 runs) and test code

**Outputs (format):**
- Root cause summary, proposed fix (diff), and retry policy if needed.

**Constraints:**
- Avoid adding large timeouts; prefer deterministic mocks.

**Steps:**
1. Reproduce locally with looped runs.  
2. Review mock behavior and concurrency.  
3. Apply fix and add flaky-test guard if necessary.

**Tone/Role:**
QA engineer with debugging experience.

**Acceptance Criteria:**
- Flakiness eliminated across 50 local runs; CI passes consistently.

---

### Security / Compliance — Secrets Audit

# Scan repo for accidental secrets and add pre-commit checks

**Goal:**
Find any accidental secrets in the history and add checks to prevent recurrence.

**Context:**
- Repo/files: entire repo; `pre-commit-config.yaml` (if present)
- Current behavior: No enforced secret scanning in pre-commit.

**Inputs:**
- List of known secret patterns, current `pre-commit` config

**Outputs (format):**
- Report of findings and patch adding `gitleaks`/`detect-secrets` to pre-commit.

**Constraints:**
- Do not rewrite history automatically; propose manual remediation steps.

**Steps:**
1. Run secret scanners in dry-run.  
2. Produce findings and remediation plan.  
3. Add pre-commit scanner and docs.

**Tone/Role:**
Security engineer, cautious and precise.

**Acceptance Criteria:**
- No high-risk secrets found; pre-commit prevents accidental commits.

