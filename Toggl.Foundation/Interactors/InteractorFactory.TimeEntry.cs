﻿using System;
using System.Collections.Generic;
using System.Reactive;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DTOs;
using Toggl.Foundation.Models;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.Suggestions;

namespace Toggl.Foundation.Interactors
{
    public sealed partial class InteractorFactory : IInteractorFactory
    {
        public IInteractor<IObservable<IThreadSafeTimeEntry>> CreateTimeEntry(ITimeEntryPrototype prototype)
            => new CreateTimeEntryInteractor(
                idProvider,
                timeService,
                dataSource,
                analyticsService,
                prototype,
                prototype.StartTime,
                prototype.Duration);

        public IInteractor<IObservable<IThreadSafeTimeEntry>> ContinueTimeEntry(ITimeEntryPrototype prototype)
            => new CreateTimeEntryInteractor(
                idProvider,
                timeService,
                dataSource,
                analyticsService,
                prototype,
                timeService.CurrentDateTime,
                null,
                TimeEntryStartOrigin.Continue);

        public IInteractor<IObservable<IThreadSafeTimeEntry>> StartSuggestion(Suggestion suggestion)
            => new CreateTimeEntryInteractor(
                idProvider,
                timeService,
                dataSource,
                analyticsService,
                suggestion,
                timeService.CurrentDateTime,
                null,
            TimeEntryStartOrigin.Suggestion);

        public IInteractor<IObservable<IThreadSafeTimeEntry>> ContinueMostRecentTimeEntry()
            => new ContinueMostRecentTimeEntryInteractor(
                idProvider,
                timeService,
                dataSource,
                analyticsService);
        
        public IInteractor<IObservable<Unit>> DeleteTimeEntry(long id)
            => new DeleteTimeEntryInteractor(timeService, dataSource.TimeEntries, id);
        
        public IInteractor<IObservable<IEnumerable<IThreadSafeTimeEntry>>> GetAllNonDeletedTimeEntries()
            => new GetAllNonDeletedInteractor(dataSource.TimeEntries);

        public IInteractor<IObservable<IThreadSafeTimeEntry>> UpdateTimeEntry(EditTimeEntryDto dto)
            => new UpdateTimeEntryInteractor(timeService, dataSource.TimeEntries, dto);

        public IInteractor<IObservable<IThreadSafeTimeEntry>> StopTimeEntry(DateTimeOffset currentDateTime)
            => new StopTimeEntryInteractor(timeService, dataSource.TimeEntries, currentDateTime);
    }
}
