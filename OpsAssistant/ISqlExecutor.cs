using System.Data;
using System.Threading.Tasks;

namespace DiagnosticCliTool.Infrastructure
{
    public interface ISqlExecutor
    {
        Task<DataTable> ExecuteQuery(string connectionString, string query, CommandType commandType = CommandType.Text);
        Task<int> ExecuteNonQuery(string connectionString, string command, CommandType commandType = CommandType.Text);
    }
}
