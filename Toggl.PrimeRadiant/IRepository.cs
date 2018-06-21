using System;
using System.Collections.Generic;
using System.Reactive;

namespace Toggl.PrimeRadiant
{
    public interface IRepository<TModel, TDto>
    {
        IObservable<TModel> GetById(long id);
        IObservable<TModel> Create(TDto entity);
        IObservable<TModel> Update(long id, TDto entity);
        IObservable<IEnumerable<IConflictResolutionResult<TModel>>> BatchUpdate(
            IEnumerable<(long Id, TDto Entity)> entities,
            Func<TModel, TDto, ConflictResolutionMode> conflictResolution,
            IRivalsResolver<TModel, TDto> rivalsResolver = null);
        IObservable<Unit> Delete(long id);
        IObservable<IEnumerable<TModel>> GetAll();
        IObservable<IEnumerable<TModel>> GetAll(Func<TModel, bool> predicate);
    }
}
