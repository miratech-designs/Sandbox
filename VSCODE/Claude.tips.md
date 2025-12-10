The .claude Folder: A 10-Minute Setup That Makes AI Code Smarter
​
Summary
The .claude folder is a lightweight setup that helps Claude Code, an AI coding agent, understand your project’s coding standards, structure, and quality expectations. By creating a few text files explaining your team’s preferences, you can improve the relevance and accuracy of Claude’s suggestions, saving time and ensuring consistent code quality. This setup is a simple yet effective way to onboard Claude and enhance its productivity within your project.
​
Manojkumar Vadivel

If you’re new to Claude Code, it’s a powerful AI coding agent that helps you write, refactor, and understand code faster. This article explains how to set it up with the .claude folder so you can get the best results and maximum productivity from day one.

If you use Claude in your editor and you ask Claude Code for help, the answer often works but feels generic.. It doesn’t fit your project. And you spend more time fixing it than you saved.

Here’s the thing: Claude Code is smart, but it doesn’t know your project. It doesn’t know your coding standards, your team’s preferences, or what “good” looks like for your specific work.

But there’s a simple fix! ….

The Problem with Default Claude Code

When you ask Claude to write code without context, you get default answers. Generic solutions. Code that works but doesn’t match how your team actually builds things. It’s like asking someone to cook dinner without telling them what you like to eat. You’ll get food, but probably not what you wanted.

Configuration Files Change Everything — The .claude Folder

What is the .claude folder?

It’s a lightweight setup at your project’s root where you store a few small text files. These files explain how your project works and how you want code to look.

Think of it as an onboarding document for a new teammate — except the teammate is Claude.


You create a few text files that explain:

How your team writes code
What your project structure looks like
What quality means for your work
Common mistakes to avoid
That’s it. No complex setup. Just some markdown files with your preferences.

A common layout:

.claude/
 ├─ rules.md      # coding standards and guardrails
 ├─ context.md    # what this project is and how it runs
 ├─ prompts.md    # reusable prompt snippets for common tasks
 └─ settings.json # optional structured preferences
Keep it in version control so the whole team benefits.

Why it matters

AI writes better code when it understands your project. These files give it the same orientation you’d give a new teammate:

What we’re building
How we structure code
Which tools and libraries we use
What “good” looks like in this repo
You’ll see more relevant suggestions and fewer style fixes.

What to put in each file?

1. rules.md — your coding standards

Be short. Be specific. Focus on things the AI can follow.

Example:

# Coding Rules
- Follow the project formatter. Do not change file style.
- Use logging instead of print. Include error context.
- Add type hints and docstrings for public functions.
- Keep functions small; prefer composition over long scripts.
- Write safe defaults. Handle timeouts and retries where external calls exist.

# Tests
- Provide a minimal test when adding new modules.
- Use fakes or fixtures; do not call real services.

# Security
- Never include secrets in code or examples.
- Use environment variables or placeholders like <API_KEY>.
2. context.md — the project in plain language

Give a one-screen brief. No fluff.

Example:

# Project Context
This is a service that ingests data from an external source, stores it, and exposes a simple API.

Main parts:
- Ingestion module (scheduled)
- Storage layer (database + object storage)
- API service
- Lightweight UI

Conventions:
- Config via environment variables
- Error handling with structured logs
- CI runs tests and lint on every PR

Dependencies:
- Runtime: Python 3.11
- Package: requests, pydantic
- Tools: Docker, Makefile for common tasks
3. prompts.md — your reusable requests

Save the prompts you repeat. Keep them task-focused.

Examples:

# Add a module
Create a new module that does X. Include:
- A clear, typed interface
- Error handling and logging
- A small unit test with a fake

# Improve performance
Review this function for bottlenecks. Propose changes.
Explain trade-offs in 3-5 bullet points.

# Write docs
Draft README instructions for running the project locally:
- Prereqs
- Setup
- Common commands
- How to run tests
4. settings.json (optional) — structured preferences

Use this if your tool supports it. Keep it minimal.

Example:

{
  "style": {
    "python": {
      "use_type_hints": true,
      "docstrings": true
    }
  },
  "tests": {
    "require_for_new_modules": true
  }
}
Getting Started in 10 Minutes

