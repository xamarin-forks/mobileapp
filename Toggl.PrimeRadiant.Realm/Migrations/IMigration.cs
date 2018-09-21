namespace Toggl.PrimeRadiant.Realm.Migrations
{
    public interface IMigration
    {
        ulong MinimumSchemaVersionToMigrateFrom { get; }

        ulong TargetSchemaVersion { get; }

        void PerformMigration(Realms.Realm oldRealm, Realms.Realm newRealm);
    }
}
