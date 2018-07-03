using Toggl.Foundation.DataSources;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant.DTOs;

namespace Toggl.Foundation.DataSources
{
    public interface IPreferencesSource : ISingletonDataSource<IThreadSafePreferences, PreferencesDto>
    {
    }
}
