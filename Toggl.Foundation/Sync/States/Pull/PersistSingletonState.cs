using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.PrimeRadiant;

namespace Toggl.Foundation.Sync.States.Pull
{
    internal sealed class PersistSingletonState<TInterface, TDatabaseInterface, TThreadsafeInterface, TDto>
        : IPersistState
        where TDatabaseInterface : TInterface, IDatabaseSyncable
        where TThreadsafeInterface : class, TDatabaseInterface, IThreadSafeModel
    {
        private readonly ISingletonDataSource<TThreadsafeInterface, TDto> dataSource;

        private readonly Func<TInterface, TDto> clean;

        public StateResult<IFetchObservables> FinishedPersisting { get; } = new StateResult<IFetchObservables>();

        public PersistSingletonState(
            ISingletonDataSource<TThreadsafeInterface, TDto> dataSource,
            Func<TInterface, TDto> clean)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(clean, nameof(clean));

            this.dataSource = dataSource;
            this.clean = clean;
        }

        public IObservable<ITransition> Start(IFetchObservables fetch)
            => fetch.GetSingle<TInterface>()
                .SingleAsync()
                .SelectMany(entity => entity == null
                    ? Observable.Return(Unit.Default)
                    : dataSource.UpdateWithConflictResolution(clean(entity)).Select(_ => Unit.Default))
                .Select(_ => FinishedPersisting.Transition(fetch));
    }
}
