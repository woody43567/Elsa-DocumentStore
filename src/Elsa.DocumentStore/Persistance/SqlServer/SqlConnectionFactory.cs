using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.DocumentStore.Persistance.SqlServer
{
    public class SqlConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString) => _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        public async Task<DbConnection> CreateConnectionAsync()
        {
            try
            {
                var sqlConnection = new SqlConnection(_connectionString);
                await sqlConnection.OpenAsync().ConfigureAwait(false);
                return sqlConnection;
            }
            catch
            {
                throw;
            }
        }
    }
}
