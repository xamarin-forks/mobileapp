using System;
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

        IObservable<IConflictResolutionResult<T>> OverwriteIfOriginalDidNotChange(T original, TDto entity);
    }
}
