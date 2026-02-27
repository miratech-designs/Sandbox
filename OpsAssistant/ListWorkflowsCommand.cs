using System.Threading.Tasks;
using DiagnosticCliTool.Application;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DiagnosticCliTool.CliCommands
{
    public sealed class ListWorkflowsCommand : AsyncCommand
    {
        private readonly IDiagnosticService _diagnosticService;

        public ListWorkflowsCommand(IDiagnosticService diagnosticService)
        {
            _diagnosticService = diagnosticService;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            AnsiConsole.MarkupLine("[green]Fetching available diagnostic workflows...[/]");

            var workflows = await _diagnosticService.GetAvailableWorkflows();

            if (workflows != null && System.Linq.Enumerable.Any(workflows))
            {
                var table = new Table();
                table.AddColumn("[bold blue]Workflow Name[/]");

                foreach (var workflow in workflows)
                {
                    table.AddRow(workflow);
                }
                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No diagnostic workflows found.[/]");
            }

            return 0;
        }
    }
}
