using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Realms;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant.Models;
using Toggl.PrimeRadiant.Realm.Migrations;
using Toggl.PrimeRadiant.Realm.Models;

namespace Toggl.PrimeRadiant.Realm
{
    public sealed class Database : ITogglDatabase
    {
        private readonly RealmConfiguration realmConfiguration;

        private readonly IMigration[] migrations =
        {
            new TagServerDeletedAt()
        };

        public Database()
        {
            realmConfiguration = createRealmConfiguration();
            IdProvider = new IdProvider(getRealmInstance);
            SinceParameters = createSinceParameterRepository();
            Tags = Repository<IDatabaseTag>.For(getRealmInstance, (tag, realm) => new RealmTag(tag, realm));
            Tasks = Repository<IDatabaseTask>.For(getRealmInstance, (task, realm) => new RealmTask(task, realm));
            User = SingleObjectStorage<IDatabaseUser>.For(getRealmInstance, (user, realm) => new RealmUser(user, realm));
            Clients = Repository<IDatabaseClient>.For(getRealmInstance, (client, realm) => new RealmClient(client, realm));
            Preferences = SingleObjectStorage<IDatabasePreferences>.For(getRealmInstance, (preferences, realm) => new RealmPreferences(preferences, realm));
            Projects = Repository<IDatabaseProject>.For(getRealmInstance, (project, realm) => new RealmProject(project, realm));
            TimeEntries = Repository<IDatabaseTimeEntry>.For(getRealmInstance, (timeEntry, realm) => new RealmTimeEntry(timeEntry, realm));
            Workspaces = Repository<IDatabaseWorkspace>.For(getRealmInstance, (workspace, realm) => new RealmWorkspace(workspace, realm));
            WorkspaceFeatures = Repository<IDatabaseWorkspaceFeatureCollection>.For(
                getRealmInstance,
                (collection, realm) => new RealmWorkspaceFeatureCollection(collection, realm),
                id => x => x.WorkspaceId == id,
                features => features.WorkspaceId);
        }

        public IIdProvider IdProvider { get; }
        public ISinceParameterRepository SinceParameters { get; }
        public IRepository<IDatabaseTag> Tags { get; }
        public IRepository<IDatabaseTask> Tasks { get; }
        public IRepository<IDatabaseClient> Clients { get; }
        public ISingleObjectStorage<IDatabasePreferences> Preferences { get; }
        public IRepository<IDatabaseProject> Projects { get; }
        public ISingleObjectStorage<IDatabaseUser> User { get; }
        public IRepository<IDatabaseTimeEntry> TimeEntries { get; }
        public IRepository<IDatabaseWorkspace> Workspaces { get; }
        public IRepository<IDatabaseWorkspaceFeatureCollection> WorkspaceFeatures { get; }

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
                new RealmAdapter<RealmSinceParameter, IDatabaseSinceParameter>(
                    getRealmInstance,
                    (parameter, realm) => new RealmSinceParameter(parameter),
                    id => entity => entity.Id == id,
                    parameter => parameter.Id);

            return new SinceParameterStorage(sinceParametersRealmAdapter);
        }

        private RealmConfiguration createRealmConfiguration()
            => new RealmConfiguration
            {
                SchemaVersion = 6,
                MigrationCallback = (realmMigration, oldSchemaVersion) =>
                    migrations
                        .Where(migration => oldSchemaVersion < migration.TargetSchemaVersion)
                        .OrderBy(migration => migration.TargetSchemaVersion)
                        .Do(ensureApplicabilityOfMigrationFrom(oldSchemaVersion))
                        .ForEach(migration => migration.PerformMigration(realmMigration.OldRealm, realmMigration.NewRealm))
            };

        private Action<IMigration> ensureApplicabilityOfMigrationFrom(ulong oldSchemaVersion)
            => migration =>
            {
                if (oldSchemaVersion < migration.MinimumSchemaVersionToMigrateFrom)
                {
                    throw new InvalidOperationException(
                        $"The old schema is too old: {migration.GetType().FullName} " +
                        $"can be used to migrate from schema versions {migration.MinimumSchemaVersionToMigrateFrom}-{migration.TargetSchemaVersion - 1} " +
                        $"to schema {migration.TargetSchemaVersion}, but it was supposed to migrate from version {oldSchemaVersion}.");
                }
            };
}
}
