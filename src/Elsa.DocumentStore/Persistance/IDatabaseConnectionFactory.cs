using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.DocumentStore.Persistance
{
    public interface IDatabaseConnectionFactory
    {

        Task<DbConnection> CreateConnectionAsync();
    }
}
