using System;
using System.Collections.Generic;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public interface IClientsSource : IDataSource<IThreadSafeClient, IDatabaseClient, ClientDto>
    {
        IObservable<IThreadSafeClient> Create(string name, long workspaceId);

        IObservable<IEnumerable<IThreadSafeClient>> GetAllInWorkspace(long workspaceId);
    }
}
