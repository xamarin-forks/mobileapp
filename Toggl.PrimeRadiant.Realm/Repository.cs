using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using Realms;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant.Exceptions;

namespace Toggl.PrimeRadiant.Realm
{
    internal sealed class Repository<TModel, TDto> : BaseStorage<TModel, TDto>, IRepository<TModel, TDto>
    {
        public Repository(IRealmAdapter<TModel, TDto> adapter)
            : base(adapter) { }

        public IObservable<TModel> Create(TDto entity)
        {
            Ensure.Argument.IsNotNull(entity, nameof(entity));

            return Observable
                .Start(() => Adapter.Create(entity))
                .Catch<TModel, Exception>(ex => Observable.Throw<TModel>(new DatabaseException(ex)));
        }
        
        public IObservable<IEnumerable<IConflictResolutionResult<TModel>>> BatchUpdate(
            IEnumerable<(long Id, TDto Entity)> entities,
            Func<TModel, TDto, ConflictResolutionMode> conflictResolution,
            IRivalsResolver<TModel, TDto> rivalsResolver = null)
        {
            Ensure.Argument.IsNotNull(entities, nameof(entities));
            Ensure.Argument.IsNotNull(conflictResolution, nameof(conflictResolution));

            return CreateObservable(() => Adapter.BatchUpdate(entities, conflictResolution, rivalsResolver));
        }

        public IObservable<TModel> GetById(long id)
            => CreateObservable(() => Adapter.Get(id));

        public static Repository<TModel, TDto> For<TRealmEntity>(
            Func<Realms.Realm> getRealmInstance, Func<TDto, Realms.Realm, TRealmEntity> convertToRealm)
            where TRealmEntity : RealmObject, IIdentifiable, TModel, IUpdatesFrom<TDto>
            => For(getRealmInstance, convertToRealm, matchById<TRealmEntity>, getId<TRealmEntity>);

        public static Repository<TModel, TDto> For<TRealmEntity>(
            Func<Realms.Realm> getRealmInstance,
            Func<TDto, Realms.Realm, TRealmEntity> convertToRealm,
            Func<long, Expression<Func<TRealmEntity, bool>>> matchById,
            Func<TRealmEntity, long> getId)
            where TRealmEntity : RealmObject, TModel, IUpdatesFrom<TDto>
            => new Repository<TModel, TDto>(new RealmAdapter<TRealmEntity, TModel, TDto>(getRealmInstance, convertToRealm, matchById, getId));

        private static Expression<Func<TRealmEntity, bool>> matchById<TRealmEntity>(long id)
            where TRealmEntity : RealmObject, IIdentifiable, TModel, IUpdatesFrom<TDto>
            => x => x.Id == id;

        private static long getId<TRealmEntity>(TRealmEntity entity)
            where TRealmEntity : RealmObject, IIdentifiable, TModel
            => entity.Id;
    }
}
