using System;
using System.Reactive;
using Toggl.Foundation.DTOs;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant.DTOs;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public interface ITimeEntriesSource
        : IObservableDataSource<IThreadSafeTimeEntry, IDatabaseTimeEntry, TimeEntryDto>
    {
        IObservable<IThreadSafeTimeEntry> TimeEntryStarted { get; }
        IObservable<IThreadSafeTimeEntry> TimeEntryStopped { get; }
        IObservable<IThreadSafeTimeEntry> TimeEntryContinued { get; }
        IObservable<IThreadSafeTimeEntry> SuggestionStarted { get; }

        IObservable<IThreadSafeTimeEntry> CurrentlyRunningTimeEntry { get; }

        IObservable<bool> IsEmpty { get; }

        IObservable<Unit> SoftDelete(IThreadSafeTimeEntry timeEntry);

        IObservable<IThreadSafeTimeEntry> Stop(DateTimeOffset stopTime);

        IObservable<IThreadSafeTimeEntry> Update(EditTimeEntryDto dto);
    }
}
