using System;
using Toggl.Foundation.DTOs;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Sync.ConflictResolution;
using Toggl.Multivac;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public sealed class ProjectsDataSource
        : DataSource<IThreadSafeProject, IDatabaseProject, ProjectDto>, IProjectsSource
    {
        private readonly IIdProvider idProvider;
        private readonly ITimeService timeService;

        public ProjectsDataSource(IIdProvider idProvider, IRepository<IDatabaseProject, ProjectDto> repository, ITimeService timeService)
            : base(repository)
        {
            Ensure.Argument.IsNotNull(idProvider, nameof(idProvider));
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));

            this.idProvider = idProvider;
            this.timeService = timeService;
        }

        public IObservable<IDatabaseProject> Create(CreateProjectDTO dto)
        {
            var project = new ProjectDto(
                id: idProvider.GetNextIdentifier(),
                name: dto.Name,
                color: dto.Color,
                clientId: dto.ClientId,
                billable: dto.Billable,
                workspaceId: dto.WorkspaceId,
                at: timeService.CurrentDateTime,
                syncStatus: SyncStatus.SyncNeeded,
                isDeleted: false,
                lastSyncErrorMessage: null,
                serverDeletedAt: null,
                isPrivate: false,
                active: true,
                template: null,
                autoEstimates: null,
                estimatedHours: null,
                rate: null,
                currency: null,
                actualHours: null);

            return Create(project);
        }

        protected override IThreadSafeProject Convert(IDatabaseProject entity)
            => Project.From(entity);

        protected override ConflictResolutionMode ResolveConflicts(IDatabaseProject first, ProjectDto second)
            => Resolver.ForProjects.Resolve(first, second);
    }
}