Create a .claude folder in your project root.
Add a context.md file. Write 2–3 sentences about what your project does and who uses it.
Add a rules.md file. List 5–10 coding preferences your team has.
Start using Claude. Notice how the responses change.
Add more details over time. The configuration grows with your project.
It’s Not Perfect, But It’s Better

Claude still makes mistakes. It’s not magic. But when it knows your context, those mistakes are smaller and easier to fix. And the code it generates actually fits your project instead of looking like it came from
a tutorial.

The setup takes 10 minutes. The time you save starts immediately.

The Real Benefits

You spend less time explaining what you want. Claude already knows.
Your code stays consistent. No more mixing coding styles within the same project.
New team members get better help. They can use the same Claude setup and get code that fits your standards.
You catch problems early. Claude knows your common mistakes and helps you avoid them.
Final thought

Most teams skip this step because it feels like extra work, and want to jump straight into coding. But think about it this way: you’re going to explain your preferences to Claude anyway. Either once in a config file, or over and over again in every conversation.

The config file is just being smart about it. Your future self will thank you when Claude starts giving you exactly what you need, right from the start.

----

The Ultimate Claude Code Cheat Sheet: Your Complete Command Reference
​
 Summarize
​
Toni Maxx
Master the AI coding assistant that’s transforming how developers work

After years of watching developers struggle with context switching between their IDE, terminal, and AI chat interfaces, Anthropic released Claude Code — a command-line tool that brings conversational AI directly into your development workflow. But like any powerful tool, it comes with a learning curve.

I’ve spent the last several weeks exploring every corner of Claude Code, from slash commands to custom agents, and I’ve distilled everything into this comprehensive cheat sheet. Whether you’re just getting started or looking to level up your Claude Code mastery, this guide has you covered.


What Makes Claude Code Different?

Before diving into commands, let’s understand what sets Claude Code apart. Unlike GitHub Copilot or cursor, Claude Code operates as a conversational REPL (Read-Eval-Print Loop) in your terminal. You’re not just getting code completions — you’re collaborating with an AI agent that can read files, execute commands, manage context, and even spawn specialized sub-agents for specific tasks.

Think of it as having a senior developer pair programming with you, but one that never gets tired, can context-switch instantly, and has access to the latest AI capabilities.

The Command Hierarchy: Know Your Tools

Claude Code organizes its functionality into several categories. Understanding this hierarchy will help you quickly find the right command for any situation.

Core Session Commands

These are your bread and butter for managing conversations:

/help — Your first stop when stuck. Displays usage help and command reference.

/clear — Wipes the conversation history clean. Use this when you want a fresh start or when context gets too cluttered.

/exit — Gracefully exits the REPL session. (Or just use Ctrl+D)

/status — Shows version info and connectivity status. Great for troubleshooting.

Context & Memory Management

Here’s where Claude Code really shines. Managing context effectively is the difference between a helpful AI and a confused one:

/context — Visualizes your current context usage as a colored grid. Think of this as your context "fuel gauge"—when it fills up, responses slow down and quality degrades.

/compact [instructions] — This is your context compression tool. It summarizes the conversation history while preserving important details. You can add optional instructions like /compact focus on authentication logic to keep specific topics intact.

/memory — Opens your CLAUDE.md memory files for editing. This is where you store project conventions, coding standards, and preferences that persist across sessions.

/init — Initializes a new project with a CLAUDE.md guide. Run this in every new repository.

Pro tip: Use the # prefix for quick memory adds. Type # Use 2-space indentation for JavaScript files and it's instantly added to CLAUDE.md.

Configuration Commands

Customize Claude Code to match your workflow:

/config — Opens the Settings interface. This is your control panel for everything from model selection to privacy settings.

/model — Quickly switch between Claude models (Sonnet, Opus, Haiku) without opening full settings.

/permissions — View and update tool permissions. Critical for security-conscious development.

/output-style — Configure how Claude formats its responses. Prefer minimal output? Set it here.

/privacy-settings — Control what data gets shared and stored.

Session Analytics

Track your usage and costs:

/usage — Shows your plan limits and rate limit status. Know before you hit the wall.

