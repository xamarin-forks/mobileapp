using System;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.DTOs
{
    public struct TagDto : ITag, IDatabaseSyncable
    {
        public long Id { get; }
        public DateTimeOffset? ServerDeletedAt { get; }
        public DateTimeOffset At { get; }
        public long WorkspaceId { get; }
        public string Name { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }

        public TagDto(
            long id,
            DateTimeOffset at,
            long workspaceId,
            string name,
            DateTimeOffset? serverDeletedAt = null,
            SyncStatus syncStatus = SyncStatus.SyncNeeded,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
        {
            Id = id;
            ServerDeletedAt = serverDeletedAt;
            At = at;
            WorkspaceId = workspaceId;
            Name = name;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static TagDto Clean(ITag entity) => createFrom(entity, SyncStatus.InSync);

        public static TagDto Unsyncable(ITag entity, string errorMessage) => createFrom(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        private static TagDto createFrom(
            ITag entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
            => new TagDto(
                id: entity.Id,
                serverDeletedAt: entity.ServerDeletedAt,
                at: entity.At,
                workspaceId: entity.WorkspaceId,
                name: entity.Name,
                syncStatus: syncStatus,
                isDeleted: isDeleted,
                lastSyncErrorMessage: lastSyncErrorMessage);
    }
}
