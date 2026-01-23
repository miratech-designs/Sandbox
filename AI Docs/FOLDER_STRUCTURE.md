# GitHub Standards & Folder Structure

Recommended `.github` layout and files for enterprise Copilot workflows and templates.

.github/
- workflows/
  - build.yml (CI build for templates and samples)
  - lint.yml (style checks)
  - pack.yml (packaging of templates)
- ISSUE_TEMPLATE/
  - bug_report.md
  - feature_request.md
- PULL_REQUEST_TEMPLATE.md
- FUNDING.yml (optional)

Repository top-level layout
- /src
  - /WorkerTemplate
    - WorkerTemplate.sln
    - src/ (C# projects)
- /samples
  - /ScalingController
  - /HealthUI
- /docs
  - ARCHITECTURE.md
  - SPECIFICATIONS.md
  - COPILOT_INSTRUCTIONS.md
- /templates
  - worker-template.zip
- README.md
- LICENSE

## Example workflow: `workflows/build.yml`
- Checkout, restore, build WorkerTemplate solution, run unit tests.
- Linting for markdown and code style.

## Best Practices
- Keep secrets out of repo; use local credential store for CI runners.
- Provide template generation scripts to scaffold new workers using the prompt templates.

(Place these files under `.github` and keep a small set of curated templates to ensure consistency.)
