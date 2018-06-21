using System;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.States.Push.Interfaces;
using Toggl.Multivac;
using Toggl.PrimeRadiant;

namespace Toggl.Foundation.Sync.States.Push
{
    internal sealed class PushSingleState<T, TDto> : IPushState<T>
        where T : class, IThreadSafeModel, IDatabaseSyncable
    {
        private readonly ISingletonDataSource<T, TDto> dataSource;

        public StateResult<T> PushEntity { get; } = new StateResult<T>();

        public StateResult NothingToPush { get; } = new StateResult();

        public PushSingleState(ISingletonDataSource<T, TDto> dataSource)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
        
            this.dataSource = dataSource;
        }

        public IObservable<ITransition> Start() =>
            dataSource
                .Get()
                .Where(entity => entity.SyncStatus == SyncStatus.SyncNeeded)
                .SingleOrDefaultAsync()
                .Select(entity =>
                    entity != null
                        ? (ITransition)PushEntity.Transition(entity)
                        : NothingToPush.Transition());
    }
}
