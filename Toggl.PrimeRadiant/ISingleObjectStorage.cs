using System;
using System.Reactive;
using Toggl.PrimeRadiant.Models;

namespace Toggl.PrimeRadiant
{
    public interface ISingleObjectStorage<TModel, TDto> : IRepository<TModel, TDto>
        where TModel : IDatabaseModel
    {
        IObservable<TModel> Single();
        IObservable<Unit> Delete();
        IObservable<TModel> Update(TDto entity);
    }
}
