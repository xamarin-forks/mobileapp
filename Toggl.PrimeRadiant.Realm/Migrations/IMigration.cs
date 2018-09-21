namespace Toggl.PrimeRadiant.Realm.Migrations
{
    public interface IMigration
    {
        ulong TargetSchemaVersion { get; }

        void PerformMigration(Realms.Realm oldRealm, Realms.Realm newRealm);
    }
}
