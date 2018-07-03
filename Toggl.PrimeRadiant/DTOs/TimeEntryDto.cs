using System;
using System.Collections.Generic;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant
{
    public struct TimeEntryDto : ITimeEntry, IDatabaseSyncable
    {
        public TimeEntryDto(
            long id,
            DateTimeOffset at,
            long workspaceId,
            long? projectId,
            long? taskId,
            bool billable,
            DateTimeOffset start,
            long? duration,
            string description,
            IEnumerable<long> tagIds,
            long userId,
            DateTimeOffset? serverDeletedAt = null,
            SyncStatus syncStatus = SyncStatus.SyncNeeded,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
        {
            Id = id;
            ServerDeletedAt = serverDeletedAt;
            At = at;
            WorkspaceId = workspaceId;
            ProjectId = projectId;
            TaskId = taskId;
            Billable = billable;
            Start = start;
            Duration = duration;
            Description = description;
            TagIds = tagIds;
            UserId = userId;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static TimeEntryDto From<T>(
            T entity,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> workspaceId = default(New<long>),
            New<long?> projectId = default(New<long?>),
            New<long?> taskId = default(New<long?>),
            New<bool> billable = default(New<bool>),
            New<DateTimeOffset> start = default(New<DateTimeOffset>),
            New<long?> duration = default(New<long?>),
            New<string> description = default(New<string>),
            New<IEnumerable<long>> tagIds = default(New<IEnumerable<long>>),
            New<long> userId = default(New<long>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : ITimeEntry, IDatabaseSyncable
        => new TimeEntryDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            projectId: projectId.ValueOr(entity.ProjectId),
            taskId: taskId.ValueOr(entity.TaskId),
            billable: billable.ValueOr(entity.Billable),
            start: start.ValueOr(entity.Start),
            duration: duration.ValueOr(entity.Duration),
            description: description.ValueOr(entity.Description),
            tagIds: tagIds.ValueOr(entity.TagIds),
            userId: userId.ValueOr(entity.UserId),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static TimeEntryDto Clean(ITimeEntry entity) => createFrom(entity, SyncStatus.InSync);

        public static TimeEntryDto Dirty(ITimeEntry entity) => createFrom(entity, SyncStatus.SyncNeeded);

        public static TimeEntryDto DirtyDeleted(ITimeEntry entity) => createFrom(entity, SyncStatus.SyncNeeded, isDeleted: true);

        public static TimeEntryDto Unsyncable(ITimeEntry entity, string errorMessage) => createFrom(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        private static TimeEntryDto createFrom(
            ITimeEntry entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
        => new TimeEntryDto(
            id: entity.Id,
            serverDeletedAt: entity.ServerDeletedAt,
            at: entity.At,
            workspaceId: entity.WorkspaceId,
            projectId: entity.ProjectId,
            taskId: entity.TaskId,
            billable: entity.Billable,
            start: entity.Start,
            duration: entity.Duration,
            description: entity.Description,
            tagIds: entity.TagIds,
            userId: entity.UserId,
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);
        public long Id { get; }
        public DateTimeOffset? ServerDeletedAt { get; }
        public DateTimeOffset At { get; }
        public long WorkspaceId { get; }
        public long? ProjectId { get; }
        public long? TaskId { get; }
        public bool Billable { get; }
        public DateTimeOffset Start { get; }
        public long? Duration { get; }
        public string Description { get; }
        public IEnumerable<long> TagIds { get; }
        public long UserId { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }
}
