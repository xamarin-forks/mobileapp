using System;
using Toggl.Foundation.DataSources.Interfaces;
using Toggl.Foundation.Models.Interfaces;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.DataSources
{
    public interface ITimeEntriesSource
        : IObservableDataSource<IThreadSafeTimeEntry, IDatabaseTimeEntry>
    {
        IObservable<IThreadSafeTimeEntry> TimeEntryStarted { get; }
        IObservable<IThreadSafeTimeEntry> TimeEntryStopped { get; }
        IObservable<IThreadSafeTimeEntry> TimeEntryContinued { get; }
        IObservable<IThreadSafeTimeEntry> SuggestionStarted { get; }

        IObservable<IThreadSafeTimeEntry> CurrentlyRunningTimeEntry { get; }

        IObservable<bool> IsEmpty { get; }
    }
}
