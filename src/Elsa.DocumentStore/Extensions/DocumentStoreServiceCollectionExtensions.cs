
using Elsa.DocumentStore.Persistance;
using Elsa.DocumentStore.Persistance.SqlServer;
using Elsa.DocumentStore.Scripts;
using Elsa.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elsa.DocumentStore.Extensions
{
    public static class DocumentStoreServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentStorage(
            this IServiceCollection services            
            )            
        {
           
            services.AddNotificationHandlers(typeof(CustomJavascriptFunctions));

            

            return services;
        }

        public static IServiceCollection WithSqlServerDocumentStore(
            this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IDatabaseConnectionFactory>( provider => new SqlConnectionFactory(connectionString));
            services.AddTransient<IDocumentStore, SqlServerDocumentStore>();

            return services;
        }
    }
}
