using Elsa.DocumentStore.Persistance;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;
using Newtonsoft.Json;
using Nito.AsyncEx.Synchronous;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Elsa.DocumentStore.Scripts
{
    public class CustomJavascriptFunctions : INotificationHandler<EvaluatingJavaScriptExpression>
    {
        public IDocumentStore DocumentStore { get; }

        public CustomJavascriptFunctions(IDocumentStore documentStore)
        {
            DocumentStore = documentStore;
        }

        public Task Handle(EvaluatingJavaScriptExpression notification, CancellationToken cancellationToken)
        {
            var engine = notification.Engine;
            var workflowExecutionContext = notification.WorkflowExecutionContext;

            engine.SetValue(
                "loadDocument",
                (Func<string, object>)(id => LoadDocument(id))
            );

            engine.SetValue(
                "createDocument",
                (Func<object, string>)(d => CreateDocument(d))
            );

            engine.SetValue(
                "deleteDocument",
                (Func<string,bool>)(id => DeleteDocument(id))
            );


            return Task.CompletedTask;
        }

        private object LoadDocument(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id must be specified.", nameof(id));
            }

            var docJson = this.DocumentStore.LoadAsync(id).WaitAndUnwrapException();

            // Check if the document was null. If so return otherwise convert it back to a string.
            if (docJson == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject(docJson);

        }

        private string CreateDocument(object o)
        {
            return this.DocumentStore.CreateAsync(JsonConvert.SerializeObject(o)).WaitAndUnwrapException();

        }

        private bool DeleteDocument(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Id must be specified.", nameof(id));
            }

            this.DocumentStore.DeleteAsync(id).WaitAndUnwrapException();

            return true;
        }
    }
}
