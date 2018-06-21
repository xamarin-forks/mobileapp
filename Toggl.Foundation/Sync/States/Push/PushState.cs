using System;
using System.Linq;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.States.Push.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Sync.States.Push
{
    internal sealed class PushState<TDatabaseModel, TThreadsafeModel, TDto> : IPushState<TThreadsafeModel>
        where TDatabaseModel : IDatabaseModel, IDatabaseSyncable
        where TThreadsafeModel : class, TDatabaseModel, ILastChangedDatable, IThreadSafeModel
    {
        private readonly IDataSource<TThreadsafeModel, TDatabaseModel, TDto> dataSource;

        public StateResult<TThreadsafeModel> PushEntity { get; } = new StateResult<TThreadsafeModel>();

        public StateResult NothingToPush { get; } = new StateResult();

        public PushState(IDataSource<TThreadsafeModel, TDatabaseModel, TDto> dataSource)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
        
            this.dataSource = dataSource;
        }

        public IObservable<ITransition> Start() =>
            getOldestUnsynced()
                .SingleAsync()
                .Select(entity =>
                    entity != null
                        ? (ITransition)PushEntity.Transition(entity)
                        : NothingToPush.Transition());

        private IObservable<TThreadsafeModel> getOldestUnsynced()
            => dataSource
                .GetAll(entity => entity.SyncStatus == SyncStatus.SyncNeeded)
                .SingleAsync()
                .Select(entities => entities
                    .OrderBy(entity => entity.At)
                    .FirstOrDefault());
    }
}
