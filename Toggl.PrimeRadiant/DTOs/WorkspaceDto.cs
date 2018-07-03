using System;
using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant
{
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

        public static WorkspaceDto Clean(IWorkspace entity) => From(entity, SyncStatus.InSync);

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

}
