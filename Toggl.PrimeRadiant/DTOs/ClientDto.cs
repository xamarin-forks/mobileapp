using System;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.DTOs
{
    public struct ClientDto : IClient, IDatabaseSyncable
    {
        public ClientDto(
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

        public static ClientDto Clean(IClient entity)
            => createFrom(entity, SyncStatus.InSync);

        public static ClientDto Unsyncable(IClient entity, string errorMessage)
            => createFrom(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        private static ClientDto createFrom(
            IClient entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
            => new ClientDto(
                id: entity.Id,
                serverDeletedAt: entity.ServerDeletedAt,
                at: entity.At,
                workspaceId: entity.WorkspaceId,
                name: entity.Name,
                syncStatus: syncStatus,
                isDeleted: isDeleted,
                lastSyncErrorMessage: lastSyncErrorMessage);

        public long Id { get; }
        public DateTimeOffset? ServerDeletedAt { get; }
        public DateTimeOffset At { get; }
        public long WorkspaceId { get; }
        public string Name { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }

}
