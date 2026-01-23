# Support & Runbook

## Support Contacts
- Primary: on-prem infra team (pager/ops email) — replace with local contact.
- Secondary: application owner.

## Runbook (quick actions)
- To check queue depth: run a SQL query `SELECT COUNT(1) FROM dbo.Queue WHERE Status=0 AND VisibleAt <= SYSUTCDATETIME();`.
- To restart a worker process: use `sc stop <service>` / `sc start <service>` or restart the process via Task Manager.
- To inspect logs: check the configured Serilog log files under the install directory.

## Vulnerability & Incident Reporting
Report incidents to the contacts above; include timestamps, correlation IDs, and log extracts.

## SLA & Escalation
Define local SLA and escalation paths in team operational docs.
