using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiagnosticCliTool.Domain.Models;
using Elsa.Workflows.Core.Models;
using Elsa.Workflows.Core.Services;
using Elsa.Workflows.Management.Contracts;
using Elsa.Workflows.Runtime.Contracts;
using Microsoft.Extensions.Logging;

namespace DiagnosticCliTool.Application
{
    public class DiagnosticService : IDiagnosticService
    {
        private readonly IWorkflowRuntime _workflowRuntime;
        private readonly IWorkflowDefinitionStore _workflowDefinitionStore;
        private readonly ILogger<DiagnosticService> _logger;

        public DiagnosticService(
            IWorkflowRuntime workflowRuntime,
            IWorkflowDefinitionStore workflowDefinitionStore,
            ILogger<DiagnosticService> logger)
        {
            _workflowRuntime = workflowRuntime;
            _workflowDefinitionStore = workflowDefinitionStore;
            _logger = logger;
        }

        public async Task<IEnumerable<DiagnosticResult>> RunDiagnosticWorkflow(string workflowName, Dictionary<string, object> inputs)
        {
            _logger.LogInformation("Attempting to run workflow: {WorkflowName}", workflowName);

            var workflowDefinition = await _workflowDefinitionStore.FindByDefinitionIdAsync(workflowName, VersionOptions.Published);

            if (workflowDefinition == null)
            {
                _logger.LogWarning("Workflow definition '{WorkflowName}' not found.", workflowName);
                return Enumerable.Empty<DiagnosticResult>();
            }

            var startWorkflowRequest = new StartWorkflowRequest(workflowDefinition.Id, Input: inputs);
            var result = await _workflowRuntime.StartWorkflowAsync(startWorkflowRequest);

            if (result == null || !result.Any())
            {
                _logger.LogWarning("Workflow '{WorkflowName}' started but returned no results.", workflowName);
                return Enumerable.Empty<DiagnosticResult>();
            }

            // In a real scenario, you would extract specific outputs from the workflow execution result.
            // For this example, we'll just return a placeholder result.
            _logger.LogInformation("Workflow '{WorkflowName}' executed successfully.", workflowName);
            return new List<DiagnosticResult>
            {
                new DiagnosticResult { StepName = "Workflow Execution", Status = "Completed", Message = $"Workflow {workflowName} finished." }
            };
        }

        public async Task<IEnumerable<string>> GetAvailableWorkflows()
        {
            _logger.LogInformation("Retrieving available workflow definitions.");
            var definitions = await _workflowDefinitionStore.FindManyAsync(new WorkflowDefinitionFilter { IsLatestOrPublished = true });
            return definitions.Select(d => d.DefinitionId).Distinct();
        }
    }
}
