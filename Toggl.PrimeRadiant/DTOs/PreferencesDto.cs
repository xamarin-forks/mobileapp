using Toggl.Multivac;
using Toggl.Multivac.Models;

namespace Toggl.PrimeRadiant.DTOs
{
    public struct PreferencesDto : IPreferences, IDatabaseSyncable, IIdentifiable
    {
        private PreferencesDto(
            TimeFormat timeOfDayFormat,
            DateFormat dateFormat,
            DurationFormat durationFormat,
            bool collapseTimeEntries,
            SyncStatus syncStatus,
            bool isDeleted,
            string lastSyncErrorMessage)
        {
            TimeOfDayFormat = timeOfDayFormat;
            DateFormat = dateFormat;
            DurationFormat = durationFormat;
            CollapseTimeEntries = collapseTimeEntries;
            SyncStatus = syncStatus;
            IsDeleted = isDeleted;
            LastSyncErrorMessage = lastSyncErrorMessage;
        }

        public static PreferencesDto From<T>(
            T entity,
            New<TimeFormat> timeOfDayFormat = default(New<TimeFormat>),
            New<DateFormat> dateFormat = default(New<DateFormat>),
            New<DurationFormat> durationFormat = default(New<DurationFormat>),
            New<bool> collapseTimeEntries = default(New<bool>),
            New<SyncStatus> syncStatus = default(New<SyncStatus>),
            New<bool> isDeleted = default(New<bool>),
            New<string> lastSyncErrorMessage = default(New<string>))
            where T : IPreferences, IDatabaseSyncable
        => new PreferencesDto(
            timeOfDayFormat: timeOfDayFormat.ValueOr(entity.TimeOfDayFormat),
            dateFormat: dateFormat.ValueOr(entity.DateFormat),
            durationFormat: durationFormat.ValueOr(entity.DurationFormat),
            collapseTimeEntries: collapseTimeEntries.ValueOr(entity.CollapseTimeEntries),
            syncStatus: syncStatus.ValueOr(entity.SyncStatus),
            isDeleted: isDeleted.ValueOr(entity.IsDeleted),
            lastSyncErrorMessage: lastSyncErrorMessage.ValueOr(entity.LastSyncErrorMessage));

        public static PreferencesDto Clean(IPreferences entity)
            => createFrom(entity, SyncStatus.InSync);

        public static PreferencesDto Unsyncable(IPreferences entity, string errorMessage)
            => createFrom(entity, SyncStatus.SyncFailed, lastSyncErrorMessage: errorMessage);

        private static PreferencesDto createFrom(
            IPreferences entity,
            SyncStatus syncStatus,
            bool isDeleted = false,
            string lastSyncErrorMessage = null)
            => new PreferencesDto(
                timeOfDayFormat: entity.TimeOfDayFormat,
                dateFormat: entity.DateFormat,
                durationFormat: entity.DurationFormat,
                collapseTimeEntries: entity.CollapseTimeEntries,
                syncStatus: syncStatus,
                isDeleted: isDeleted,
                lastSyncErrorMessage: lastSyncErrorMessage);

        public long Id => 0;
        public TimeFormat TimeOfDayFormat { get; }
        public DateFormat DateFormat { get; }
        public DurationFormat DurationFormat { get; }
        public bool CollapseTimeEntries { get; }
        public SyncStatus SyncStatus { get; }
        public bool IsDeleted { get; }
        public string LastSyncErrorMessage { get; }
    }
}
