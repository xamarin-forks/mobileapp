using System;
using System.Reactive;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Extensions;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant;
using Toggl.Ultrawave.Exceptions;

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

        public StateResult<ApiException> ErrorOccured { get; } = new StateResult<ApiException>();

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
                    : dataSource.UpdateWithConflictResolution(clean(entity)).SelectUnit())
                .Select(FinishedPersisting.Transition(fetch))
                .OnErrorReturnResult(ErrorOccured);
    }
}
