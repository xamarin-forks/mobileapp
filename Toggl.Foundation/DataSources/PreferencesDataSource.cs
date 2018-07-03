using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.ConflictResolution;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public sealed class PreferencesDataSource
        : SingletonDataSource<IThreadSafePreferences, IDatabasePreferences, PreferencesDto>, IPreferencesSource
    {
        public PreferencesDataSource(ISingleObjectStorage<IDatabasePreferences, PreferencesDto> storage)
            : base(storage, Preferences.DefaultPreferences)
        {
        }

        protected override IThreadSafePreferences Convert(IDatabasePreferences entity)
            => Preferences.From(entity);

        protected override ConflictResolutionMode ResolveConflicts(IDatabasePreferences first, PreferencesDto second)
            => Resolver.ForPreferences.Resolve(first, second);
    }
}
