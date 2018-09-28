﻿using MvvmCross.Navigation;
using NSubstitute;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Interactors;
using Toggl.Foundation.MvvmCross.Services;
using Toggl.Foundation.Services;
using Toggl.Foundation.Shortcuts;
using Toggl.Multivac;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Settings;

namespace Toggl.Foundation.Tests.MvvmCross
{
    public abstract class BaseMvvmCrossTests
    {
        protected IIdProvider IdProvider { get; } = Substitute.For<IIdProvider>();
        protected ITimeService TimeService { get; } = Substitute.For<ITimeService>();
        protected ITogglDataSource DataSource { get; } = Substitute.For<ITogglDataSource>();
        protected IUserPreferences UserPreferences { get; } = Substitute.For<IUserPreferences>();
        protected IAnalyticsService AnalyticsService { get; } = Substitute.For<IAnalyticsService>();
        protected IInteractorFactory InteractorFactory { get; } = Substitute.For<IInteractorFactory>();
        protected IApplicationShortcutCreator ApplicationShortcutCreator { get; }
            = Substitute.For<IApplicationShortcutCreator>();

        protected IForkingNavigationService NavigationService { get; } = Substitute.For<IForkingNavigationService>();
        protected TestSchedulerProvider SchedulerProvider { get; } = new TestSchedulerProvider();
        protected IIntentDonationService IntentDonationService { get; } = Substitute.For<IIntentDonationService>();
        protected IPrivateSharedStorageService PrivateSharedStorageService { get; } = Substitute.For<IPrivateSharedStorageService>();
    }
}
