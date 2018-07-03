using System;
using System.Reactive;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Sync.States.Push
{
    internal sealed class DeleteLocalEntityState<TDatabaseModel, TThreadsafeModel, TDto> : ISyncState<TThreadsafeModel>
        where TDatabaseModel : class, IDatabaseModel
        where TThreadsafeModel : TDatabaseModel, IThreadSafeModel, IIdentifiable
    {
        private IDataSource<TThreadsafeModel, TDatabaseModel, TDto> dataSource { get; }

        public StateResult Deleted { get; } = new StateResult();

        public StateResult DeletingFailed { get; } = new StateResult();

        public DeleteLocalEntityState(IDataSource<TThreadsafeModel, TDatabaseModel, TDto> dataSource)
        {
            this.dataSource = dataSource;
        }

        public IObservable<ITransition> Start(TThreadsafeModel entity)
            => delete(entity)
                .Select(_ => Deleted.Transition())
                .Catch((Exception e) => Observable.Return(DeletingFailed.Transition()));

        private IObservable<Unit> delete(TThreadsafeModel entity)
            => entity == null
                ? Observable.Throw<Unit>(new ArgumentNullException(nameof(entity)))
                : dataSource.Delete(entity.Id);
    }
}
