using System;
using System.Reactive.Linq;
using Android.App;
using Android.Content;
using Java.Lang;
using Java.Net;
using Toggl.Foundation;
using Toggl.Foundation.Models.Interfaces;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Giskard.Activities;
using Toggl.Giskard.Extensions;
using Uri = Android.Net.Uri;

namespace Toggl.Giskard.Helper
{
    public static class PersistentNotificationsHelper
    {
        private static Uri togglMainNavigationUri = Uri.Parse(ApplicationUrls.Main.Open);
        private static int runningTimeEntryNotificationId = 111;
        private static int idleTimerNotificationId = 112;
        private static char dotSeparator = '\u00b7';

        public static void BindRunningTimeEntry<T>(this T activity,
            NotificationManager notificationManager,
            IObservable<IThreadSafeTimeEntry> runningTimeEntry,
            IObservable<bool> shouldShowRunningTimeEntryNotification)
        where T : Activity, IReactiveBindingHolder
        {
            var runningTimeEntryNotificationSource = runningTimeEntry
                .CombineLatest(shouldShowRunningTimeEntryNotification,
                    (te, shouldShowNotification) => shouldShowNotification ? te : null);
            activity.Bind(runningTimeEntryNotificationSource, te => updateRunningNotification(te, activity, notificationManager));
        }

        public static void BindIdleTimer<T>(this T activity,
            NotificationManager notificationManager,
            IObservable<bool> isTimeEntryRunning,
            IObservable<bool> shouldShowStoppedTimeEntryNotification)
        where T : Activity, IReactiveBindingHolder
        {
            var idleTimerNotificationSource = isTimeEntryRunning
                .CombineLatest(shouldShowStoppedTimeEntryNotification,
                    (isRunning, shouldShowNotification) => shouldShowNotification && !isRunning);
            activity.Bind(idleTimerNotificationSource, shouldShow => updateIdleTimerNotification(shouldShow, activity, notificationManager));
        }

        private static void updateRunningNotification(IThreadSafeTimeEntry timeEntryViewModel, Activity activity, NotificationManager notificationManager)
        {
            if (notificationManager == null) return;

            if (timeEntryViewModel != null)
            {
                var startTime = timeEntryViewModel.Start.ToUnixTimeMilliseconds();
                var timeEntryDescription = string.IsNullOrEmpty(timeEntryViewModel.Description)
                    ? Resources.NoDescription
                    : timeEntryViewModel.Description;
                var projectDetails = extractProjectDetails(timeEntryViewModel);

                var notification = activity.CreateNotificationBuilderWithDefaultChannel(notificationManager)
                    .SetShowWhen(true)
                    .SetUsesChronometer(true)
                    .SetAutoCancel(false)
                    .SetOngoing(true)
                    .SetContentTitle(timeEntryDescription)
                    .SetContentText(projectDetails)
                    .SetWhen(startTime)
                    .SetContentIntent(getIntentFor(activity))
                    .SetSmallIcon(Resource.Drawable.ic_icon_running)
                    .Build();

                notificationManager.Notify(runningTimeEntryNotificationId, notification);
            }
            else
            {
                notificationManager.Cancel(runningTimeEntryNotificationId);
            }
        }

        private static void updateIdleTimerNotification(bool shouldShow, Activity activity, NotificationManager notificationManager)
        {
            if (notificationManager == null) return;

            if (shouldShow)
            {
                var notification = activity.CreateNotificationBuilderWithDefaultChannel(notificationManager)
                    .SetShowWhen(true)
                    .SetAutoCancel(false)
                    .SetOngoing(true)
                    .SetContentTitle(Resources.AppTitle)
                    .SetContentText(Resources.IdleTimerNotification)
                    .SetContentIntent(getIntentFor(activity))
                    .SetSmallIcon(Resource.Drawable.ic_icon_notrunning)
                    .Build();

                notificationManager.Notify(idleTimerNotificationId, notification);
            }
            else
            {
                notificationManager.Cancel(idleTimerNotificationId);
            }
        }

        private static string extractProjectDetails(IThreadSafeTimeEntry timeEntryViewModel)
        {
            if (timeEntryViewModel.ProjectId == null)
            {
                return Resources.NoProject;
            }

            var projectDetails = new StringBuilder(timeEntryViewModel.Project.Name);
            if (timeEntryViewModel.TaskId != null)
            {
                projectDetails.Append($": {timeEntryViewModel.Task.Name}");
            }

            if (timeEntryViewModel.Project.ClientId != null)
            {
                projectDetails.Append($" {dotSeparator} {timeEntryViewModel.Project.Client.Name}");
            }
            return projectDetails.ToString();
        }


        private static PendingIntent getIntentFor(Activity activity)
        {
            var notificationIntent = activity.PackageManager.GetLaunchIntentForPackage(activity.PackageName);
            notificationIntent.SetPackage(null);
            notificationIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);
            return PendingIntent.GetActivity(activity, 0, notificationIntent, 0);
        }
    }
}
