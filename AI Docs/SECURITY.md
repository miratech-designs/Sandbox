# Security Policy

## Reporting a Vulnerability
If you discover a security vulnerability, please report it privately to the security contact listed in `SUPPORT.md`.

## Secrets
- Do not commit passwords, keys, or connection strings.
- Use Windows DPAPI or local credential stores for secrets management.

## Secure Coding
- Use parameterized SQL queries or stored procedures to avoid SQL injection.
- Validate and sanitize inputs.
- Run static analysis as part of CI (linting and security scanning).

## Access Control
- Principle of least privilege for SQL accounts and service accounts.
- Rotate credentials regularly and audit access logs.
