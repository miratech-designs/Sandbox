using System.Collections.Generic;
using System.Threading.Tasks;
using DiagnosticCliTool.Domain.Models;

namespace DiagnosticCliTool.Application
{
    public interface IDiagnosticService
    {
        Task<IEnumerable<DiagnosticResult>> RunDiagnosticWorkflow(string workflowName, Dictionary<string, object> inputs);
        Task<IEnumerable<string>> GetAvailableWorkflows();
    }
}
