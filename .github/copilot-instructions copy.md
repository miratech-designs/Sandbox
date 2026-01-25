# Copilot Reviews

## Review Philosophy

* Only comment when you have HIGH CONFIDENCE (>80%) that an issue exists.
* Avoid speculation. Never say "consider changing…" unless it's a real concern.
* Be concise. One sentence per comment when possible.
* Every comment should be actionable, not observational.
* When reviewing text or comments, only flag issues if clarity impacts understanding or functionality.
* Assume the contributor made intentional choices unless there's evidence otherwise.

## Priority Areas (Review These)

### 🛡️ Security & Safety

* Unsafe code (e.g. `unsafe {}` in Rust) without justification.
* Potential command injection (e.g. shell commands using user input).
* Path traversal vulnerabilities.
* Exposure of credentials, tokens, or secrets.
* Missing validation for external inputs.
* Logging sensitive data.
* Improper error handling that leaks stack traces or internal state.

### ✅ Correctness

* Logic errors that might panic, crash, or misbehave.
* Unchecked use of `unwrap()` or `expect()` in production code.
* Race conditions in async or multithreaded code.
* Leaked resources (files, connections, memory).
* Off-by-one errors, incorrect loop boundaries.
* Bad default values (e.g. `Option<bool>` that should just be `false`).
* Defensive programming that adds no real safety.
* Confusing or misleading variable names.
* Tests that don't actually test anything meaningful.

### 🧱 Architecture & Conventions

* Violations of established patterns (e.g. error handling with `anyhow::Result`).
* Async misuse (e.g. blocking operations in async contexts).
* Missing trait bounds or improper trait implementations.
* Inconsistent file/module structure.

## Project-Specific Context

* Language: Rust, using cargo workspaces.
* Crates: `goose`, `goose-cli`, `goose-server`, `goose-mcp`.
* Async runtime: `tokio`.
* Error handling: Use `anyhow::Result`, avoid `unwrap()` in production.
* Follows standards defined in `HOWTOAI.md`.
* `MCP` modules handle critical infra and require detailed scrutiny.

## CI Pipeline Context

Copilot reviews PRs **before** CI finishes. Do NOT flag issues that are already caught by automated tests.

### Rust CI:

* `cargo fmt --check`
* `cargo test --jobs 2`
* `clippy-lint.sh`
* `check-openapi-schema`

### Frontend (Electron Desktop):

* `npm ci`
* `npm run lint:check`
* `npm run test:run`

### CI Setup Steps:

* System dependencies installed.
* Hermit environment activated.
* Cargo/npm caches restored.
* Node dependencies installed using `npm ci`.
📝 FYI: `npx` checks local `node_modules` first. Don't flag missing binaries unless CI will fail.

## Skip These (Low Value)

* Formatting issues (rustfmt, Prettier).
* Warnings already caught by Clippy or linters.
* Broken imports or missing packages (`npm ci` handles this).
* Variable naming nits (unless misleading).
* Redundant comments or suggestions to "add comments."
* Refactoring suggestions that don't fix bugs.
* Suggestions involving personal preference or style.
* Pedantic grammar or spelling, unless it changes meaning.
* Logging suggestions, unless they relate to security or PII.

## Response Format

1. **State the problem clearly.** (1 sentence)
2. **Explain why it matters.** (Optional)
3. **Suggest a fix.** (Code snippet or concrete action)

### Example:

Avoid using `unwrap()` here; it could panic if `None`.  
Use `?` or handle the error explicitly with `if let Some(val) = ...`.

## When to Stay Silent

* If you're not sure something is a problem, say nothing.
* Avoid commenting just to "say something."
* Don't suggest alternatives unless the current approach has real risks.