using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Exceptions;

namespace Toggl.PrimeRadiant.Realm
{
    internal abstract class BaseStorage<TDatabaseModel, TDto>
    {
        protected IRealmAdapter<TDatabaseModel, TDto> Adapter { get; }

        protected BaseStorage(IRealmAdapter<TDatabaseModel, TDto> adapter)
        {
            Adapter = adapter;
        }

        public IObservable<TDatabaseModel> Update(long id, TDto entity)
        {
            Ensure.Argument.IsNotNull(entity, nameof(entity));

            return CreateObservable(() => Adapter.Update(id, entity));
        }

        public IObservable<Unit> Delete(long id)
            => CreateObservable(() =>
            {
                Adapter.Delete(id);
                return Unit.Default;
            });

        public IObservable<IEnumerable<TDatabaseModel>> GetAll(Func<TDatabaseModel, bool> predicate)
        {
            Ensure.Argument.IsNotNull(predicate, nameof(predicate));

            return CreateObservable(() => Adapter.GetAll().Where(predicate));
        }

        public IObservable<IEnumerable<TDatabaseModel>> GetAll()
            => CreateObservable(() => Adapter.GetAll());

        protected static IObservable<T> CreateObservable<T>(Func<T> getFunction)
        {
            return Observable.Create<T>(observer =>
            {
                try
                {
                    var data = getFunction();
                    observer.OnNext(data);
                    observer.OnCompleted();
                }
                catch (InvalidOperationException ex)
                {
                    observer.OnError(new DatabaseOperationException<TDatabaseModel>(ex));
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }

                return Disposable.Empty;
            });
        }
    }
}
