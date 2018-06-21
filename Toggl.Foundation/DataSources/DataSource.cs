using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Extensions;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public abstract class DataSource<TThreadsafe, TDatabase, TDto> : BaseDataSource<TThreadsafe, TDatabase, TDto>, IDataSource<TThreadsafe, TDatabase, TDto>
        where TDto : IIdentifiable
        where TDatabase : IDatabaseModel
        where TThreadsafe : TDatabase, IThreadSafeModel, IIdentifiable
    {
        protected DataSource(IRepository<TDatabase, TDto> repository)
            : base(repository)
        {
        }

        public IObservable<TThreadsafe> GetById(long id)
            => Repository.GetById(id).Select(Convert);

        public virtual IObservable<IEnumerable<TThreadsafe>> GetAll()
            => Repository.GetAll().Select(entities => entities.Select(Convert));

        public virtual IObservable<IEnumerable<TThreadsafe>> GetAll(Func<TDatabase, bool> predicate)
            => Repository.GetAll(predicate).Select(entities => entities.Select(Convert));

        public virtual IObservable<IEnumerable<IConflictResolutionResult<TThreadsafe>>> DeleteAll(IEnumerable<TDto> entities)
            => Repository.BatchUpdate(convertEntitiesForBatchUpdate(entities), safeAlwaysDelete)
                         .ToThreadSafeResult(Convert);

        public virtual IObservable<Unit> Delete(long id)
            => Repository.Delete(id);

        public virtual IObservable<IEnumerable<IConflictResolutionResult<TThreadsafe>>> BatchUpdate(IEnumerable<TDto> entities)
            => Repository.BatchUpdate(
                    convertEntitiesForBatchUpdate(entities),
                    ResolveConflicts,
                    RivalsResolver)
                .ToThreadSafeResult(Convert);

        private IEnumerable<(long, TDto)> convertEntitiesForBatchUpdate(
            IEnumerable<TDto> entities)
            => entities.Select(entity => (entity.Id, entity));

        private static ConflictResolutionMode safeAlwaysDelete(TDatabase old, TDto now)
            => old == null ? ConflictResolutionMode.Ignore : ConflictResolutionMode.Delete;
    }
}
