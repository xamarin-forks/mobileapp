using System;
using System.Linq;
using System.Linq.Expressions;

namespace Toggl.PrimeRadiant
{
    public interface IRivalsResolver<TModel, TDto>
    {
        bool CanHaveRival(TModel entity);
        Expression<Func<TModel, bool>> AreRivals(TModel entity);
        (TDto FixedEntity, TDto FixedRival) FixRivals<TRealmObject>(TModel entity, TModel rival, IQueryable<TRealmObject> allEntities)
            where TRealmObject : TModel;
    }
}