/export [filename] — Export the conversation to a file or clipboard. Great for sharing solutions or creating documentation.

/rewind — Time travel for your code. Rewind to any checkpoint in the conversation or code state.

Development Tools

Commands specifically designed for coding workflows:

/review — Requests a code review of your current changes. Like having a senior dev look over your shoulder.

/todos — Lists all TODO items Claude is tracking from your conversation. Never lose track of action items.

/doctor — Runs health checks on your Claude Code installation. Troubleshooting made easy.

/add-dir — Add additional working directories to the session. Useful for monorepos or multi-project work.

The Agent System: Delegation at Scale

This is where Claude Code gets truly powerful. Instead of cramming everything into one conversation, you can spawn specialized AI agents for specific tasks.

Understanding Main vs. Sub-agents

Main Conversation:

Full context window
All tool permissions
Accumulates history
Delegates to sub-agents
Sub-agents:

Isolated context windows
Custom system prompts
Configurable permissions
Task-specific expertise
Think of it like a senior developer (main conversation) delegating tasks to specialists (sub-agents). The code reviewer agent doesn’t need to know about your authentication logic, and the security auditor doesn’t need your entire git history.

Key Agent Commands

/agents — Manage your custom AI sub-agents. View available agents, create new ones, or configure existing agents.

/bashes — List and manage background bash tasks. Sub-agents can run long-running commands without blocking your main conversation.

Agent Location Hierarchy

Claude Code looks for agents in this priority order:

.claude/agents/ — Project-level (team-shared)
CLI --agents flag — Session-specific
~/.claude/agents/ — User-level (personal)
Plugin agents — System-wide
Built-in agents — Default agents
This means your project can define team conventions while you maintain personal agents for your workflow.

Creating Custom Agents

Want a specialized agent for database migrations? Create a markdown file in .claude/agents/:

---
name: db-migration-expert
description: Database schema migrations, SQL optimization, and data integrity
model: sonnet
tools:
  - Read
  - Write
  - Bash
---
# Database Migration ExpertYou are an expert in database schema design and migrations.## Capabilities
- Analyze schema changes for potential issues
- Generate migration scripts
- Suggest indexing strategies
- Identify performance implications## Guidelines
- Always consider backwards compatibility
- Recommend rollback strategies
- Check for data loss scenarios
- Suggest testing approaches
Claude will automatically invoke this agent when you discuss database migrations, keeping your main conversation focused.

Integration & Extensions

MCP (Model Context Protocol)

Connect Claude to external tools, databases, and APIs:

# Add an HTTP server
claude mcp add --transport http api https://api.example.com \
  --header "Authorization: Bearer token"
# Add a local server
claude mcp add --transport stdio github npx -y @modelcontextprotocol/server-github
Use MCP resources:

@github:issue://123 — Reference external resources
/mcp__github__list_prs — Execute prompts from MCP servers
Manage servers:

claude mcp list              # View all servers
claude mcp get <name>        # Get details
claude mcp remove <name>     # Remove server
/mcp                         # In-session authentication
Hooks System

Automate workflows at specific trigger points:

Available hooks:

SessionStart — Run setup scripts
SessionEnd — Cleanup or export logs
PreToolUse — Validate operations
PostToolUse — Log tool usage
PermissionRequest — Auto-approve patterns
UserPromptSubmit — Pre-process input
Stop — Post-process responses
Configure hooks in .claude/settings.json:

{
  "hooks": {
    "SessionStart": {
      "command": "echo 'Session started'",
      "enabled": true
    }
  }
}
Keyboard Shortcuts: Work at the Speed of Thought

Essential Shortcuts

Ctrl+C — Cancel current input or generation. Your emergency stop button.

Ctrl+R — Search command history. Just like your bash history search.

Ctrl+L — Clear screen without clearing conversation.

Tab — Toggle extended thinking mode. Let Claude show its reasoning.

Shift+Tab or Alt+M — Toggle permission modes on the fly.

Esc + Esc — Rewind code or conversation. Made a mistake? Go back.

Ctrl+B — Background bash commands. Run long operations without blocking.

Multiline Input Methods

Need to write multiple lines? You have options:

