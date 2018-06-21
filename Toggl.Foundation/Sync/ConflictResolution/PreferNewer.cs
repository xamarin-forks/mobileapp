using Toggl.PrimeRadiant;
using Toggl.Multivac;
using static Toggl.PrimeRadiant.ConflictResolutionMode;
using System;
using Toggl.Multivac.Models;

namespace Toggl.Foundation.Sync.ConflictResolution
{
    internal sealed class PreferNewer<T, TDto> : IConflictResolver<T, TDto>
        where T : class, ILastChangedDatable, IDatabaseSyncable
        where TDto : ILastChangedDatable
    {
        public TimeSpan MarginOfError { get; }

        public PreferNewer()
            : this(TimeSpan.Zero)
        {
        }

        public PreferNewer(TimeSpan marginOfError)
        {
            Ensure.Argument.IsNotNull(marginOfError, nameof(marginOfError));

            MarginOfError = marginOfError;
        }

        public ConflictResolutionMode Resolve(T localEntity, TDto serverEntity)
        {
            Ensure.Argument.IsNotNull(serverEntity, nameof(serverEntity));

            if (serverEntity is IDeletable deletable && deletable.ServerDeletedAt.HasValue)
                return localEntity == null ? Ignore : Delete;

            if (localEntity == null)
                return Create;

            if (localEntity.SyncStatus == SyncStatus.InSync
                || localEntity.SyncStatus == SyncStatus.RefetchingNeeded)
                return Update;

            var receivedDataIsOutdated = localEntity.At.Add(MarginOfError) > serverEntity.At;
            if (receivedDataIsOutdated)
                return Ignore;

            return Update;
        }
    }
}
