using System;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant;

namespace Toggl.Foundation.DataSources.Interfaces
{
    public interface ISingletonDataSource<T, TDto> : IBaseDataSource<T, TDto>
        where T : IThreadSafeModel
    {
        IObservable<T> Current { get; }

        IObservable<T> Get();

        IObservable<IConflictResolutionResult<T>> UpdateWithConflictResolution(TDto entity);
    }
}
