using Toggl.Foundation.DataSources;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.ConflictResolution;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public sealed class TasksDataSource
        : DataSource<IThreadSafeTask, IDatabaseTask, TaskDto>, ITasksSource
    {
        public TasksDataSource(IRepository<IDatabaseTask, TaskDto> repository)
            : base(repository)
        {
        }

        protected override IThreadSafeTask Convert(IDatabaseTask entity)
            => Task.From(entity);

        protected override ConflictResolutionMode ResolveConflicts(IDatabaseTask first, TaskDto second)
            => Resolver.ForTasks.Resolve(first, second);
    }
}
