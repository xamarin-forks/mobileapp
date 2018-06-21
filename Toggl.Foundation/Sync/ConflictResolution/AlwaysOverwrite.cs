using Toggl.Multivac;
using Toggl.PrimeRadiant;
using static Toggl.PrimeRadiant.ConflictResolutionMode;

namespace Toggl.Foundation.Sync.ConflictResolution
{
    internal sealed class AlwaysOverwrite<T, TDto> : IConflictResolver<T, TDto>
        where T : class
    {
        public ConflictResolutionMode Resolve(T localEntity, TDto serverEntity)
        {
            Ensure.Argument.IsNotNull(serverEntity, nameof(serverEntity));

            if (localEntity == null)
                return Create;

            return Update;
        }
    }
}
