using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;
using Raven.Client.Document;

namespace ExpenseTracker.App_Start.Modules
{
    public class RavenModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentStore>().ToMethod(ctx =>
            {
                var documentStore = new DocumentStore { ConnectionStringName = "RavenDB" };
                return documentStore.Initialize();
            }).InSingletonScope();

            Bind<IDocumentSession>().ToMethod(ctx => ctx.Kernel.Get<IDocumentStore>().OpenSession()).InRequestScope();
        }
    }
}