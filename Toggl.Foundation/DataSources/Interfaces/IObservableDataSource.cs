using System;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources.Interfaces
{
    public interface IObservableDataSource<TThreadsafe, out TDatabase, TDto>
        : IDataSource<TThreadsafe, TDatabase, TDto>
        where TDatabase : IDatabaseModel
        where TThreadsafe : IThreadSafeModel, TDatabase
    {
        IObservable<TThreadsafe> Created { get; }
        
        IObservable<EntityUpdate<TThreadsafe>> Updated { get; }
        
        IObservable<long> Deleted { get; }
    }
}
