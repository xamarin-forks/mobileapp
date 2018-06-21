using Toggl.PrimeRadiant;

namespace Toggl.Foundation.Sync.ConflictResolution
{
    internal interface IConflictResolver<T, TDto>
    {
        ConflictResolutionMode Resolve(T localEntity, TDto serverEntity);
    }
}