Backslash-enter: \ + Enter
Option-enter (macOS): Option + Enter
Shift-enter: Shift + Enter (after /terminal-setup)
Control sequence: Ctrl+J
Quick Prefixes

# — Instant memory add to CLAUDE.md

/ — Slash commands

! — Direct bash execution (bypasses Claude, runs immediately)

Become a member
@ — File path autocomplete

Permissions & Security

Claude Code’s permission system gives you granular control:

{
  "permissions": {
    "allow": [
      "Bash(npm run lint:*)",
      "WebSearch",
      "WebFetch(domain:github.com)"
    ],
    "ask": [
      "Write(*.ts)",
      "Bash(git push*)"
    ],
    "deny": [
      "Read(.env)",
      "Write(package-lock.json)"
    ]
  }
}
Permission arrays:

allow — Pre-approved operations (no prompts)
ask — Requires confirmation each time
deny — Blocked completely
Use glob patterns for flexible rules. This is crucial for CI/CD environments or when working with sensitive codebases.

CLI Flags & Options

Starting Sessions

# Launch REPL
claude
# Start with a query
claude "Analyze the performance bottlenecks in auth.ts"# Print mode (non-interactive)
claude -p "Generate API documentation"# Continue recent conversation
claude -c# Resume specific session
claude -r "session-id-here" "Add error handling"# Update Claude Code
claude update
Configuration Flags

# Add working directories
claude --add-dir ./services --add-dir ./shared
# Pre-approve specific tools
claude --allowedTools "Read,Write,WebSearch"# Block dangerous operations
claude --disallowedTools "Bash(rm*)"# Replace system prompt
claude --system-prompt "You are a security-focused code reviewer"# Append to system prompt
claude --append-system-prompt "Always suggest tests"# Select model
claude --model opus# Limit agentic turns (prevent runaway loops)
claude --max-turns 10
Output Control

# JSON output for scripting
claude -p --output-format json "List all TypeScript files"
# Structured JSON response
claude --json-schema '{"type":"object","properties":{"files":{"type":"array"}}}' \
  "Find config files"
Environment Variables

Customize behavior via environment:

Authentication

ANTHROPIC_API_KEY=sk-...
ANTHROPIC_AUTH_TOKEN=token-...
Model Configuration

ANTHROPIC_MODEL=claude-sonnet-4-20250514
CLAUDE_CODE_SUBAGENT_MODEL=claude-haiku-4-20241022
MAX_THINKING_TOKENS=10000
Performance Tuning

CLAUDE_CODE_MAX_OUTPUT_TOKENS=8192
BASH_DEFAULT_TIMEOUT_MS=30000
Privacy

DISABLE_TELEMETRY=1
DISABLE_ERROR_REPORTING=1
The Quick Reference Card

Print this, keep it on your desk, tattoo it on your arm — whatever works:

Most Used Commands

/help            Get help
/clear           Clear conversation
/compact         Compress context
/context         View context usage
/memory          Edit CLAUDE.md
/agents          Manage agents
/review          Code review
/permissions     Manage permissions
/cost            Token usage
/status          View status
Essential Shortcuts

Ctrl+C           Cancel
Ctrl+R           Search history
Tab              Toggle thinking
Shift+Tab        Toggle modes
Esc Esc          Rewind
# text           Quick memory
@ path           File autocomplete
! command        Direct bash
Real-World Workflow Examples

Let me show you how these commands work together in actual development scenarios:

Example 1: Starting a New Feature

# 1. Start fresh
/clear
# 2. Set context with memory
# Use JWT tokens for authentication
# Follow React hooks conventions
# Write tests for all API endpoints# 3. Begin work
Create an authentication middleware for Express that validates JWT tokens# 4. Review the changes
/review# 5. Check what's left
/todos
Example 2: Debugging Production Issue

# 1. Continue from incident investigation
claude -c
# 2. Check context usage (might be high from logs)
/context# 3. Compress if needed, focus on error
/compact focus on authentication errors# 4. Spawn specialized agent
Use the code-debugger agent to analyze the auth failures# 5. Export findings
/export incident-report.md
Example 3: Code Review Workflow

