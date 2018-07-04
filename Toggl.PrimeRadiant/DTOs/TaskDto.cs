using System;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.DTOs
{
    public struct TaskDto : ITask, IDatabaseSyncable
    {
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

        private TaskDto(
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

        public static TaskDto Clean(ITask entity) => createFrom(entity, SyncStatus.InSync);

        private static TaskDto createFrom(
            ITask entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
        => new TaskDto(
            id: entity.Id,
            name: entity.Name,
            at: entity.At,
            projectId: entity.ProjectId,
            workspaceId: entity.WorkspaceId,
            userId: entity.UserId,
            estimatedSeconds: entity.EstimatedSeconds,
            active: entity.Active,
            trackedSeconds: entity.TrackedSeconds,
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);
    }
}
