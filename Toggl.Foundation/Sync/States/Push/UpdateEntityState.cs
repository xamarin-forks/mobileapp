using System;
using System.Reactive.Linq;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Extensions;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.PrimeRadiant;
using Toggl.Ultrawave.ApiClients;
using static Toggl.Foundation.Sync.PushSyncOperation;

namespace Toggl.Foundation.Sync.States.Push
{
    internal sealed class UpdateEntityState<TModel, TThreadsafeModel, TDto>
        : BasePushEntityState<TThreadsafeModel, TDto>
        where TThreadsafeModel : class, TModel, IDatabaseSyncable, IThreadSafeModel
    {
        private readonly IUpdatingApiClient<TModel> api;

        private readonly IBaseDataSource<TThreadsafeModel, TDto> dataSource;

        private readonly Func<TModel, TDto> clean;

        public StateResult<TThreadsafeModel> EntityChanged { get; } = new StateResult<TThreadsafeModel>();

        public StateResult<TThreadsafeModel> UpdatingSucceeded { get; } = new StateResult<TThreadsafeModel>();

        public UpdateEntityState(
            IUpdatingApiClient<TModel> api,
            IBaseDataSource<TThreadsafeModel, TDto> dataSource,
            IAnalyticsService analyticsService,
            Func<TModel, TDto> clean)
            : base(dataSource, analyticsService)
        {
            Ensure.Argument.IsNotNull(api, nameof(api));
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(clean, nameof(clean));

            this.api = api;
            this.dataSource = dataSource;
            this.clean = clean;
        }

        public override IObservable<ITransition> Start(TThreadsafeModel entity)
            => update(entity)
                .SelectMany(tryOverwrite(entity))
                .SelectMany(result => result is IgnoreResult<TThreadsafeModel>
                    ? entityChanged(entity)
                    : succeeded(extractFrom(result)))
                .Track(AnalyticsService.EntitySynced, Update, entity.GetSafeTypeName())
                .Track(AnalyticsService.EntitySyncStatus, entity.GetSafeTypeName(), $"{Update}:{Resources.Success}")
                .Catch(Fail(entity, Update));

        private IObservable<TModel> update(TModel entity)
            => entity == null
                ? Observable.Throw<TModel>(new ArgumentNullException(nameof(entity)))
                : api.Update(entity);

        private IObservable<ITransition> entityChanged(TThreadsafeModel entity)
            => Observable.Return(EntityChanged.Transition(entity));

        private Func<TModel, IObservable<IConflictResolutionResult<TThreadsafeModel>>> tryOverwrite(TThreadsafeModel entity)
            => updatedEntity => dataSource.OverwriteIfOriginalDidNotChange(entity, clean(updatedEntity));

        private TThreadsafeModel extractFrom(IConflictResolutionResult<TThreadsafeModel> result)
        {
            switch (result)
            {
                case CreateResult<TThreadsafeModel> c:
                    return c.Entity;
                case UpdateResult<TThreadsafeModel> u:
                    return u.Entity;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        private IObservable<ITransition> succeeded(TThreadsafeModel entity)
            => Observable.Return((ITransition)UpdatingSucceeded.Transition(entity));
    }
}
