using System.Linq;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Models
{
    internal partial class Project
    {
        private Project(IDatabaseProject entity)
            : this(entity as IProject, entity.SyncStatus, entity.LastSyncErrorMessage, entity.IsDeleted)
        {
            Client = entity.Client == null ? null : Models.Client.From(entity.Client);
            Workspace = entity.Workspace == null ? null : Models.Workspace.From(entity.Workspace);
            Tasks = entity.Tasks == null ? null : entity.Tasks.Select(Models.Task.From);
            SyncStatus = entity.SyncStatus;
            LastSyncErrorMessage = entity.LastSyncErrorMessage;
            IsDeleted = entity.IsDeleted;
        }

        public static Project From(IDatabaseProject entity)
            => new Project(entity);

        private Project(IProject entity, SyncStatus syncStatus, string lastSyncErrorMessage, bool isDeleted = false)
        {
            Id = entity.Id;
            WorkspaceId = entity.WorkspaceId;
            ClientId = entity.ClientId;
            Name = entity.Name;
            IsPrivate = entity.IsPrivate;
            Active = entity.Active;
            Color = entity.Color;
            Billable = entity.Billable;
            Template = entity.Template;
            AutoEstimates = entity.AutoEstimates;
            EstimatedHours = entity.EstimatedHours;
            Rate = entity.Rate;
            Currency = entity.Currency;
            ActualHours = entity.ActualHours;
            At = entity.At;
            ServerDeletedAt = entity.ServerDeletedAt;
            SyncStatus = syncStatus;
            LastSyncErrorMessage = lastSyncErrorMessage;
            IsDeleted = isDeleted;
        }
    }

    internal partial class Tag
    {
        private Tag(IDatabaseTag entity)
            : this(entity as ITag, entity.SyncStatus, entity.LastSyncErrorMessage, entity.IsDeleted)
        {
            Workspace = entity.Workspace == null ? null : Models.Workspace.From(entity.Workspace);
            SyncStatus = entity.SyncStatus;
            LastSyncErrorMessage = entity.LastSyncErrorMessage;
            IsDeleted = entity.IsDeleted;
        }

        public static Tag From(IDatabaseTag entity)
            => new Tag(entity);

        private Tag(ITag entity, SyncStatus syncStatus, string lastSyncErrorMessage, bool isDeleted = false)
        {
            Id = entity.Id;
            WorkspaceId = entity.WorkspaceId;
            Name = entity.Name;
            At = entity.At;
            ServerDeletedAt = entity.ServerDeletedAt;
            SyncStatus = syncStatus;
            LastSyncErrorMessage = lastSyncErrorMessage;
            IsDeleted = isDeleted;
        }
    }

    internal partial class Task
    {
        private Task(IDatabaseTask entity)
            : this(entity as ITask, entity.SyncStatus, entity.LastSyncErrorMessage, entity.IsDeleted)
        {
            User = entity.User == null ? null : Models.User.From(entity.User);
            Project = entity.Project == null ? null : Models.Project.From(entity.Project);
            Workspace = entity.Workspace == null ? null : Models.Workspace.From(entity.Workspace);
            SyncStatus = entity.SyncStatus;
            LastSyncErrorMessage = entity.LastSyncErrorMessage;
            IsDeleted = entity.IsDeleted;
        }

        public static Task From(IDatabaseTask entity)
            => new Task(entity);

        private Task(ITask entity, SyncStatus syncStatus, string lastSyncErrorMessage, bool isDeleted = false)
        {
            Id = entity.Id;
            Name = entity.Name;
            ProjectId = entity.ProjectId;
            WorkspaceId = entity.WorkspaceId;
            UserId = entity.UserId;
            EstimatedSeconds = entity.EstimatedSeconds;
            Active = entity.Active;
            TrackedSeconds = entity.TrackedSeconds;
            At = entity.At;
            SyncStatus = syncStatus;
            LastSyncErrorMessage = lastSyncErrorMessage;
            IsDeleted = isDeleted;
        }
    }

    internal partial class TimeEntry
    {
        private TimeEntry(IDatabaseTimeEntry entity)
            : this(entity as ITimeEntry, entity.SyncStatus, entity.LastSyncErrorMessage, entity.IsDeleted)
        {
            Task = entity.Task == null ? null : Models.Task.From(entity.Task);
            User = entity.User == null ? null : Models.User.From(entity.User);
            Project = entity.Project == null ? null : Models.Project.From(entity.Project);
            Workspace = entity.Workspace == null ? null : Models.Workspace.From(entity.Workspace);
            Tags = entity.Tags == null ? null : entity.Tags.Select(Models.Tag.From);
            SyncStatus = entity.SyncStatus;
            LastSyncErrorMessage = entity.LastSyncErrorMessage;
            IsDeleted = entity.IsDeleted;
        }

        public static TimeEntry From(IDatabaseTimeEntry entity)
            => new TimeEntry(entity);

        public static TimeEntry From<T>(T entity)
            where T : ITimeEntry, IDatabaseSyncable
            => new TimeEntry(entity, entity.SyncStatus, entity.LastSyncErrorMessage, entity.IsDeleted);

        private TimeEntry(ITimeEntry entity, SyncStatus syncStatus, string lastSyncErrorMessage, bool isDeleted = false)
        {
            Id = entity.Id;
            WorkspaceId = entity.WorkspaceId;
            ProjectId = entity.ProjectId;
            TaskId = entity.TaskId;
            Billable = entity.Billable;
            Start = entity.Start;
            Duration = entity.Duration;
            Description = entity.Description;
            TagIds = entity.TagIds;
            UserId = entity.UserId;
            At = entity.At;
            ServerDeletedAt = entity.ServerDeletedAt;
            SyncStatus = syncStatus;
            LastSyncErrorMessage = lastSyncErrorMessage;
            IsDeleted = isDeleted;
        }
    }

    internal partial class User
    {
        private User(IDatabaseUser entity)
            : this(entity as IUser, entity.SyncStatus, entity.LastSyncErrorMessage, entity.IsDeleted)
        {
            SyncStatus = entity.SyncStatus;
            LastSyncErrorMessage = entity.LastSyncErrorMessage;
            IsDeleted = entity.IsDeleted;
        }

        public static User From(IDatabaseUser entity)
            => new User(entity);

        public static User From<T>(T entity)
            where T : IUser, IDatabaseSyncable
            => new User(entity, entity.SyncStatus, entity.LastSyncErrorMessage, entity.IsDeleted);

        private User(IUser entity, SyncStatus syncStatus, string lastSyncErrorMessage, bool isDeleted = false)
        {
            Id = entity.Id;
            ApiToken = entity.ApiToken;
            DefaultWorkspaceId = entity.DefaultWorkspaceId;
            Email = entity.Email;
            Fullname = entity.Fullname;
            BeginningOfWeek = entity.BeginningOfWeek;
            Language = entity.Language;
            ImageUrl = entity.ImageUrl;
            At = entity.At;
            SyncStatus = syncStatus;
            LastSyncErrorMessage = lastSyncErrorMessage;
            IsDeleted = isDeleted;
        }
    }

    internal partial class Workspace
    {
        private Workspace(IDatabaseWorkspace entity)
            : this(entity as IWorkspace, entity.SyncStatus, entity.LastSyncErrorMessage, entity.IsDeleted)
        {
            SyncStatus = entity.SyncStatus;
            LastSyncErrorMessage = entity.LastSyncErrorMessage;
            IsDeleted = entity.IsDeleted;
        }

        public static Workspace From(IDatabaseWorkspace entity)
            => new Workspace(entity);

        private Workspace(IWorkspace entity, SyncStatus syncStatus, string lastSyncErrorMessage, bool isDeleted = false)
        {
            Id = entity.Id;
            Name = entity.Name;
            Admin = entity.Admin;
            SuspendedAt = entity.SuspendedAt;
            DefaultHourlyRate = entity.DefaultHourlyRate;
            DefaultCurrency = entity.DefaultCurrency;
            OnlyAdminsMayCreateProjects = entity.OnlyAdminsMayCreateProjects;
            OnlyAdminsSeeBillableRates = entity.OnlyAdminsSeeBillableRates;
            OnlyAdminsSeeTeamDashboard = entity.OnlyAdminsSeeTeamDashboard;
            ProjectsBillableByDefault = entity.ProjectsBillableByDefault;
            Rounding = entity.Rounding;
            RoundingMinutes = entity.RoundingMinutes;
            LogoUrl = entity.LogoUrl;
            At = entity.At;
            ServerDeletedAt = entity.ServerDeletedAt;
            SyncStatus = syncStatus;
            LastSyncErrorMessage = lastSyncErrorMessage;
            IsDeleted = isDeleted;
        }
    }

    internal partial class WorkspaceFeature
    {
        private WorkspaceFeature(IDatabaseWorkspaceFeature entity)
            : this(entity as IWorkspaceFeature)
        {
        }

        public static WorkspaceFeature From(IDatabaseWorkspaceFeature entity)
            => new WorkspaceFeature(entity);

        private WorkspaceFeature(IWorkspaceFeature entity)
        {
            FeatureId = entity.FeatureId;
            Enabled = entity.Enabled;
        }

    }

    internal partial class WorkspaceFeatureCollection
    {
        private WorkspaceFeatureCollection(IDatabaseWorkspaceFeatureCollection entity)
            : this(entity as IWorkspaceFeatureCollection)
        {
            Workspace = entity.Workspace == null ? null : Models.Workspace.From(entity.Workspace);
            DatabaseFeatures = entity.DatabaseFeatures == null ? null : entity.DatabaseFeatures.Select(Models.WorkspaceFeature.From);
        }

        public static WorkspaceFeatureCollection From(IDatabaseWorkspaceFeatureCollection entity)
            => new WorkspaceFeatureCollection(entity);

        private WorkspaceFeatureCollection(IWorkspaceFeatureCollection entity)
        {
            WorkspaceId = entity.WorkspaceId;
            Features = entity.Features;
        }

    }
}
