using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Extensions;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public abstract class SingletonDataSource<TThreadsafe, TDatabase, TDto>
        : BaseDataSource<TThreadsafe, TDatabase, TDto>,
          ISingletonDataSource<TThreadsafe, TDto>
        where TDto : IIdentifiable
        where TDatabase : IDatabaseModel
        where TThreadsafe : IThreadSafeModel, IIdentifiable, TDatabase
    {
        private readonly ISingleObjectStorage<TDatabase, TDto> storage;

        private readonly ISubject<TThreadsafe> currentSubject;

        public IObservable<TThreadsafe> Current { get; }

        protected SingletonDataSource(ISingleObjectStorage<TDatabase, TDto> storage, TThreadsafe defaultCurrentValue)
            : base(storage)
        {
            Ensure.Argument.IsNotNull(storage, nameof(storage));

            this.storage = storage;

            currentSubject = new Subject<TThreadsafe>();

            var initialValueObservable = storage.Single()
                .Select(Convert)
                .Catch((Exception _) => Observable.Return(defaultCurrentValue))
                .FirstAsync();

            var connectableCurrent = initialValueObservable.Concat(currentSubject).Replay(1);
            connectableCurrent.Connect();

            Current = connectableCurrent;
        }

        public virtual IObservable<TThreadsafe> Get()
            => storage.Single().Select(Convert);

        public override IObservable<TThreadsafe> Create(TDto entity)
            => base.Create(entity).Do(currentSubject.OnNext);

        public override IObservable<TThreadsafe> Update(TDto entity)
            => base.Update(entity).Do(currentSubject.OnNext);

        public override IObservable<TThreadsafe> Overwrite(TThreadsafe original, TDto entity)
            => base.Overwrite(original, entity).Do(currentSubject.OnNext);

        public override IObservable<IConflictResolutionResult<TThreadsafe>> OverwriteIfOriginalDidNotChange(
            TThreadsafe original, TDto entity)
            => base.OverwriteIfOriginalDidNotChange(original, entity).Do(handleConflictResolutionResult);

        public virtual IObservable<IConflictResolutionResult<TThreadsafe>> UpdateWithConflictResolution(
            TDto entity)
            => Repository.UpdateWithConflictResolution(entity.Id, entity, ResolveConflicts, RivalsResolver)
                .Select(result => result.ToThreadSafeResult(Convert))
                .Do(handleConflictResolutionResult);

        private void handleConflictResolutionResult(IConflictResolutionResult<TThreadsafe> result)
        {
            switch (result)
            {
                case CreateResult<TThreadsafe> c:
                    currentSubject.OnNext(c.Entity);
                    break;

                case UpdateResult<TThreadsafe> u:
                    currentSubject.OnNext(u.Entity);
                    break;
            }
        }
    }
}
