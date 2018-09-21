using System.Linq;

namespace Toggl.PrimeRadiant.Realm.Migrations
{
    public sealed class TagServerDeletedAt : IMigration
    {
        public ulong MinimumSchemaVersionToMigrateFrom => 0;

        public ulong TargetSchemaVersion => 4;

        public void PerformMigration(Realms.Realm oldRealm, Realms.Realm newRealm)
        {
            var newTags = newRealm.All<RealmTag>();
            var oldTags = oldRealm.All("RealmTag");
            for (var i = 0; i < newTags.Count(); i++)
            {
                var oldTag = oldTags.ElementAt(i);
                var newTag = newTags.ElementAt(i);
                newTag.ServerDeletedAt = oldTag.DeletedAt;
            }
        }
    }
}
