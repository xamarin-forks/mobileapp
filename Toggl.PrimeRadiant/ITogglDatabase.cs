using System;
using System.Reactive;
using Toggl.PrimeRadiant.Models;

namespace Toggl.PrimeRadiant
{
    public interface ITogglDatabase
    {
        ISingleObjectStorage<IDatabaseUser, UserDto> User { get; }
        IRepository<IDatabaseClient, ClientDto> Clients { get; }
        IRepository<IDatabaseProject, ProjectDto> Projects { get; }
        ISingleObjectStorage<IDatabasePreferences, PreferencesDto> Preferences { get; }
        IRepository<IDatabaseTag, TagDto> Tags { get; }
        IRepository<IDatabaseTask, TaskDto> Tasks { get; }
        IRepository<IDatabaseTimeEntry, TimeEntryDto> TimeEntries { get; }
        IRepository<IDatabaseWorkspace, WorkspaceDto> Workspaces { get; }
        IRepository<IDatabaseWorkspaceFeatureCollection, WorkspaceFeatureCollectionDto> WorkspaceFeatures { get; }
        IIdProvider IdProvider { get; }
        ISinceParameterRepository SinceParameters { get; }
        IObservable<Unit> Clear();
    }
}
