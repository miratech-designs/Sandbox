namespace DiagnosticCliTool.Domain.Models
{
    public class DiagnosticResult
    {
        public string StepName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        // Add more properties as needed, e.g., Severity, Timestamp, RawOutput
    }
}
