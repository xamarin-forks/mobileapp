using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Realms;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant.Exceptions;
using Toggl.PrimeRadiant.Models;

namespace Toggl.PrimeRadiant.Realm
{
    internal sealed class SingleObjectStorage<TModel, TDto>
        : BaseStorage<TModel, TDto>, ISingleObjectStorage<TModel, TDto>
        where TModel : IDatabaseModel
    {
        private const int fakeId = 0;

        public SingleObjectStorage(IRealmAdapter<TModel, TDto> adapter)
            : base(adapter) { }

        public IObservable<TModel> GetById(long _)
            => Single();

        public IObservable<TModel> Create(TDto entity)
        {
            Ensure.Argument.IsNotNull(entity, nameof(entity));

            return Observable.Defer(() =>
            {
                if (Adapter.GetAll().Any())
                    return Observable.Throw<TModel>(new EntityAlreadyExistsException());

                return Adapter.Create(entity)
                              .Apply(Observable.Return)
                              .Catch<TModel, Exception>(ex => Observable.Throw<TModel>(new DatabaseException(ex)));
            });
        }

        public IObservable<IEnumerable<IConflictResolutionResult<TModel>>> BatchUpdate(
            IEnumerable<(long Id, TDto Entity)> entities,
            Func<TModel, TDto, ConflictResolutionMode> conflictResolution,
            IRivalsResolver<TModel, TDto> rivalsResolver = null)
            => CreateObservable(() =>
            {
                var list = entities.ToList();
                if (list.Count > 1)
                    throw new ArgumentException("Too many entities to update.");

                return Adapter.BatchUpdate(list, conflictResolution, rivalsResolver);
            });

        public IObservable<TModel> Single()
            => CreateObservable(() => Adapter.GetAll().Single());

        public static SingleObjectStorage<TModel, TDto> For<TRealmEntity>(
            Func<Realms.Realm> getRealmInstance, Func<TDto, Realms.Realm, TRealmEntity> convertToRealm)
            where TRealmEntity : RealmObject, TModel, IUpdatesFrom<TDto>
            => new SingleObjectStorage<TModel, TDto>(new RealmAdapter<TRealmEntity, TModel, TDto>(
                getRealmInstance,
                convertToRealm, _ => __ => true,
                obj => fakeId));

        public IObservable<TModel> Update(TDto entity)
            => Update(fakeId, entity);

        public IObservable<Unit> Delete()
            => Single().SelectMany(entity => Delete(fakeId));
    }
}
