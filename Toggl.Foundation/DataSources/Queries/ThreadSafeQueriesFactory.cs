using System.Collections.Generic;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Models;
using Toggl.PrimeRadiant.Queries;

namespace Toggl.Foundation.DataSources.Queries
{
    public sealed class ThreadSafeQueriesFactory : IThreadSafeQueryFactory
    {
        private readonly IQueryFactory queryFactory;

        public ThreadSafeQueriesFactory(IQueryFactory queryFactory)
        {
            Ensure.Argument.IsNotNull(queryFactory, nameof(queryFactory));

            this.queryFactory = queryFactory;
        }
    }
}
