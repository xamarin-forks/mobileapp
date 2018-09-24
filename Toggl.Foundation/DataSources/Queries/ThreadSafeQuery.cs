using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Queries;

namespace Toggl.Foundation.DataSources.Queries
{
    public sealed class ThreadSafeQuery<TThreadSafeModel, TDatabaseModel> : IQuery<IEnumerable<TThreadSafeModel>>
    {
        private readonly Func<TDatabaseModel, TThreadSafeModel> convert;

        private readonly IQuery<IEnumerable<TDatabaseModel>> query;

        public ThreadSafeQuery(IQuery<IEnumerable<TDatabaseModel>> query, Func<TDatabaseModel, TThreadSafeModel> convert)
        {
            Ensure.Argument.IsNotNull(query, nameof(query));
            Ensure.Argument.IsNotNull(convert, nameof(convert));

            this.query = query;
            this.convert = convert;
        }

        public IEnumerable<TThreadSafeModel> Execute()
            => query.Execute().Select(convert);
    }
}
