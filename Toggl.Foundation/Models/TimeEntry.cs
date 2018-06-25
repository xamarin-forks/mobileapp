using System;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Models;

namespace Toggl.Foundation.Models
{
    internal partial class TimeEntry
    {
        public TimeEntry(IDatabaseTimeEntry timeEntry, long duration)
            : this(timeEntry, SyncStatus.SyncNeeded, null)
        {
            if (duration < 0)
                throw new ArgumentOutOfRangeException(nameof(duration), "The duration must be a non-negative number.");

            Duration = duration;
        }
    }

    internal static class TimeEntryExtensions
    {
        public static TimeEntry With(this IDatabaseTimeEntry self, long duration) => new TimeEntry(self, duration);
    }
}
