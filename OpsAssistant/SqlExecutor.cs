using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace DiagnosticCliTool.Infrastructure
{
    public class SqlExecutor : ISqlExecutor
    {
        private readonly ILogger<SqlExecutor> _logger;

        public SqlExecutor(ILogger<SqlExecutor> logger)
        {
            _logger = logger;
        }

        public async Task<DataTable> ExecuteQuery(string connectionString, string query, CommandType commandType = CommandType.Text)
        {
            _logger.LogDebug("Executing SQL query: {Query}", query);
            var dataTable = new DataTable();

            try
            {
                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                await using var command = new SqlCommand(query, connection);
                command.CommandType = commandType;
                await using var adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                _logger.LogInformation("SQL query executed successfully. Rows returned: {RowCount}", dataTable.Rows.Count);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL query execution failed: {ErrorMessage}", ex.Message);
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database operation failed: {ErrorMessage}", ex.Message);
                throw;
            }
            return dataTable;
        }

        public async Task<int> ExecuteNonQuery(string connectionString, string command, CommandType commandType = CommandType.Text)
        {
            _logger.LogDebug("Executing SQL non-query command: {Command}", command);
            int rowsAffected = 0;
            try
            {
                await using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                await using var sqlCommand = new SqlCommand(command, connection);
                sqlCommand.CommandType = commandType;
                rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
                _logger.LogInformation("SQL non-query command executed successfully. Rows affected: {RowsAffected}", rowsAffected);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL non-query command execution failed: {ErrorMessage}", ex.Message);
                throw;
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database operation failed: {ErrorMessage}", ex.Message);
                throw;
            }
            return rowsAffected;
        }
    }
}
