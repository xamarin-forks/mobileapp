using System;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.DTOs
{
    public struct ProjectDto : IProject, IDatabaseSyncable
    {
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

        public ProjectDto(
            long id,
            DateTimeOffset at,
            long workspaceId,
            long? clientId,
            string name,
            bool isPrivate,
            bool active,
            string color,
            bool? billable = null,
            bool? template = null,
            bool? autoEstimates = null,
            long? estimatedHours = null,
            double? rate = null,
            string currency = null,
            int? actualHours = null,
            DateTimeOffset? serverDeletedAt = null,
            SyncStatus syncStatus = SyncStatus.SyncNeeded,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
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

        public static ProjectDto Clean(IProject entity)
            => createFrom(entity, SyncStatus.InSync);

        public static ProjectDto Unsyncable(IProject entity, string errorMessage)
            => createFrom(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        private static ProjectDto createFrom(
            IProject entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
        => new ProjectDto(
            id: entity.Id,
            serverDeletedAt: entity.ServerDeletedAt,
            at: entity.At,
            workspaceId: entity.WorkspaceId,
            clientId: entity.ClientId,
            name: entity.Name,
            isPrivate: entity.IsPrivate,
            active: entity.Active,
            color: entity.Color,
            billable: entity.Billable,
            template: entity.Template,
            autoEstimates: entity.AutoEstimates,
            estimatedHours: entity.EstimatedHours,
            rate: entity.Rate,
            currency: entity.Currency,
            actualHours: entity.ActualHours,
            syncStatus: syncStatus,
            isDeleted: isDeleted,
            lastSyncErrorMessage: lastSyncErrorMessage);
    }
}
