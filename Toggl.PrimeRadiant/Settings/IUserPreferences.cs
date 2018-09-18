using System;

namespace Toggl.PrimeRadiant.Settings
{
    public interface IUserPreferences
    {
        IObservable<bool> IsShowingSuggestionsObservable { get; }

        IObservable<bool> IsManualModeEnabledObservable { get; }

        IObservable<bool> AreRunningTimerNotificationsEnabledObservable { get; }

        IObservable<bool> AreStoppedTimerNotificationsEnabledObservable { get; }

        bool IsManualModeEnabled { get; }

        bool AreRunningTimerNotificationsEnabled { get; }

        bool AreStoppedTimerNotificationsEnabled { get; }

        bool IsShowingSuggestions { get; }

        void EnableManualMode();

        void EnableTimerMode();

        void SetIsShowingSuggestions(bool state);

        void SetRunningTimerNotifications(bool state);

        void SetStoppedTimerNotifications(bool state);

        void Reset();
    }
}
