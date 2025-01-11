using System.Data.SqlClient;

namespace DB_Project.Models
{
    public class SqlDatabaseExecutor : IDatabaseExecutor
    {
        private readonly SqlConnection _connection;

        public SqlDatabaseExecutor(SqlConnection connection)
        {
            _connection = connection;
        }

        public void ExecuteQuery(string query)
        {
            try
            {
                _connection.Open();
                SqlCommand cmd = new SqlCommand(query, _connection);
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
