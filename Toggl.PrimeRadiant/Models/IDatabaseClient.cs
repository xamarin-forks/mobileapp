using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.Models
{
    public interface IDatabaseClient : IClient, IDatabaseSyncable, IDatabaseModel
    {
        IDatabaseWorkspace Workspace { get; }
    }
}
