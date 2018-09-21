using System;
using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Models;
using Toggl.PrimeRadiant.Queries;

namespace Toggl.PrimeRadiant.Realm.Queries
{
    public sealed class QueryFactory : IQueryFactory
    {
        private readonly Func<Realms.Realm> getRealmInstance;

        public QueryFactory(Func<Realms.Realm> getRealmInstance)
        {
            Ensure.Argument.IsNotNull(getRealmInstance, nameof(getRealmInstance));

            this.getRealmInstance = getRealmInstance;
        }

        public IQuery<IDatabaseProject> GetAllProjectsContaining(IEnumerable<string> words)
            => new GetAllProjectsContainingQuery(getRealmInstance, words);
    }
}
