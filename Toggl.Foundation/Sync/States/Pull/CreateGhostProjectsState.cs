using System;
using System.Linq;
using System.Reactive.Linq;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Extensions;
using Toggl.Foundation.Helper;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Models;
using Toggl.PrimeRadiant;
using Toggl.Multivac.Extensions;
using static Toggl.Multivac.Extensions.CommonFunctions;

namespace Toggl.Foundation.Sync.States.Pull
{
    public sealed class CreateGhostProjectsState : IPersistState
    {
        private readonly IProjectsSource dataSource;

        private readonly IAnalyticsService analyticsService;

        public StateResult<IFetchObservables> FinishedPersisting { get; } = new StateResult<IFetchObservables>();

        public CreateGhostProjectsState(
            IProjectsSource dataSource,
            IAnalyticsService analyticsService)
        {
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(analyticsService, nameof(analyticsService));

            this.dataSource = dataSource;
            this.analyticsService = analyticsService;
        }

        public IObservable<ITransition> Start(IFetchObservables fetch)
            => fetch.GetList<ITimeEntry>()
                .SingleAsync()
                .SelectMany(Identity)
                .Distinct(timeEntry => timeEntry.ProjectId)
                .WhereAsync(needsGhostProject)
                .SelectMany(createGhostProject)
                .Count()
                .Track(analyticsService.ProjectGhostsCreated)
                .Select(FinishedPersisting.Transition(fetch));

        private IObservable<bool> needsGhostProject(ITimeEntry timeEntry)
            => timeEntry.ProjectId.HasValue
                ? dataSource.GetAll(project => project.Id == timeEntry.ProjectId.Value)
                    .SingleAsync()
                    .Select(projects => projects.None())
                : Observable.Return(false);

        private IObservable<IThreadSafeProject> createGhostProject(ITimeEntry timeEntry)
        {
            var ghost = new ProjectDto(
                id: timeEntry.ProjectId.Value,
                workspaceId: timeEntry.WorkspaceId,
                name: Resources.InaccessibleProject,
                active: false,
                color: Color.NoProject,
                at: default(DateTimeOffset),
                syncStatus: SyncStatus.RefetchingNeeded,
                serverDeletedAt: null,
                clientId: null,
                isPrivate: true,
                billable: false,
                template: null,
                autoEstimates: null,
                estimatedHours: null,
                rate: null,
                currency: null,
                actualHours: null,
                isDeleted: false,
                lastSyncErrorMessage: null);

            return dataSource.Create(ghost);
        }
    }
}
