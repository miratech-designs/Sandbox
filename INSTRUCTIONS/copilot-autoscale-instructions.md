🚀 1. Requirements: Autoscaling Worker Pool

Build a complete worker-pool system using:
	•	System.Threading.Channels
	•	Dynamic autoscaling (scale-out + scale-in)
	•	Configurable:
	•	MinWorkers
	•	MaxWorkers
	•	BacklogPerWorkerScaleOut
	•	Idle timeout before scaling-in
	•	Thread-safe worker lifecycle
	•	Graceful pool shutdown
	•	CancellationToken propagation

⸻

⚡ 2. Scheduling Features

Priority Queues
	•	High
	•	Normal
	•	Low
	•	Independent Channel<ScheduledWorkItem> for each priority

Fair-Share Scheduling

Weighted selection based on configurable dictionary:
  •	PriorityLevel.High = 8
  •	PriorityLevel.Normal = 3
  •	PriorityLevel.Low = 1

Aging
	•	Each item tracks enqueue timestamp
	•	Effective priority increases linearly: effective = baseWeight + (ageSeconds * AgingFactor)
  •	Prevents starvation

⸻

📊 3. Observability

Include:
	•	IWorkerPoolMetrics
	•	ConsoleWorkerPoolMetrics (default)
	•	Hooks for:
	•	worker created/destroyed
	•	task queued/dequeued
	•	task duration
	•	backlog snapshots
	•	scale events
	•	Fully thread-safe

⸻

📁 4. Project Structure

Generate full source files (one class per file) inside:
src/AutoScaling.WorkerPool/src/

Files required:
WorkerPoolConfig.cs
WorkPriority.cs
ScheduledWorkItem.cs
IWorkerPoolMetrics.cs
ConsoleWorkerPoolMetrics.cs
IWorkerPool.cs
PriorityAutoScalingWorkerPool.cs
Worker.cs
WorkerPoolExtensions.cs   (ASP.NET DI)

All classes must contain complete compilable code—no placeholders.

⸻

🧪 5. Example Applications

Generate two runnable examples:

A. Console App Example (Program.cs)
	•	Instantiate worker pool
	•	Enqueue 50 mixed-priority tasks
	•	Show autoscaling behavior
	•	Graceful shutdown

B. ASP.NET Core Example (Program.cs)

Minimal API with endpoints:
POST /enqueue/{priority}
GET /status

Uses DI extension:
services.AddAutoScalingWorkerPool(...)
📦 6. NuGet Packaging Support

Generate all packaging files:

✔ Directory.Build.props

Include:
	•	VersionPrefix
	•	Authors
	•	Company
	•	PackageLicenseExpression
	•	PackageProjectUrl
	•	RepositoryUrl
	•	RepositoryType
	•	PackageReadmeFile
	•	PackageTags
	•	GenerateDocumentationFile

✔ Full .csproj with NuGet metadata for:
  •	AutoScaling.WorkerPool
  •	AutoScaling.WorkerPool.ConsoleExample
  •	AutoScaling.WorkerPool.AspNetExample

Including:
	•	Multi-targeting: net8.0;netstandard2.1
	•	Proper XML metadata for NuGet
	•	Icon placeholder
	•	README.md packaging
	•	SourceLink support
	•	Deterministic builds
	•	XML documentation generation

✔ Generate a nuget.config (optional)

Tell NuGet to output packages to /artifacts.

✔ Provide CLI instructions:
	•	Pack: dotnet pack -c Release
  •	Publish: dotnet nuget push ./artifacts/*.nupkg --api-key <KEY> --source https://api.nuget.org/v3/index.json


📖 7. GitHub-Ready README.md

Generate a fully polished README containing:

Sections
	1.	Overview
	2.	Features
	3.	Installation (NuGet)
	4.	Quick Start
	5.	Console Example
	6.	ASP.NET Example
	7.	Architecture Overview
	8.	Priority Queues
	9.	Fair-Share Scheduling
	10.	Aging Explained
	11.	Autoscaling Algorithm
	12.	Observability & Metrics
	13.	ASCII Diagrams
	14.	NuGet Packaging Usage
	15.	Contributing
	16.	License placeholder

Make it high quality and professional.

⸻

📐 8. Architecture Diagrams

Include ASCII diagrams for:
	•	Queue architecture
	•	Worker scheduling flow
	•	Autoscaling decision tree
	•	Aging growth curve
	•	Priority queue selection pseudo-math

⸻

📌 9. Formatting Rules
	•	Provide all source files in full
	•	Everything must compile
	•	No pseudo-code
	•	Use consistent namespaces
	•	Use cancellation tokens correctly
	•	Do not abbreviate class names
	•	No “…” or partial code
	•	All examples must be drop-in ready