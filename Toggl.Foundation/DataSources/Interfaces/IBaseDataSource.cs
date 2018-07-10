using System;
using System.Collections.Generic;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant;

namespace Toggl.Foundation.DataSources
{
    public interface IBaseDataSource<T, TDto>
        where T : IThreadSafeModel
    {
        IObservable<T> Create(TDto entity);

        IObservable<T> Update(TDto entity);

        IObservable<T> Overwrite(T original, TDto entity);

        IObservable<IEnumerable<IConflictResolutionResult<T>>> OverwriteIfOriginalDidNotChange(T original, TDto entity);
    }
}
