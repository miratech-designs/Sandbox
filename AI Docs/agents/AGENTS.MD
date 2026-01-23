<!-- markdownlint-disable MD025 -->
# Template

# Project Commands
- <command>: <what it does>
​
# Style
- <rule> (with a short example)
​
# Workflow
- Plan before code
- Run <test command>
- Run <lint/typecheck>
​
# Guardrails
- Don’t <risky action>, use <safe alternative>
​
# Project Map
- <entry file>
- <core module>
- <data store>

---

# Sample 1

## Plan Mode

- Make the plan extremely concise. Sacrifice grammar for the sake of concision.
- At the end of each plan, give me a list of unresolved questions to answer, if any.

---

# Sample 2

## 🚨 CRITICAL: CONCURRENT EXECUTION & FILE MANAGEMENT

**ABSOLUTE RULES**:

1. ALL operations MUST be concurrent/parallel in a single message
2. **NEVER save working files, text/mds and tests to the root folder**
3. ALWAYS organize files in appropriate subdirectories
4. **USE CLAUDE CODE'S TASK TOOL** for spawning agents concurrently, not just MCP

### ⚡ GOLDEN RULE: "1 MESSAGE = ALL RELATED OPERATIONS"

**MANDATORY PATTERNS:**

- **TodoWrite**: ALWAYS batch ALL todos in ONE call (5-10+ todos minimum)
- **Task tool (Claude Code)**: ALWAYS spawn ALL agents in ONE message with full instructions
- **File operations**: ALWAYS batch ALL reads/writes/edits in ONE message
- **Bash commands**: ALWAYS batch ALL terminal operations in ONE message
- **Memory operations**: ALWAYS batch ALL memory store/retrieve in ONE message

---

# Sample 3

## Project Overview

This is a documentation website for "Awesome Agentic Patterns" - a curated catalogue of AI agent design patterns.

## Architecture

The project has a unique architecture where pattern documentation drives the entire site:

1. **Pattern Files** (`patterns/*.md`): Source of truth for all patterns
   - Must include YAML front-matter with: title, status, authors, category, tags
   - Content sections: Problem, Solution, Example (with Mermaid diagrams), References

2. **Automated Generation**: The `scripts/build_readme.py` script:
   - Scans all pattern files and extracts metadata
   - Updates README.md between `<!-- PATTERN_LIST_START -->` and `<!-- PATTERN_LIST_END -->` markers
   - Updates mkdocs.yaml navigation structure
   - Groups patterns by category

---

# Sample 4

## Project Overview

This is a Next.js 16 boilerplate for building AI-powered applications with authentication, database, and modern UI components.

### Key Points

- This project uses **OpenRouter** as the AI provider, NOT direct OpenAI
- OpenRouter provides access to 100+ AI models through a single unified API
- Default model: `openai/gpt-5-mini` (configurable via `OPENROUTER_MODEL` env var)
- Users browse models at: https://openrouter.ai/models
- Users get API keys from: https://openrouter.ai/settings/keys

---

