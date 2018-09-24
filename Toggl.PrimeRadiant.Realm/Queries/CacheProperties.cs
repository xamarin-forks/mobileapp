using System;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Queries;

namespace Toggl.PrimeRadiant.Realm.Queries
{
    public sealed class CacheProperties : IQuery
    {
        private readonly Func<Realms.Realm> getRealmInstance;

        public CacheProperties(Func<Realms.Realm> getRealmInstance)
        {
            Ensure.Argument.IsNotNull(getRealmInstance, nameof(getRealmInstance));

            this.getRealmInstance = getRealmInstance;
        }

        public void Execute()
        {
            var realm = getRealmInstance();

            var transaction = realm.BeginWrite();
            var projects = realm.All<RealmProject>();
            foreach (var project in projects)
            {
                project.ClientName = project.Client?.Name;
            }

            transaction.Commit();
        }
    }
}
