using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.ConflictResolution;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public sealed class WorkspacesDataSource : DataSource<IThreadSafeWorkspace, IDatabaseWorkspace, WorkspaceDto>
    {
        public WorkspacesDataSource(IRepository<IDatabaseWorkspace, WorkspaceDto> repository)
            : base(repository)
        {
        }

        protected override IThreadSafeWorkspace Convert(IDatabaseWorkspace entity)
            => Workspace.From(entity);

        protected override ConflictResolutionMode ResolveConflicts(IDatabaseWorkspace first, WorkspaceDto second)
            => Resolver.ForWorkspaces.Resolve(first, second);
    }
}
