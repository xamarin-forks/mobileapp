using System;
using System.Collections.Generic;
using System.Reactive;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources.Interfaces
{
    public interface IDataSource<TThreadsafe, out TDatabase, TDto> : IBaseDataSource<TThreadsafe, TDto>
        where TDatabase : IDatabaseModel
        where TThreadsafe : TDatabase, IThreadSafeModel
    {
        IObservable<TThreadsafe> GetById(long id);

        IObservable<IEnumerable<TThreadsafe>> GetAll();

        IObservable<IEnumerable<TThreadsafe>> GetAll(Func<TDatabase, bool> predicate);

        IObservable<IEnumerable<IConflictResolutionResult<TThreadsafe>>> DeleteAll(IEnumerable<TDto> entities);

        IObservable<Unit> Delete(long id);

        IObservable<IEnumerable<IConflictResolutionResult<TThreadsafe>>> BatchUpdate(IEnumerable<TDto> entities);
    }
}
