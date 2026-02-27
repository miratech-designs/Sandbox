using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using DiagnosticCliTool.CliCommands;
using DiagnosticCliTool.Application;
using DiagnosticCliTool.Infrastructure;
using Elsa.Workflows.Core.Extensions;
using Elsa.Workflows.Management.Extensions;
using Elsa.Workflows.Runtime.Extensions;
using Serilog;

namespace DiagnosticCliTool
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/diagnostic-tool-.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting Diagnostic CLI Tool...");

                var host = Host.CreateDefaultBuilder(args)
                    .UseSerilog()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<ISqlExecutor, SqlExecutor>();
                        services.AddSingleton<IDiagnosticService, DiagnosticService>();

                        // Configure Elsa Workflows
                        services.AddElsa(elsa =>
                        {
                            // Register the workflow runtime.
                            elsa.AddWorkflowRuntime();

                            // Register the workflow management feature.
                            elsa.AddWorkflowManagement();

                            // Register custom activities if any
                                                        // elsa.AddActivitiesFrom<Program>();
                            elsa.AddActivitiesFrom<HostBuildDiagnosticWorkflow>();

                            // Register custom workflows
                                                        // elsa.AddWorkflow<MyCustomWorkflow>();
                            elsa.AddWorkflow<Workflows.HostBuildDiagnosticWorkflow>();
                        });
                    })
                    .Build();

                // Configure Spectre.Console CLI App
                var app = new CommandApp(new TypeRegistrar(host));

                app.Configure(config =>
                {
                    config.AddCommand<RunDiagnosticCommand>("run")
                        .WithDescription("Runs a diagnostic workflow.");
                    config.AddCommand<ListWorkflowsCommand>("list")
                        .WithDescription("Lists available diagnostic workflows.");

                    // Add more commands as needed

                    config.ValidateExamples();
                });

                Log.Information("CLI App configured. Executing command...");
                return await app.RunAsync(args);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }

    // Custom TypeRegistrar for Spectre.Console.Cli to integrate with Microsoft.Extensions.DependencyInjection
    public sealed class TypeRegistrar : ITypeRegistrar
    {
        private readonly IHost _host;

        public TypeRegistrar(IHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public ITypeResolver Build()
        {
            return new TypeResolver(_host);
        }

        public void Register(Type service, Type implementation)
        {
            _host.Services.As<IServiceCollection>().AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _host.Services.As<IServiceCollection>().AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            _host.Services.As<IServiceCollection>().AddSingleton(service, (provider) => factory());
        }
    }

    public sealed class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IHost _host;
        private readonly IServiceScope _scope;

        public TypeResolver(IHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _scope = _host.Services.CreateScope();
        }

        public object? Resolve(Type? type)
        {
            if (type == null)
            {
                return null;
            }
            return _scope.ServiceProvider.GetService(type);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
