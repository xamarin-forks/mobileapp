﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Toggl.Foundation.MvvmCross.ViewModels.Settings;
using Toggl.Foundation.Tests.Generators;
using Toggl.Multivac;
using Xunit;

namespace Toggl.Foundation.Tests.MvvmCross.ViewModels
{
    public sealed class UpcomingEventsNotificationSettingsViewModelTests
    {
        public abstract class UpcomingEventsNotificationSettingsViewModelTest : BaseViewModelTests<UpcomingEventsNotificationSettingsViewModel>
        {
            protected override UpcomingEventsNotificationSettingsViewModel CreateViewModel()
                => new UpcomingEventsNotificationSettingsViewModel(NavigationService, UserPreferences);
        }

        public sealed class TheConstructor : UpcomingEventsNotificationSettingsViewModelTest
        {
            [Theory, LogIfTooSlow]
            [ConstructorData]
            public void ThrowsIfAnyOfTheArgumentsIsNull(
                bool useNavigationService,
                bool useUserPreferences)
            {
                Action tryingToConstructWithEmptyParameters =
                    () => new UpcomingEventsNotificationSettingsViewModel(
                        useNavigationService ? NavigationService : null,
                        useUserPreferences ? UserPreferences : null
                    );

                tryingToConstructWithEmptyParameters.Should().Throw<ArgumentNullException>();
            }
        }


        public sealed class TheSelectOptionAction : UpcomingEventsNotificationSettingsViewModelTest
        {
            [Theory, LogIfTooSlow]
            [InlineData(CalendarNotificationsOption.Disabled, false, 0)]
            [InlineData(CalendarNotificationsOption.WhenEventStarts, true, 0)]
            [InlineData(CalendarNotificationsOption.FiveMinutes, true, 5)]
            [InlineData(CalendarNotificationsOption.TenMinutes, true, 10)]
            [InlineData(CalendarNotificationsOption.FifteenMinutes, true, 15)]
            [InlineData(CalendarNotificationsOption.ThirtyMinutes, true, 30)]
            [InlineData(CalendarNotificationsOption.OneHour, true, 60)]
            public async Task SavesTheSelectedOption(CalendarNotificationsOption option, bool enabled, int minutes)
            {
                await ViewModel.SelectOption.Execute(option);
                await ViewModel.Close.Execute(Unit.Default);

                UserPreferences.Received().SetCalendarNotificationsEnabled(enabled);

                if (enabled)
                    UserPreferences.Received().SetTimeSpanBeforeCalendarNotifications(Arg.Is<TimeSpan>(arg => arg == TimeSpan.FromMinutes(minutes)));
                else
                    UserPreferences.DidNotReceive().SetTimeSpanBeforeCalendarNotifications(Arg.Any<TimeSpan>());

                NavigationService.Received().Close(Arg.Any<UpcomingEventsNotificationSettingsViewModel>(), Unit.Default);
            }
        }

        public sealed class TheCloseAction : UpcomingEventsNotificationSettingsViewModelTest
        {
            [Fact, LogIfTooSlow]
            public async Task ClosesTheViewModel()
            {
                await ViewModel.Close.Execute(Unit.Default);

                UserPreferences.DidNotReceive().SetCalendarNotificationsEnabled(Arg.Any<bool>());
                UserPreferences.DidNotReceive().SetTimeSpanBeforeCalendarNotifications(Arg.Any<TimeSpan>());

                NavigationService.Received().Close(Arg.Any<UpcomingEventsNotificationSettingsViewModel>(), Unit.Default);
            }
        }
    }
}
