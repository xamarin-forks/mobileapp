using System;
using System.Reactive.Linq;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Extensions;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Interactors
{
    public class ContinueMostRecentTimeEntryInteractor : IInteractor<IObservable<IThreadSafeTimeEntry>>
    {
        private readonly IIdProvider idProvider;
        private readonly ITimeService timeService;
        private readonly ITogglDataSource dataSource;
        private readonly IAnalyticsService analyticsService;

        public ContinueMostRecentTimeEntryInteractor(
            IIdProvider idProvider,
            ITimeService timeService,
            ITogglDataSource dataSource,
            IAnalyticsService analyticsService)
        {
            Ensure.Argument.IsNotNull(idProvider, nameof(idProvider));
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));
            Ensure.Argument.IsNotNull(analyticsService, nameof(analyticsService));

            this.idProvider = idProvider;
            this.dataSource = dataSource;
            this.timeService = timeService;
            this.analyticsService = analyticsService;
        }

        public IObservable<IThreadSafeTimeEntry> Execute()
            => dataSource.TimeEntries
                .GetAll(te => !te.IsDeleted)
                .Select(timeEntries => timeEntries.MaxBy(te => te.Start))
                .Select(newTimeEntry)
                .SelectMany(dataSource.TimeEntries.Create)
                .Do(_ => dataSource.SyncManager.PushSync())
                .Track(StartTimeEntryEvent.With(TimeEntryStartOrigin.ContinueMostRecent), analyticsService);

        private TimeEntryDto newTimeEntry(IThreadSafeTimeEntry timeEntry)
            => TimeEntryDto.From<IThreadSafeTimeEntry>(
                timeEntry,
                id: idProvider.GetNextIdentifier(),
                start: timeService.CurrentDateTime,
                syncStatus: SyncStatus.SyncNeeded,
                at: timeService.CurrentDateTime);
    }
}
