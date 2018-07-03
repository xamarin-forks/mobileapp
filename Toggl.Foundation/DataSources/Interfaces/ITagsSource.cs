using System;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public interface ITagsSource : IDataSource<IThreadSafeTag, IDatabaseTag, TagDto>
    {
        IObservable<IThreadSafeTag> Create(string name, long workspaceId);
    }
}
