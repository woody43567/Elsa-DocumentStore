using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Elsa.DocumentStore.Persistance.SqlServer
{
    public class SqlServerDocumentStore : IDocumentStore
    {
        public SqlServerDocumentStore(IDatabaseConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public IDatabaseConnectionFactory ConnectionFactory { get; }

        public async Task<string> CreateAsync(string document)
        {
            using (var conn = await ConnectionFactory.CreateConnectionAsync().ConfigureAwait(false))
            {
                var id = Guid.NewGuid();


                _ = await conn.ExecuteAsync("INSERT INTO ElsaDocuments(Id,Document) VALUES(@id,@document)",
                    param: new
                    {
                        id,
                        document
                    }).ConfigureAwait(false);

                return id.ToString();
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (Guid.TryParse(id, out var documentId) == false)
            {
                throw new ArgumentException("Id must be a guid", nameof(id));
            }

            using (var conn = await ConnectionFactory.CreateConnectionAsync().ConfigureAwait(false))
            {


                _ = await conn.ExecuteAsync("DELETE FROM ElsaDocuments WGERE Id = @id",
                    param: new
                    {
                        id = documentId
                    }).ConfigureAwait(false);
            }

            return true;
        }

        public async Task<string> LoadAsync(string id)
        {
            if (Guid.TryParse(id, out var documentId) == false)
            {
                throw new ArgumentException("Id must be a guid", nameof(id));
            }

            using (var conn = await ConnectionFactory.CreateConnectionAsync().ConfigureAwait(false))
            {

                return await conn.QueryFirstAsync<string>("SELECT Document FROM ElsaDocuments WHERE Id =@id",
                    param: new
                    {
                        id = documentId

                    }).ConfigureAwait(false);

            }
        }
    }
}
