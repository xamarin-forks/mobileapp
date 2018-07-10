using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Extensions;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public abstract class BaseDataSource<TThreadsafe, TDatabase, TDto> : IBaseDataSource<TThreadsafe, TDto>
        where TDto : IIdentifiable
        where TDatabase : IDatabaseModel
        where TThreadsafe : IThreadSafeModel, IIdentifiable, TDatabase
    {
        protected readonly IRepository<TDatabase, TDto> Repository;

        protected virtual IRivalsResolver<TDatabase, TDto> RivalsResolver { get; } = null;

        protected BaseDataSource(IRepository<TDatabase, TDto> repository)
        {
            Ensure.Argument.IsNotNull(repository, nameof(repository));

            Repository = repository;
        }

        public virtual IObservable<TThreadsafe> Create(TDto entity)
            => Repository.Create(entity).Select(Convert);

        public virtual IObservable<TThreadsafe> Update(TDto entity)
            => Repository.Update(entity.Id, entity).Select(Convert);

        public virtual IObservable<TThreadsafe> Overwrite(TThreadsafe original, TDto entity)
            => Repository.Update(original.Id, entity).Select(Convert);

        public virtual IObservable<IEnumerable<IConflictResolutionResult<TThreadsafe>>> OverwriteIfOriginalDidNotChange(TThreadsafe original, TDto entity)
            => Repository.UpdateWithConflictResolution(original.Id, entity, ignoreIfChangedLocally(original), RivalsResolver)
                .ToThreadSafeResult(Convert);

        private Func<TDatabase, TDto, ConflictResolutionMode> ignoreIfChangedLocally(TThreadsafe localEntity)
            => (currentLocal, serverEntity) => localEntity.DiffersFrom(currentLocal)
                ? ConflictResolutionMode.Ignore
                : ConflictResolutionMode.Update;

        protected abstract TThreadsafe Convert(TDatabase entity);

        protected abstract ConflictResolutionMode ResolveConflicts(TDatabase first, TDto second);
    }
}
