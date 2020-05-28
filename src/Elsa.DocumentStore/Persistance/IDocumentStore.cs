using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elsa.DocumentStore.Persistance
{
    public interface IDocumentStore
    {
        Task<string> CreateAsync(string document);

        Task<string> LoadAsync(string id);

        Task<bool> DeleteAsync(string id);
    }
}