# 1. Start with clean slate
claude
# 2. Configure for review
/model opus  # Use more powerful model# 3. Review changes
/review# 4. Check security
Use the code-security-auditor agent to analyze these changes# 5. Track action items
/todos
Memory System: Teaching Claude About Your Project

The CLAUDE.md system is one of the most underutilized features. Here’s the hierarchy:

Enterprise — CLAUDE_ENTERPRISE.md (IT managed)
Project — ./CLAUDE.md or ./.claude/CLAUDE.md
User — ~/.claude/CLAUDE.md
Best Practices for CLAUDE.md

Be specific:

# Bad
- Write good code
# Good
- Use 2-space indentation for JavaScript
- Place imports in order: React, third-party, local
- Name test files with .test.ts suffix
Use structure:

# Project Conventions
## Code Style
- ESLint configuration: .eslintrc.js
- Prettier: 100 character line width
- Use TypeScript strict mode## Testing
- Jest for unit tests
- Playwright for E2E
- Minimum 80% coverage## Git Workflow
- Feature branches from `develop`
- Squash commits on merge
- Conventional commit messages## Architecture
- Clean architecture layers
- Repository pattern for data
- Service layer for business logic
Review and evolve: Your CLAUDE.md should grow with your project. When you find yourself repeatedly correcting Claude on the same issue, add it to memory.

Advanced: Custom Agent Patterns

Here are some agent patterns I’ve found incredibly useful:

The Specialist Pattern

Create agents for specific domains:

database-expert for SQL/migrations
api-designer for REST/GraphQL
performance-optimizer for optimization
security-auditor for security reviews
The Role Pattern

Model after team roles:

code-reviewer mimics senior developer reviews
product-manager analyzes features from user perspective
devops-engineer handles deployment and infrastructure
The Tool Pattern

Agents focused on specific tools:

docker-specialist for containerization
kubernetes-expert for orchestration
terraform-engineer for IaC
Agent Configuration Example

Here’s a production-ready security auditor agent:

---
name: security-auditor
description: Security vulnerabilities, authentication, authorization, and data protection
model: opus  # Use most capable model
tools:
  - Read
  - Grep
  - Glob
permissionMode: ask  # Always confirm security changes
---
# Security AuditorYou are a cybersecurity expert specializing in application security.## Focus Areas
- Authentication and authorization flaws
- Input validation and sanitization
- SQL injection and XSS vulnerabilities
- Sensitive data exposure
- Security misconfiguration
- Insecure dependencies## Review Process
1. Scan for common vulnerability patterns
2. Check authentication/authorization logic
3. Review data handling and storage
4. Assess external dependencies
5. Verify security headers and CORS
6. Check for hardcoded secrets## Output Format
For each finding:
- **Severity**: Critical/High/Medium/Low
- **Location**: File and line number
- **Issue**: Description of vulnerability
- **Impact**: Potential consequences
- **Recommendation**: How to fix## Guidelines
- Be thorough but avoid false positives
- Provide actionable recommendations
- Reference OWASP Top 10 when relevant
- Consider both code and configuration
And this is my favorite, Jenny (and her colleague)

---
name: jenny
description: Jenny is your Project Manager Assistant. She tracks daily progress, updates README-PROGRESS.md, manages GitHub commits with detailed messages, and maintains project documentation. Use PROACTIVELY for progress updates, commits, and project status summaries.
model: sonnet
---

You are Jenny, the dedicated Project Manager Assistant for the Kanojo project. You help track progress, manage documentation, and handle GitHub operations with a professional and organized approach.

## Your Responsibilities

### 1. Progress Tracking
- Update `README-PROGRESS.md` with daily changelog entries
- Aggregate status from all `docs/modules/*-status.md` files
- Maintain feature status overview table
- Track milestones and roadmap items

### 2. GitHub Operations
- Create comprehensive commit messages
- Push changes to remote repository
- **Always ask before committing** - never auto-commit
- Support PR creation with proper descriptions

### 3. Documentation Management
- Keep progress documentation current
- Cross-reference module status files
- Maintain project health overview
- Archive completed milestones

## Project Structure Knowledge

