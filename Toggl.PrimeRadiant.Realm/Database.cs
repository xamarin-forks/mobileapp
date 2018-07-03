using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Realms;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;
using Toggl.PrimeRadiant.Realm.Models;

namespace Toggl.PrimeRadiant.Realm
{
    public sealed class Database : ITogglDatabase
    {
        private readonly RealmConfiguration realmConfiguration;

        public Database()
        {
            realmConfiguration = createRealmConfiguration();
            IdProvider = new IdProvider(getRealmInstance);
            SinceParameters = createSinceParameterRepository();
            Tags = Repository<IDatabaseTag, TagDto>.For(getRealmInstance, (tag, realm) => new RealmTag(tag, realm));
            Tasks = Repository<IDatabaseTask, TaskDto>.For(getRealmInstance, (task, realm) => new RealmTask(task, realm));
            User = SingleObjectStorage<IDatabaseUser, UserDto>.For(getRealmInstance, (user, realm) => new RealmUser(user, realm));
            Clients = Repository<IDatabaseClient, ClientDto>.For(getRealmInstance, (client, realm) => new RealmClient(client, realm));
            Preferences = SingleObjectStorage<IDatabasePreferences, PreferencesDto>.For(getRealmInstance, (preferences, realm) => new RealmPreferences(preferences, realm));
            Projects = Repository<IDatabaseProject, ProjectDto>.For(getRealmInstance, (project, realm) => new RealmProject(project, realm));
            TimeEntries = Repository<IDatabaseTimeEntry, TimeEntryDto>.For(getRealmInstance, (timeEntry, realm) => new RealmTimeEntry(timeEntry, realm));
            Workspaces = Repository<IDatabaseWorkspace, WorkspaceDto>.For(getRealmInstance, (workspace, realm) => new RealmWorkspace(workspace, realm));
            WorkspaceFeatures = Repository<IDatabaseWorkspaceFeatureCollection, WorkspaceFeatureCollectionDto>.For(
                getRealmInstance,
                (collection, realm) => new RealmWorkspaceFeatureCollection(collection, realm),
                id => x => x.WorkspaceId == id,
                features => features.WorkspaceId);
        }

        public IIdProvider IdProvider { get; }
        public ISinceParameterRepository SinceParameters { get; }
        public IRepository<IDatabaseTag, TagDto> Tags { get; }
        public IRepository<IDatabaseTask, TaskDto> Tasks { get; }
        public IRepository<IDatabaseClient, ClientDto> Clients { get; }
        public ISingleObjectStorage<IDatabasePreferences, PreferencesDto> Preferences { get; }
        public IRepository<IDatabaseProject, ProjectDto> Projects { get; }
        public ISingleObjectStorage<IDatabaseUser, UserDto> User { get; }
        public IRepository<IDatabaseTimeEntry, TimeEntryDto> TimeEntries { get; }
        public IRepository<IDatabaseWorkspace, WorkspaceDto> Workspaces { get; }
        public IRepository<IDatabaseWorkspaceFeatureCollection, WorkspaceFeatureCollectionDto> WorkspaceFeatures { get; }

        public IObservable<Unit> Clear() =>
            Observable.Start(() =>
            {
                var realm = getRealmInstance();

                using (var transaction = realm.BeginWrite())
                {
                    realm.RemoveAll();
                    transaction.Commit();
                }
            });

        private Realms.Realm getRealmInstance()
            => Realms.Realm.GetInstance(realmConfiguration);

        private ISinceParameterRepository createSinceParameterRepository()
        {
            var sinceParametersRealmAdapter =
                new RealmAdapter<RealmSinceParameter, IDatabaseSinceParameter, SinceParameterDto>(
                    getRealmInstance,
                    (parameter, realm) => new RealmSinceParameter(parameter),
                    id => entity => entity.Id == id,
                    parameter => parameter.Id);

            return new SinceParameterStorage(sinceParametersRealmAdapter);
        }

        private RealmConfiguration createRealmConfiguration()
            => new RealmConfiguration
            {
                SchemaVersion = 5,
                MigrationCallback = (migration, oldSchemaVersion) =>
                {
                    if (oldSchemaVersion < 3)
                    {
                        // nothing needs explicit updating when updating from schema 0 up to 3
                    }

                    if (oldSchemaVersion < 4)
                    {
                        var newTags = migration.NewRealm.All<RealmTag>();
                        var oldTags = migration.OldRealm.All("RealmTag");
                        for (var i = 0; i < newTags.Count(); i++)
                        {
                            var oldTag = oldTags.ElementAt(i);
                            var newTag = newTags.ElementAt(i);
                            newTag.ServerDeletedAt = oldTag.DeletedAt;
                        }
                    }

                    if (oldSchemaVersion < 5)
                    {
                        // nothing needs explicit updating when updating from schema 4 up to 5
                    }
                }
            };
    }
}
