using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit.Subscriptions.Coordinator;
using Raven.Client;
using Raven.Client.Document;

namespace Storage
{
    public class RavenSubscriptionStorage : SubscriptionStorage
    {
        private static readonly IDocumentStore InternalDocumentStore;

        static RavenSubscriptionStorage()
        {
            InternalDocumentStore = new DocumentStore {DefaultDatabase = "Subscriptions", Url = "http://POHVTRDB01:8090/"};
            InternalDocumentStore.Initialize();
        }

        public void Dispose()
        {
        }

        public void Add(PersistentSubscription subscription)
        {
            using (var session = InternalDocumentStore.OpenSession())
            {
                session.Store(subscription);
                session.SaveChanges();
            }
        }

        public void Remove(PersistentSubscription subscription)
        {
            using (var session = InternalDocumentStore.OpenSession())
            {
                session.Delete(subscription);
                session.SaveChanges();
            }
        }

        public IEnumerable<PersistentSubscription> Load(Uri busUri)
        {
            using (var session = InternalDocumentStore.OpenSession())
            {
                var subscription = session.Query<PersistentSubscription>().Where(x => x.BusUri == busUri).ToList();
                return subscription;
            }
        }
    }
}
