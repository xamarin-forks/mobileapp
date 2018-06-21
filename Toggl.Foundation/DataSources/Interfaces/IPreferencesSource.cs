using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant;

namespace Toggl.Foundation.DataSources
{
    public interface IPreferencesSource : ISingletonDataSource<IThreadSafePreferences, PreferencesDto>
    {
    }
}