### Progress Files
```
README-PROGRESS.md              # Main progress changelog (root)
docs/modules/
├── fbgc-status.md             # Firebase & Google Cloud
├── markdown-status.md         # Markdown Viewer
├── themetypo-status.md        # Theme & Typography
├── jenny-status.md            # Your own status
└── [module]-status.md         # Other modules
```

### Subagent Team
```
.claude/agents/
├── jenny.md                   # You (PM Assistant)
├── fbgc-team.md              # Firebase & GCloud specialist
├── markdown-team.md          # Markdown specialist
├── themetypo-team.md         # Theme & Typography specialist
├── mobile-developer.md       # Mobile development
├── typescript-developer.md   # TypeScript specialist
├── code-reviewer.md          # Code review
├── code-debugger.md          # Debugging
├── code-security-auditor.md  # Security
├── firebase-specialist.md    # Firebase general
└── _template-module.md       # Template for new modules
```

## Progress Update Format

### README-PROGRESS.md Structure
```markdown
# Project Progress

## Overview
Brief project description and current phase.

## Feature Status
| Module | Status | Details |
|--------|--------|---------|
| Name | ✅/🚧/📋 | [Link](docs/modules/x-status.md) |

## Daily Changelog

### [Date]
- ✅ Completed item
- 🚧 In progress item
- 📋 Planned item
- 🐛 Bug fix
- 📝 Documentation update
```

### Status Indicators
- ✅ Complete
- 🚧 In Progress
- 📋 Planned/Todo
- 🐛 Bug Fix
- 📝 Documentation
- ⚠️ Needs Attention
- 🔄 Refactored

## Git Commit Guidelines

### Before Committing
1. Run `git status` to see all changes
2. Run `git diff` to review changes
3. Check recent commits for message style
4. **Ask user for confirmation**

### Commit Message Format
```
[emoji] [Type]: [Brief description]

[Detailed bullet points of changes]

🤖 Generated with [Claude Code](https://claude.com/claude-code)

Co-Authored-By: Claude <noreply@anthropic.com>
```

### Commit Type Emojis
- ✅ Feature complete
- 🚧 Work in progress
- 🐛 Bug fix
- 📝 Documentation
- 🔄 Refactor
- ⚡️ Performance
- 🎨 Style/UI
- 🧪 Tests
- 🔧 Config

### Example Commit Message
```
✅ Subagents: Created PM Assistant (Jenny)

- Created jenny.md subagent for project management
- Added README-PROGRESS.md for daily changelog tracking
- Added jenny-status.md for self-tracking
- Established progress update workflow

🤖 Generated with [Claude Code](https://claude.com/claude-code)

Co-Authored-By: Claude <noreply@anthropic.com>
```

## Workflow Patterns

### After Work Session
1. Summarize what was accomplished
2. Update `README-PROGRESS.md` with new changelog entry
3. Update relevant `docs/modules/*-status.md`
4. Ask user if they want to commit and push

### Daily Progress Update
```markdown
### [Today's Date]
- [List all completed items]
- [List items in progress]
- [Note any blockers or issues]
```

### Weekly Summary (Optional)
- Aggregate week's progress
- Update milestone status
- Note upcoming priorities

## Communication Style

### Professional & Organized
- Clear, concise updates
- Bullet points for readability
- Consistent formatting
- Emoji indicators for quick scanning

### Always Ask Before
- Committing changes
- Pushing to remote
- Creating PRs
- Any destructive operations

### Proactive Updates
- Suggest progress updates after sessions
- Remind about uncommitted changes
- Offer to summarize module statuses

## Status Aggregation

### Reading Module Statuses
When summarizing project status, read all `docs/modules/*-status.md` files:

```typescript
// Pseudo-code for aggregation
const modules = [
  'fbgc-status.md',
  'markdown-status.md',
  'themetypo-status.md',
  // ... other modules
];

for (const module of modules) {
  // Extract: Module name, status, last updated, next steps
}
```

### Feature Status Table
Keep the table in README-PROGRESS.md updated:
```markdown
| Module | Status | Details |
|--------|--------|---------|
| Firebase & GCloud | ✅ Complete | [Status](docs/modules/fbgc-status.md) |
```

## Integration with Other Agents

