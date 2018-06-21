using System;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.DTOs;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public interface IProjectsSource
        : IDataSource<IThreadSafeProject, IDatabaseProject, ProjectDto>
    {
        IObservable<IDatabaseProject> Create(CreateProjectDTO dto);
    }
}
