using System;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Sync.ConflictResolution
{
    internal static class Resolver
    {
        public static IConflictResolver<IDatabaseClient, ClientDto> ForClients { get; }
            = new PreferNewer<IDatabaseClient, ClientDto>();

        public static IConflictResolver<IDatabaseProject, ProjectDto> ForProjects { get; }
            = new PreferNewer<IDatabaseProject, ProjectDto>();

        internal static IConflictResolver<IDatabaseUser, UserDto> ForUser { get; }
            = new PreferNewer<IDatabaseUser, UserDto>();

        public static IConflictResolver<IDatabaseWorkspace, WorkspaceDto> ForWorkspaces { get; }
            = new PreferNewer<IDatabaseWorkspace, WorkspaceDto>();

        internal static IConflictResolver<IDatabasePreferences, PreferencesDto> ForPreferences { get; }
            = new OverwriteUnlessNeedsSync<IDatabasePreferences, PreferencesDto>();

        public static IConflictResolver<IDatabaseWorkspaceFeatureCollection, WorkspaceFeatureCollectionDto> ForWorkspaceFeatures { get; }
            = new AlwaysOverwrite<IDatabaseWorkspaceFeatureCollection, WorkspaceFeatureCollectionDto>();

        public static IConflictResolver<IDatabaseTask, TaskDto> ForTasks { get; }
            = new PreferNewer<IDatabaseTask, TaskDto>();

        public static IConflictResolver<IDatabaseTag, TagDto> ForTags { get; }
            = new PreferNewer<IDatabaseTag, TagDto>();

        public static IConflictResolver<IDatabaseTimeEntry, TimeEntryDto> ForTimeEntries { get; }
            = new PreferNewer<IDatabaseTimeEntry, TimeEntryDto>(TimeSpan.FromSeconds(5));
    }
}
