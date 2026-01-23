# ROLE DEFINITION
You are a solution architect designing scalable, maintainable enterprise systems using .NET Core and SQL Server

# CONTEXT

## BACKGROUND
Team needs to standardize console application templates for workflows.

## CURRENT STATE
- multiple console applications with inconsistent patterns
- manual or no scaling processes
- limited or no observability and metrics
- no standard error handling approach

## CONSTRAINTS
- must run on Windows Server 2019+
- maximum memory footprint 2 GB per instance
- must support queue-based processing (SQL Server tables as queues)
- requires metrics integration

# TASK

## OBJECTIVE
Provide a set of templates and sample prompt, instruction, specification markdown files to be used with Copilot by an enterprise team.
Provide typical files that are standard under the .github folder and provide folder structure and hierarchy.

## REQUIREMENTS
1. must use local resources/services with no external/cloud dependencies
2. support sql server
3. consoles must implement graceful shutdown
4. include health check endpoints
5. provide metrics
6. support dynamic scaling based on load/demand
7. include comprehensive logging
8. handle transient failures with retry policies

# FORMAT

## OUTPUT STRUCTURE
1. high-level architecture diagram (mermaid)
2. component descriptions
3. key classes and interfaces
4. configuration approach
5. sample implementation from components

## COMPLETE DELIVERABLES

### Architecture 8. Design (ARCHITECTURE.md)
- 3 comprehensive Mermaid diagrams (architecture, components, scaling)
- Detailed component descriptions
- Key classes and interfaces
- Configuration approach
- Performance targets

### Developer Guidelines (COPILOT_INSTRUCTIONS.md)
- Code standards and patterns
- Required implementations
- SQL Server patterns
- Testing and security guidelines

### Reusable Templates (PROMPT_TEMPLATES.md)
- 10 production-ready prompt templates
- Templates for every major feature
- Complete console worker generation

### Technical Specs (SPECIFICATIONS.md)
- System and performance requirements
- Complete database schemas
- Configuration schemas
- Monitoring specifications

### GitHub Standards (FOLDER STRUCTURE.md)
- Complete github folder hierarchy
- Workflows, templates, and scripts
- Best practices guide

### Support Documentation
- Contributiing.md - contribution guide
- Security.md - Security policy
- Support.md - support resources

### Implementaion Resources
- Sample application README
- Production ready SQL schema
- Complete stored procedures

All files are to be enterprise-ready.
