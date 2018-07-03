using System;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant
{
    public struct TaskDto : ITask, IDatabaseSyncable
    {
        public TaskDto(
            long id,
            string name,
            DateTimeOffset at,
            long projectId,
            long workspaceId,
            long? userId,
            long estimatedSeconds,
            bool active,
            long trackedSeconds,
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
        {
            Id = id;
            Name = name;
            At = at;
            ProjectId = projectId;
            WorkspaceId = workspaceId;
            UserId = userId;
            EstimatedSeconds = estimatedSeconds;
            Active = active;
            TrackedSeconds = trackedSeconds;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static TaskDto From(
            ITask entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null,
            New<long> id = default(New<long>),
            New<string> name = default(New<string>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> projectId = default(New<long>),
            New<long> workspaceId = default(New<long>),
            New<long?> userId = default(New<long?>),
            New<long> estimatedSeconds = default(New<long>),
            New<bool> active = default(New<bool>),
            New<long> trackedSeconds = default(New<long>))
        => new TaskDto(
            id: id.ValueOr(entity.Id),
            name: name.ValueOr(entity.Name),
            at: at.ValueOr(entity.At),
            projectId: projectId.ValueOr(entity.ProjectId),
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            userId: userId.ValueOr(entity.UserId),
            estimatedSeconds: estimatedSeconds.ValueOr(entity.EstimatedSeconds),
            active: active.ValueOr(entity.Active),
            trackedSeconds: trackedSeconds.ValueOr(entity.TrackedSeconds),
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);

        public static TaskDto Clean(ITask entity) => From(entity, SyncStatus.InSync);

        public long Id { get; }
        public string Name { get; }
        public DateTimeOffset At { get; }
        public long ProjectId { get; }
        public long WorkspaceId { get; }
        public long? UserId { get; }
        public long EstimatedSeconds { get; }
        public bool Active { get; }
        public long TrackedSeconds { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }
}
