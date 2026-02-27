using Elsa.Workflows.Core.Activities;
using Elsa.Workflows.Core.Contracts;
using Elsa.Workflows.Core.Models;
using DiagnosticCliTool.Infrastructure;
using DiagnosticCliTool.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DiagnosticCliTool.Workflows
{
    public class HostBuildDiagnosticWorkflow : WorkflowBase
    {
        protected override void Build(IWorkflowBuilder builder)
        {
            builder.Root = new Sequence
            {
                Activities =
                {
                    new Inline(async context =>
                    {
                        var logger = context.GetRequiredService<ILogger<HostBuildDiagnosticWorkflow>>();
                        var sqlExecutor = context.GetRequiredService<ISqlExecutor>();
                        var connectionString = context.GetInput<string>("ConnectionString") ?? "Data Source=.;Initial Catalog=master;Integrated Security=True;TrustServerCertificate=True";
                        var hostName = context.GetInput<string>("HostName") ?? "DefaultHost";

                        logger.LogInformation("Starting diagnostic for host: {HostName}", hostName);

                        var results = new List<DiagnosticResult>();

                        // Step 1: Check if host record exists
                        logger.LogInformation("Executing SQL Query: CheckHostRecord");
                        var hostRecordQuery = $"SELECT COUNT(*) FROM HostTable WHERE HostName = '{hostName}'";
                        var hostRecordTable = await sqlExecutor.ExecuteQuery(connectionString, hostRecordQuery);
                        var hostExists = (int)hostRecordTable.Rows[0][0] > 0;
                        results.Add(new DiagnosticResult { StepName = "CheckHostRecord", Status = hostExists ? "Pass" : "Fail", Message = hostExists ? "Host record found." : "Host record NOT found." });

                        // Step 2: Check build status
                        if (hostExists)
                        {
                            logger.LogInformation("Executing SQL Query: CheckBuildStatus");
                            var buildStatusQuery = $"SELECT BuildStatus FROM HostBuilds WHERE HostName = '{hostName}' ORDER BY BuildDate DESC";
                            var buildStatusTable = await sqlExecutor.ExecuteQuery(connectionString, buildStatusQuery);
                            var buildStatus = buildStatusTable.Rows.Count > 0 ? buildStatusTable.Rows[0][0].ToString() : "NotFound";
                            var buildPassed = buildStatus == "Success";
                            results.Add(new DiagnosticResult { StepName = "CheckBuildStatus", Status = buildPassed ? "Pass" : "Fail", Message = $"Latest build status: {buildStatus}" });

                            if (!buildPassed)
                            {
                                logger.LogWarning("Build failed for host {HostName}. Attempting remediation.", hostName);
                                // Simulate remediation: Update build status to 'Retrying'
                                var updateStatusCmd = $"UPDATE HostBuilds SET BuildStatus = 'Retrying' WHERE HostName = '{hostName}' AND BuildStatus = 'Failed'";
                                var rowsAffected = await sqlExecutor.ExecuteNonQuery(connectionString, updateStatusCmd);
                                results.Add(new DiagnosticResult { StepName = "RemediateBuildStatus", Status = rowsAffected > 0 ? "Success" : "NoAction", Message = rowsAffected > 0 ? "Updated failed build status to Retrying." : "No failed builds to retry." });
                            }
                        }

                        context.SetOutput("DiagnosticResults", results);
                    })
                }
            };
        }
    }
}
