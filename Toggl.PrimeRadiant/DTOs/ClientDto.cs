using System;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant
{
    public struct ClientDto : IClient, IDatabaseSyncable
    {
        public ClientDto(
            long id,
            DateTimeOffset? serverDeletedAt,
            DateTimeOffset at,
            long workspaceId,
            string name,
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
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

        public static ClientDto From(
            IClient entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> workspaceId = default(New<long>),
            New<string> name = default(New<string>))
        => new ClientDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            name: name.ValueOr(entity.Name),
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);

        public static ClientDto Clean(IClient entity) => From(entity, SyncStatus.InSync);

        public static ClientDto Unsyncable(IClient entity, string errorMessage) => From(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

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
