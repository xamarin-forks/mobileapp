using System;
using System.Collections.Generic;
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

        public static ClientDto From<T>(
            T entity,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> workspaceId = default(New<long>),
            New<string> name = default(New<string>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : IClient, IDatabaseSyncable
        => new ClientDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            name: name.ValueOr(entity.Name),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static ClientDto Clean(IClient entity) => From(entity, SyncStatus.InSync);

        public static ClientDto Dirty(IClient entity) => From(entity, SyncStatus.SyncNeeded);

        public static ClientDto DirtyDeleted(IClient entity) => From(entity, SyncStatus.SyncNeeded, isDeleted: true);

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

    public struct PreferencesDto : IPreferences, IDatabaseSyncable, IIdentifiable
    {
        public PreferencesDto(
            TimeFormat timeOfDayFormat,
            DateFormat dateFormat,
            DurationFormat durationFormat,
            bool collapseTimeEntries,
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
        {
            TimeOfDayFormat = timeOfDayFormat;
            DateFormat = dateFormat;
            DurationFormat = durationFormat;
            CollapseTimeEntries = collapseTimeEntries;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static PreferencesDto From(
            IPreferences entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null,
            New<TimeFormat> timeOfDayFormat = default(New<TimeFormat>),
            New<DateFormat> dateFormat = default(New<DateFormat>),
            New<DurationFormat> durationFormat = default(New<DurationFormat>),
            New<bool> collapseTimeEntries = default(New<bool>))
        => new PreferencesDto(
            timeOfDayFormat: timeOfDayFormat.ValueOr(entity.TimeOfDayFormat),
            dateFormat: dateFormat.ValueOr(entity.DateFormat),
            durationFormat: durationFormat.ValueOr(entity.DurationFormat),
            collapseTimeEntries: collapseTimeEntries.ValueOr(entity.CollapseTimeEntries),
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);

        public static PreferencesDto From<T>(
            T entity,
            New<TimeFormat> timeOfDayFormat = default(New<TimeFormat>),
            New<DateFormat> dateFormat = default(New<DateFormat>),
            New<DurationFormat> durationFormat = default(New<DurationFormat>),
            New<bool> collapseTimeEntries = default(New<bool>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : IPreferences, IDatabaseSyncable
        => new PreferencesDto(
            timeOfDayFormat: timeOfDayFormat.ValueOr(entity.TimeOfDayFormat),
            dateFormat: dateFormat.ValueOr(entity.DateFormat),
            durationFormat: durationFormat.ValueOr(entity.DurationFormat),
            collapseTimeEntries: collapseTimeEntries.ValueOr(entity.CollapseTimeEntries),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static PreferencesDto Clean(IPreferences entity) => From(entity, SyncStatus.InSync);

        public static PreferencesDto Dirty(IPreferences entity) => From(entity, SyncStatus.SyncNeeded);

        public static PreferencesDto DirtyDeleted(IPreferences entity) => From(entity, SyncStatus.SyncNeeded, isDeleted: true);

        public static PreferencesDto Unsyncable(IPreferences entity, string errorMessage) => From(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        public long Id => 0;
        public TimeFormat TimeOfDayFormat { get; }
        public DateFormat DateFormat { get; }
        public DurationFormat DurationFormat { get; }
        public bool CollapseTimeEntries { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }

    public struct ProjectDto : IProject, IDatabaseSyncable
    {
        public ProjectDto(
            long id,
            DateTimeOffset? serverDeletedAt,
            DateTimeOffset at,
            long workspaceId,
            long? clientId,
            string name,
            bool isPrivate,
            bool active,
            string color,
            bool? billable,
            bool? template,
            bool? autoEstimates,
            long? estimatedHours,
            double? rate,
            string currency,
            int? actualHours,
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
        {
            Id = id;
            ServerDeletedAt = serverDeletedAt;
            At = at;
            WorkspaceId = workspaceId;
            ClientId = clientId;
            Name = name;
            IsPrivate = isPrivate;
            Active = active;
            Color = color;
            Billable = billable;
            Template = template;
            AutoEstimates = autoEstimates;
            EstimatedHours = estimatedHours;
            Rate = rate;
            Currency = currency;
            ActualHours = actualHours;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static ProjectDto From(
            IProject entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> workspaceId = default(New<long>),
            New<long?> clientId = default(New<long?>),
            New<string> name = default(New<string>),
            New<bool> isPrivate = default(New<bool>),
            New<bool> active = default(New<bool>),
            New<string> color = default(New<string>),
            New<bool?> billable = default(New<bool?>),
            New<bool?> template = default(New<bool?>),
            New<bool?> autoEstimates = default(New<bool?>),
            New<long?> estimatedHours = default(New<long?>),
            New<double?> rate = default(New<double?>),
            New<string> currency = default(New<string>),
            New<int?> actualHours = default(New<int?>))
        => new ProjectDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            clientId: clientId.ValueOr(entity.ClientId),
            name: name.ValueOr(entity.Name),
            isPrivate: isPrivate.ValueOr(entity.IsPrivate),
            active: active.ValueOr(entity.Active),
            color: color.ValueOr(entity.Color),
            billable: billable.ValueOr(entity.Billable),
            template: template.ValueOr(entity.Template),
            autoEstimates: autoEstimates.ValueOr(entity.AutoEstimates),
            estimatedHours: estimatedHours.ValueOr(entity.EstimatedHours),
            rate: rate.ValueOr(entity.Rate),
            currency: currency.ValueOr(entity.Currency),
            actualHours: actualHours.ValueOr(entity.ActualHours),
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);

        public static ProjectDto From<T>(
            T entity,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> workspaceId = default(New<long>),
            New<long?> clientId = default(New<long?>),
            New<string> name = default(New<string>),
            New<bool> isPrivate = default(New<bool>),
            New<bool> active = default(New<bool>),
            New<string> color = default(New<string>),
            New<bool?> billable = default(New<bool?>),
            New<bool?> template = default(New<bool?>),
            New<bool?> autoEstimates = default(New<bool?>),
            New<long?> estimatedHours = default(New<long?>),
            New<double?> rate = default(New<double?>),
            New<string> currency = default(New<string>),
            New<int?> actualHours = default(New<int?>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : IProject, IDatabaseSyncable
        => new ProjectDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            clientId: clientId.ValueOr(entity.ClientId),
            name: name.ValueOr(entity.Name),
            isPrivate: isPrivate.ValueOr(entity.IsPrivate),
            active: active.ValueOr(entity.Active),
            color: color.ValueOr(entity.Color),
            billable: billable.ValueOr(entity.Billable),
            template: template.ValueOr(entity.Template),
            autoEstimates: autoEstimates.ValueOr(entity.AutoEstimates),
            estimatedHours: estimatedHours.ValueOr(entity.EstimatedHours),
            rate: rate.ValueOr(entity.Rate),
            currency: currency.ValueOr(entity.Currency),
            actualHours: actualHours.ValueOr(entity.ActualHours),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static ProjectDto Clean(IProject entity) => From(entity, SyncStatus.InSync);

        public static ProjectDto Dirty(IProject entity) => From(entity, SyncStatus.SyncNeeded);

        public static ProjectDto DirtyDeleted(IProject entity) => From(entity, SyncStatus.SyncNeeded, isDeleted: true);

        public static ProjectDto Unsyncable(IProject entity, string errorMessage) => From(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        public long Id { get; }
        public DateTimeOffset? ServerDeletedAt { get; }
        public DateTimeOffset At { get; }
        public long WorkspaceId { get; }
        public long? ClientId { get; }
        public string Name { get; }
        public bool IsPrivate { get; }
        public bool Active { get; }
        public string Color { get; }
        public bool? Billable { get; }
        public bool? Template { get; }
        public bool? AutoEstimates { get; }
        public long? EstimatedHours { get; }
        public double? Rate { get; }
        public string Currency { get; }
        public int? ActualHours { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }

    public struct TagDto : ITag, IDatabaseSyncable
    {
        public TagDto(
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

        public static TagDto From(
            ITag entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> workspaceId = default(New<long>),
            New<string> name = default(New<string>))
        => new TagDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            name: name.ValueOr(entity.Name),
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);

        public static TagDto From<T>(
            T entity,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> workspaceId = default(New<long>),
            New<string> name = default(New<string>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : ITag, IDatabaseSyncable
        => new TagDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            name: name.ValueOr(entity.Name),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static TagDto Clean(ITag entity) => From(entity, SyncStatus.InSync);

        public static TagDto Dirty(ITag entity) => From(entity, SyncStatus.SyncNeeded);

        public static TagDto DirtyDeleted(ITag entity) => From(entity, SyncStatus.SyncNeeded, isDeleted: true);

        public static TagDto Unsyncable(ITag entity, string errorMessage) => From(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        public long Id { get; }
        public DateTimeOffset? ServerDeletedAt { get; }
        public DateTimeOffset At { get; }
        public long WorkspaceId { get; }
        public string Name { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }

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

        public static TaskDto From<T>(
            T entity,
            New<long> id = default(New<long>),
            New<string> name = default(New<string>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long> projectId = default(New<long>),
            New<long> workspaceId = default(New<long>),
            New<long?> userId = default(New<long?>),
            New<long> estimatedSeconds = default(New<long>),
            New<bool> active = default(New<bool>),
            New<long> trackedSeconds = default(New<long>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : ITask, IDatabaseSyncable
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
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static TaskDto Clean(ITask entity) => From(entity, SyncStatus.InSync);

        public static TaskDto Dirty(ITask entity) => From(entity, SyncStatus.SyncNeeded);

        public static TaskDto DirtyDeleted(ITask entity) => From(entity, SyncStatus.SyncNeeded, isDeleted: true);

        public static TaskDto Unsyncable(ITask entity, string errorMessage) => From(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

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

    public struct TimeEntryDto : ITimeEntry, IDatabaseSyncable
    {
        public TimeEntryDto(
            long id,
            DateTimeOffset? serverDeletedAt,
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
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
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

        public static TimeEntryDto From(
            ITimeEntry entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null,
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
            New<long> userId = default(New<long>))
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
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);

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

        public static TimeEntryDto Clean(ITimeEntry entity) => From(entity, SyncStatus.InSync);

        public static TimeEntryDto Dirty(ITimeEntry entity) => From(entity, SyncStatus.SyncNeeded);

        public static TimeEntryDto DirtyDeleted(ITimeEntry entity) => From(entity, SyncStatus.SyncNeeded, isDeleted: true);

        public static TimeEntryDto Unsyncable(ITimeEntry entity, string errorMessage) => From(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

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

    public struct UserDto : IUser, IDatabaseSyncable
    {
        public UserDto(
            long id,
            string apiToken,
            DateTimeOffset at,
            long? defaultWorkspaceId,
            Email email,
            string fullname,
            BeginningOfWeek beginningOfWeek,
            string language,
            string imageUrl,
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
        {
            Id = id;
            ApiToken = apiToken;
            At = at;
            DefaultWorkspaceId = defaultWorkspaceId;
            Email = email;
            Fullname = fullname;
            BeginningOfWeek = beginningOfWeek;
            Language = language;
            ImageUrl = imageUrl;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static UserDto From(
            IUser entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null,
            New<long> id = default(New<long>),
            New<string> apiToken = default(New<string>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long?> defaultWorkspaceId = default(New<long?>),
            New<Email> email = default(New<Email>),
            New<string> fullname = default(New<string>),
            New<BeginningOfWeek> beginningOfWeek = default(New<BeginningOfWeek>),
            New<string> language = default(New<string>),
            New<string> imageUrl = default(New<string>))
        => new UserDto(
            id: id.ValueOr(entity.Id),
            apiToken: apiToken.ValueOr(entity.ApiToken),
            at: at.ValueOr(entity.At),
            defaultWorkspaceId: defaultWorkspaceId.ValueOr(entity.DefaultWorkspaceId),
            email: email.ValueOr(entity.Email),
            fullname: fullname.ValueOr(entity.Fullname),
            beginningOfWeek: beginningOfWeek.ValueOr(entity.BeginningOfWeek),
            language: language.ValueOr(entity.Language),
            imageUrl: imageUrl.ValueOr(entity.ImageUrl),
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);

        public static UserDto From<T>(
            T entity,
            New<long> id = default(New<long>),
            New<string> apiToken = default(New<string>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<long?> defaultWorkspaceId = default(New<long?>),
            New<Email> email = default(New<Email>),
            New<string> fullname = default(New<string>),
            New<BeginningOfWeek> beginningOfWeek = default(New<BeginningOfWeek>),
            New<string> language = default(New<string>),
            New<string> imageUrl = default(New<string>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : IUser, IDatabaseSyncable
        => new UserDto(
            id: id.ValueOr(entity.Id),
            apiToken: apiToken.ValueOr(entity.ApiToken),
            at: at.ValueOr(entity.At),
            defaultWorkspaceId: defaultWorkspaceId.ValueOr(entity.DefaultWorkspaceId),
            email: email.ValueOr(entity.Email),
            fullname: fullname.ValueOr(entity.Fullname),
            beginningOfWeek: beginningOfWeek.ValueOr(entity.BeginningOfWeek),
            language: language.ValueOr(entity.Language),
            imageUrl: imageUrl.ValueOr(entity.ImageUrl),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static UserDto Clean(IUser entity) => From(entity, SyncStatus.InSync);

        public static UserDto Dirty(IUser entity) => From(entity, SyncStatus.SyncNeeded);

        public static UserDto DirtyDeleted(IUser entity) => From(entity, SyncStatus.SyncNeeded, isDeleted: true);

        public static UserDto Unsyncable(IUser entity, string errorMessage) => From(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        public long Id { get; }
        public string ApiToken { get; }
        public DateTimeOffset At { get; }
        public long? DefaultWorkspaceId { get; }
        public Email Email { get; }
        public string Fullname { get; }
        public BeginningOfWeek BeginningOfWeek { get; }
        public string Language { get; }
        public string ImageUrl { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }

    public struct WorkspaceDto : IWorkspace, IDatabaseSyncable
    {
        public WorkspaceDto(
            long id,
            DateTimeOffset? serverDeletedAt,
            DateTimeOffset at,
            string name,
            bool admin,
            DateTimeOffset? suspendedAt,
            double? defaultHourlyRate,
            string defaultCurrency,
            bool onlyAdminsMayCreateProjects,
            bool onlyAdminsSeeBillableRates,
            bool onlyAdminsSeeTeamDashboard,
            bool projectsBillableByDefault,
            int rounding,
            int roundingMinutes,
            string logoUrl,
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
        {
            Id = id;
            ServerDeletedAt = serverDeletedAt;
            At = at;
            Name = name;
            Admin = admin;
            SuspendedAt = suspendedAt;
            DefaultHourlyRate = defaultHourlyRate;
            DefaultCurrency = defaultCurrency;
            OnlyAdminsMayCreateProjects = onlyAdminsMayCreateProjects;
            OnlyAdminsSeeBillableRates = onlyAdminsSeeBillableRates;
            OnlyAdminsSeeTeamDashboard = onlyAdminsSeeTeamDashboard;
            ProjectsBillableByDefault = projectsBillableByDefault;
            Rounding = rounding;
            RoundingMinutes = roundingMinutes;
            LogoUrl = logoUrl;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static WorkspaceDto From(
            IWorkspace entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<string> name = default(New<string>),
            New<bool> admin = default(New<bool>),
            New<DateTimeOffset?> suspendedAt = default(New<DateTimeOffset?>),
            New<double?> defaultHourlyRate = default(New<double?>),
            New<string> defaultCurrency = default(New<string>),
            New<bool> onlyAdminsMayCreateProjects = default(New<bool>),
            New<bool> onlyAdminsSeeBillableRates = default(New<bool>),
            New<bool> onlyAdminsSeeTeamDashboard = default(New<bool>),
            New<bool> projectsBillableByDefault = default(New<bool>),
            New<int> rounding = default(New<int>),
            New<int> roundingMinutes = default(New<int>),
            New<string> logoUrl = default(New<string>))
        => new WorkspaceDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            name: name.ValueOr(entity.Name),
            admin: admin.ValueOr(entity.Admin),
            suspendedAt: suspendedAt.ValueOr(entity.SuspendedAt),
            defaultHourlyRate: defaultHourlyRate.ValueOr(entity.DefaultHourlyRate),
            defaultCurrency: defaultCurrency.ValueOr(entity.DefaultCurrency),
            onlyAdminsMayCreateProjects: onlyAdminsMayCreateProjects.ValueOr(entity.OnlyAdminsMayCreateProjects),
            onlyAdminsSeeBillableRates: onlyAdminsSeeBillableRates.ValueOr(entity.OnlyAdminsSeeBillableRates),
            onlyAdminsSeeTeamDashboard: onlyAdminsSeeTeamDashboard.ValueOr(entity.OnlyAdminsSeeTeamDashboard),
            projectsBillableByDefault: projectsBillableByDefault.ValueOr(entity.ProjectsBillableByDefault),
            rounding: rounding.ValueOr(entity.Rounding),
            roundingMinutes: roundingMinutes.ValueOr(entity.RoundingMinutes),
            logoUrl: logoUrl.ValueOr(entity.LogoUrl),
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);

        public static WorkspaceDto From<T>(
            T entity,
            New<long> id = default(New<long>),
            New<DateTimeOffset?> serverDeletedAt = default(New<DateTimeOffset?>),
            New<DateTimeOffset> at = default(New<DateTimeOffset>),
            New<string> name = default(New<string>),
            New<bool> admin = default(New<bool>),
            New<DateTimeOffset?> suspendedAt = default(New<DateTimeOffset?>),
            New<double?> defaultHourlyRate = default(New<double?>),
            New<string> defaultCurrency = default(New<string>),
            New<bool> onlyAdminsMayCreateProjects = default(New<bool>),
            New<bool> onlyAdminsSeeBillableRates = default(New<bool>),
            New<bool> onlyAdminsSeeTeamDashboard = default(New<bool>),
            New<bool> projectsBillableByDefault = default(New<bool>),
            New<int> rounding = default(New<int>),
            New<int> roundingMinutes = default(New<int>),
            New<string> logoUrl = default(New<string>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : IWorkspace, IDatabaseSyncable
        => new WorkspaceDto(
            id: id.ValueOr(entity.Id),
            serverDeletedAt: serverDeletedAt.ValueOr(entity.ServerDeletedAt),
            at: at.ValueOr(entity.At),
            name: name.ValueOr(entity.Name),
            admin: admin.ValueOr(entity.Admin),
            suspendedAt: suspendedAt.ValueOr(entity.SuspendedAt),
            defaultHourlyRate: defaultHourlyRate.ValueOr(entity.DefaultHourlyRate),
            defaultCurrency: defaultCurrency.ValueOr(entity.DefaultCurrency),
            onlyAdminsMayCreateProjects: onlyAdminsMayCreateProjects.ValueOr(entity.OnlyAdminsMayCreateProjects),
            onlyAdminsSeeBillableRates: onlyAdminsSeeBillableRates.ValueOr(entity.OnlyAdminsSeeBillableRates),
            onlyAdminsSeeTeamDashboard: onlyAdminsSeeTeamDashboard.ValueOr(entity.OnlyAdminsSeeTeamDashboard),
            projectsBillableByDefault: projectsBillableByDefault.ValueOr(entity.ProjectsBillableByDefault),
            rounding: rounding.ValueOr(entity.Rounding),
            roundingMinutes: roundingMinutes.ValueOr(entity.RoundingMinutes),
            logoUrl: logoUrl.ValueOr(entity.LogoUrl),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static WorkspaceDto Clean(IWorkspace entity) => From(entity, SyncStatus.InSync);

        public static WorkspaceDto Dirty(IWorkspace entity) => From(entity, SyncStatus.SyncNeeded);

        public static WorkspaceDto DirtyDeleted(IWorkspace entity) => From(entity, SyncStatus.SyncNeeded, isDeleted: true);

        public static WorkspaceDto Unsyncable(IWorkspace entity, string errorMessage) => From(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        public long Id { get; }
        public DateTimeOffset? ServerDeletedAt { get; }
        public DateTimeOffset At { get; }
        public string Name { get; }
        public bool Admin { get; }
        public DateTimeOffset? SuspendedAt { get; }
        public double? DefaultHourlyRate { get; }
        public string DefaultCurrency { get; }
        public bool OnlyAdminsMayCreateProjects { get; }
        public bool OnlyAdminsSeeBillableRates { get; }
        public bool OnlyAdminsSeeTeamDashboard { get; }
        public bool ProjectsBillableByDefault { get; }
        public int Rounding { get; }
        public int RoundingMinutes { get; }
        public string LogoUrl { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }

    public struct WorkspaceFeatureDto : IWorkspaceFeature
    {
        public WorkspaceFeatureDto(
            WorkspaceFeatureId featureId,
            bool enabled)
        {
            FeatureId = featureId;
            Enabled = enabled;
        }

        public static WorkspaceFeatureDto From(
            IWorkspaceFeature entity,
            New<WorkspaceFeatureId> featureId = default(New<WorkspaceFeatureId>),
            New<bool> enabled = default(New<bool>))
        => new WorkspaceFeatureDto(
            featureId: featureId.ValueOr(entity.FeatureId),
            enabled: enabled.ValueOr(entity.Enabled));

        public WorkspaceFeatureId FeatureId { get; }
        public bool Enabled { get; }
    }

    public struct WorkspaceFeatureCollectionDto : IWorkspaceFeatureCollection, IIdentifiable
    {
        public WorkspaceFeatureCollectionDto(
            long workspaceId,
            IEnumerable<IWorkspaceFeature> features)
        {
            WorkspaceId = workspaceId;
            Features = features;
        }

        public static WorkspaceFeatureCollectionDto From(
            IWorkspaceFeatureCollection entity,
            New<long> workspaceId = default(New<long>),
            New<IEnumerable<IWorkspaceFeature>> features = default(New<IEnumerable<IWorkspaceFeature>>))
        => new WorkspaceFeatureCollectionDto(
            workspaceId: workspaceId.ValueOr(entity.WorkspaceId),
            features: features.ValueOr(entity.Features));

        public long Id => WorkspaceId;
        public long WorkspaceId { get; }
        public IEnumerable<IWorkspaceFeature> Features { get; }
    }
}
