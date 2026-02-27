using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DiagnosticCliTool.Application;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DiagnosticCliTool.CliCommands
{
    public sealed class RunDiagnosticCommand : AsyncCommand<RunDiagnosticCommand.Settings>
    {
        private readonly IDiagnosticService _diagnosticService;

        public RunDiagnosticCommand(IDiagnosticService diagnosticService)
        {
            _diagnosticService = diagnosticService;
        }

        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "<WORKFLOW_NAME>")]
            [Description("The name of the diagnostic workflow to run.")]
            public string WorkflowName { get; set; } = string.Empty;

            [CommandOption("-i|--input <KEY=VALUE>")]
            [Description("Input parameters for the workflow (e.g., HostName=MyServer,InstanceId=123).")]
            public string[]? InputParameters { get; set; }

            public override ValidationResult Validate()
            {
                if (string.IsNullOrWhiteSpace(WorkflowName))
                {
                    return ValidationResult.Error("Workflow name cannot be empty.");
                }
                return ValidationResult.Success();
            }
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            AnsiConsole.MarkupLine($"[green]Running diagnostic workflow: {settings.WorkflowName}[/]");

            var inputs = new Dictionary<string, object>();
            if (settings.InputParameters != null)
            {
                foreach (var param in settings.InputParameters)
                {
                    var parts = param.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        inputs[parts[0]] = parts[1];
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[yellow]Warning: Invalid input parameter format: {param}. Skipping.[/]");
                    }
                }
            }

            var results = await _diagnosticService.RunDiagnosticWorkflow(settings.WorkflowName, inputs);

            if (results != null)
            {
                var table = new Table();
                table.AddColumn("[bold blue]Step Name[/]");
                table.AddColumn("[bold blue]Status[/]");
                table.AddColumn("[bold blue]Message[/]");

                foreach (var result in results)
                {
                    table.AddRow(result.StepName, result.Status, result.Message);
                }
                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No results returned from the workflow.[/]");
            }

            return 0;
        }
    }
}