### After Module Work
When a module agent (fbgc-team, markdown-team, etc.) completes work:
1. They update their `docs/modules/[module]-status.md`
2. You aggregate into `README-PROGRESS.md`
3. You handle the commit

### Coordination
- Reference other agents' status files
- Don't duplicate detailed information
- Link to module status for details
- Summarize at high level

## Documentation Requirements

**IMPORTANT**: After completing any PM work session, you MUST update `docs/modules/jenny-status.md` with:

```markdown
## Session Update

**Agent ID**: [Your agent ID from this session]
**Date**: [Current date]

### What We Did Today
- [Progress updates made]
- [Commits created]
- [Documentation updated]

### Current Status
- [Overall project health]
- [Modules needing attention]
- [Upcoming priorities]

### Files Modified
- [List of files updated]
```

---

## Quick Reference

### Common Tasks

**Update daily progress:**
```
> Ask Jenny to update today's progress
```

**Commit and push:**
```
> Have Jenny commit the current changes
```

**Status summary:**
```
> Ask Jenny for a project status summary
```

**Weekly report:**
```
> Have Jenny create a weekly progress report
```

---

Remember: You're here to help keep the project organized and well-documented. Always be thorough with commit messages, keep progress tracking current, and maintain clear communication with the user about all operations.
Troubleshooting Common Issues

“Context window full”

Symptom: Slow responses, degraded quality Solutions:

Run /context to visualize usage
Use /compact to compress history
Run /clear for fresh start
Move complex work to sub-agents
“Rate limit exceeded”

Symptom: Error messages about rate limits Solutions:

Check /usage for limits
Slow down requests
Use /cost to monitor token usage
Consider upgrading plan
“Agent not responding”

Symptom: Sub-agent seems stuck Solutions:

Check /bashes for hanging processes
Use Ctrl+C to cancel
Review agent permissions
Check agent configuration
“Permissions keep asking”

Symptom: Repeated permission prompts Solutions:

Add to allow array in settings
Use glob patterns for broader rules
Check .claude/settings.json hierarchy
Run /permissions to review
Pro Tips from the Trenches

After weeks of daily Claude Code usage, here are the insights that made the biggest difference:

1. Start every project with /init Setting up CLAUDE.md from the beginning saves countless correction cycles later.

2. Use sub-agents liberally Don’t be afraid to delegate. Sub-agents keep your main conversation focused and prevent context pollution.

3. The # prefix is your friend Quick memory adds while you work create a self-documenting system. Future you will thank present you.

4. Pre-approve common operations Configure your permissions to allow routine operations. Save your attention for the important decisions.

5. Export important conversations Use /export after solving tricky problems. These become searchable documentation and learning resources.

6. Name your sessions When starting important work, use descriptive prompts that make sessions easy to find later with claude -c.

7. Compress proactively Don’t wait for context issues. Run /compact periodically to keep performance optimal.

8. Background long operations Use Ctrl+B for tests, builds, or anything that takes more than a few seconds. Keep the conversation flowing.

9. Review the /todos Before ending a session, check /todos to ensure nothing gets lost.

10. Teach Claude incrementally Add to CLAUDE.md as you go, not all at once. Organic growth matches project evolution.

The Future of AI-Assisted Development

Claude Code represents a fundamental shift in how we think about developer tools. We’re moving from:

Code completion → Code conversation
Static analysis → Dynamic collaboration
Tool assistance → AI partnership
The developers who master conversational AI workflows today will be the senior engineers of tomorrow. Claude Code isn’t just a tool — it’s a new way of thinking about software development.

The commands and techniques in this guide are your foundation. From here, you’ll develop your own patterns, create custom agents that match your workflow, and discover optimizations I haven’t thought of yet.

That’s the beauty of a tool that adapts to you rather than forcing you to adapt to it.

Getting Help

When you get stuck:

Use /help for quick command reference
Run /doctor to check installation health
Use /bug to report issues with full context
Check GitHub issues: https://github.com/anthropics/claude-code/issues
Ask the claude-code-guide agent for documentation queries
Your Turn

Claude Code is waiting in your terminal. Whether you’re refactoring legacy code, debugging production issues, or building the next big thing, you now have a comprehensive command reference at your fingertips.

