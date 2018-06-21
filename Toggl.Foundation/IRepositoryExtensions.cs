using System;
using System.Linq;
using System.Reactive.Linq;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;

namespace Toggl.Foundation
{
    public static class IRepositoryExtensions
    {
        public static IObservable<TModel> Update<TModel, TDto>(this IRepository<TModel, TDto> repository, TDto entity)
            where TDto : IIdentifiable
            => repository.Update(entity.Id, entity);

        public static IObservable<IConflictResolutionResult<TModel>> UpdateWithConflictResolution<TModel, TDto>(
            this IRepository<TModel, TDto> repository,
            long id,
            TDto entity,
            Func<TModel, TDto, ConflictResolutionMode> conflictResolution,
            IRivalsResolver<TModel, TDto> rivalsResolver = null)
            => repository
                .BatchUpdate(new[] { (id, entity) }, conflictResolution, rivalsResolver)
                .SingleAsync()
                .Select(entities => entities.First());
    }
}
