# CLAUDE.md

## Overview

CLAUDE.md is located at the root. You can create additional CLAUDE.md files in subfolders to provide additional context 
for e.g /CLAUDE.md and /frontend/CLAUDE.md. Additional files don’t overwrite and only append to the context.

if you want to have Claude create it for you

```command
/init. 
```

- CLAUDE.md – your project’s persistent brain that teaches Claude how to code like a pro.
- Feed Claude a persistent set of rules and context before any code was generated.
- Markdown file that auto-injects crucial knowledge and coding rules into every Claude prompt .
- Start by crafting a CLAUDE.md file – Anthropic’s secret weapon for agentic AI coding
- CLAUDE.md serves as Claude’s long-term memory – loaded at the start of each session – covering everything from your tech stack and style guide to those quirky project-specific gotchas.
- Below are 10 killer CLAUDE.md prompts (with examples) that transformed my agentic AI coding workflow from all-nighters to autopilot.

1. Architecture Blueprint

    ```markdown
    ## The Core Architecture
    This project is a full-stack Personal Finance Tracker with a React frontend and Node.js/Express backend:
    - Frontend: React 19 single-page app (Vite) with a component-based UI.
    - Backend: Express.js REST API with a SQLite database.
    - Database: SQLite3 with tables for `transactions`, `categories`, `savings_goals`.
    - Communication: Frontend calls backend at `http://localhost:3001/api/`.
    - Design Pattern: Backend follows MVC – routes for API endpoints, models for DB operations.
    ```

2. Command Center

    ```markdown
    Bash Commands
    - `npm run dev` – Start the development server (auto-reloads on code changes).
    - `npm run build` – Build the project for production (outputs to `dist/` folder).
    - `npm run test` – Run the test suite (uses Jest with coverage).
    - `npm run lint` – Lint all files using ESLint (enforces coding standards).
    ```

3. Style Guide Sheriff

    ```markdown
    Code Style Guidelines
    - Syntax: Use ES Modules (`import`/`export`) rather than CommonJS. Use modern ES6+ features (arrow functions, etc.) where appropriate.
    - Formatting: 2 spaces for indentation. Use single quotes for strings. No trailing semicolons (we run Prettier) – except where necessary in TypeScript (enums, interfaces).
    - Naming: Use `camelCase` for variables/functions, `PascalCase` for React components and classes. Constants in `UPPER_SNAKE_CASE`.
    - Patterns: Prefer functional components with hooks over class components in React. Avoid using any deprecated APIs.
    ```

4. Test Bench Coach

    ```markdown
    Testing Instructions
    - Always follow TDD mindset: for any bug fix or new feature, consider writing tests first or immediately after coding.
    - Use Jest for unit tests. For React components, use @testing-library/react for rendering and assertions.
    - Aim for high coverage on core logic (services, reducers, etc.). Include edge cases (invalid inputs, error states) in tests.
    - Test Naming: use `describe` blocks for modules and `it(‘should …’)` for behaviors. Keep tests clear and focused.
    - Run tests with `npm run test` and ensure all pass before considering a task done.
    ```

5. Error Handling Mantra

    ```markdown
    Error Handling & Debugging
    - Diagnose, Don’t Guess: When encountering a bug or failing test, first explain possible causes step-by-step : docs.claude.com. Check assumptions, inputs, and relevant code paths.
    - Graceful Handling: Code should handle errors gracefully. For example, use try/catch around async calls, and return user-friendly error messages or fallback values when appropriate.
    - Logging: Include helpful console logs or error logs for critical failures (but avoid log spam in production code).
    - No Silent Failures: Do not swallow exceptions silently. Always surface errors either by throwing or logging them.
    ```

6. Clean Code Commandments

    ```markdown
    Clean Code Guidelines
    - Function Size: Aim for functions ≤ 50 lines. If a function is doing too much, break it into smaller helper functions.
    - Single Responsibility: Each function/module should have one clear purpose. Don’t lump unrelated logic together.
    - Naming: Use descriptive names. Avoid generic names like `tmp`, `data`, `handleStuff`. For example, prefer `calculateInvoiceTotal` over `doCalc`.
    - DRY Principle: Do not duplicate code. If similar logic exists in two places, refactor into a shared function (or clarify why both need their own implementation).
    - Comments: Explain non-obvious logic, but don’t over-comment self-explanatory code. Remove any leftover debug or commented-out code.
    ```

7. Security Sentry

    ```markdown
    Security Guidelines
    - Input Validation: Validate all inputs (especially from users or external APIs). Never trust user input – e.g., check for valid email format, string length limits, etc.
    - Authentication: Never store passwords in plain text. Use bcrypt with a salt for hashing passwords. Implement account lockout or rate limiting on repeated failed logins.
    - Database Safety: Use parameterized queries or an ORM to prevent SQL injection. Do not concatenate user input in SQL queries directly.
    - XSS & CSRF: Sanitize any HTML or user-generated content before rendering (consider using a library like DOMPurify). Use CSRF tokens for state-changing form submissions.
    - Dependencies: Be cautious of eval or executing dynamic code. Avoid introducing packages with known vulnerabilities (Claude should prefer built-in solutions if external libs are risky).
    ```

8. Teamwork Protocol

    ```markdown
    Collaboration & Workflow
    - Git Branches: Follow GitFlow lite – create feature branches off `dev` (e.g., `feature/login-form`), merge via Pull Request. Do **not** commit directly to `main`.
    - Commit Messages: Use conventional commits (e.g., `feat: `, `fix: `, `docs: ` prefixes). Include JIRA ticket ID in commit if available. Keep message concise (one line summary, optional details after).
    - Pull Requests: When a task is done, have Claude open a PR with a brief description of changes and tag the relevant reviewers (e.g., `@frontend-team` for UI changes).
    - Documentation: If code changes affect user-facing behavior or APIs, update the relevant Markdown docs in the `docs/` folder as part of the same PR.
    - Code Reviews: Claude should assist in code reviews if asked (e.g., static analysis for bugs, ensure style guide adherence) and only approve when all checks pass.
    ```

9. Edge-Case Oracle

    ```markdown
    Edge Case Considerations
    - Always consider edge and corner cases for any logic:
    - Empty or null inputs (e.g., an empty list, missing fields, zero values).
    - Max/min values and overflow (e.g., extremely large numbers, very long text).
    - Invalid states (e.g., end date before start date, negative quantities).
    - Concurrency issues (e.g., two users editing the same data simultaneously).
    - If an edge case is identified, handle it in code or at least flag it with a comment/TODO.
    - Prefer to fail fast on bad input (throw an error or return a safe default) rather than proceeding with wrong assumptions.
    ```

10. Agentic Workflow Guardrails

    ```markdown
    Workflow & Planning Guidelines
    - For any complex or multi-step task, Claude should first output a clear plan or outline of the approach anthropic.com. (E.g., list the steps or modules needed).
    - Incremental Development: Implement in logical chunks. After each chunk, verify it aligns with the plan and passes tests before moving on.
    - Think Aloud: Use extended reasoning (“think harder or ultrathink”) for complex decisions. It’s okay to spend more tokens to ensure a solid approach rather than rushing coding.
    - User Approval: Pause for confirmation after providing a plan or major design decision. Only proceed once the user/developer confirms.
    - Error Recovery: If a solution isn’t working, Claude should backtrack and rethink rather than stubbornly persisting. Consider alternative approaches if tests fail or constraints are hit.
    ```
