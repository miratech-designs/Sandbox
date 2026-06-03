---
name: migration-gen
description: Generate database migrations. Use when adding tables or changing schema.
model: claude-sonnet-4-5-20250929
tools:
  - Read
  - Write
  - Glob
  - Bash
---
1. Read existing migrations to understand the tool, naming convention, and file structure.
2. Read the current schema file.
3. Create the migration file following project conventions.

Rules:
* Always include both UP and DOWN migrations.
* Add indexes for foreign keys and frequently queried columns.
* Never add NOT NULL without a DEFAULT value.
After creating, run the migration locally and verify it applies cleanly.