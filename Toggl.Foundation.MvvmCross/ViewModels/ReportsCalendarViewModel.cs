﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PropertyChanged;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.MvvmCross.Parameters;
using Toggl.Foundation.MvvmCross.Services;
using Toggl.Foundation.MvvmCross.ViewModels.Calendar;
using Toggl.Foundation.MvvmCross.ViewModels.Calendar.QuickSelectShortcuts;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;

namespace Toggl.Foundation.MvvmCross.ViewModels
{
    [Preserve(AllMembers = true)]
    public sealed class ReportsCalendarViewModel : MvxViewModel
    {
        private const int monthsToShow = 13;

        //Fields
        private readonly ITimeService timeService;
        private readonly IDialogService dialogService;
        private readonly ITogglDataSource dataSource;
        private readonly ISubject<ReportsDateRangeParameter> selectedDateRangeSubject = new Subject<ReportsDateRangeParameter>();
        private readonly string[] dayHeaders =
        {
            Resources.SundayInitial,
            Resources.MondayInitial,
            Resources.TuesdayInitial,
            Resources.WednesdayInitial,
            Resources.ThursdayInitial,
            Resources.FridayInitial,
            Resources.SaturdayInitial
        };

        private CalendarMonth initialMonth;
        private CalendarDayViewModel startOfSelection;

        private CompositeDisposable disposableBag;


        public BeginningOfWeek BeginningOfWeek { get; private set; }

        //Properties
        [DependsOn(nameof(CurrentPage))]
        public CalendarMonth CurrentMonth => convertPageIndexTocalendarMonth(CurrentPage);

        public int CurrentPage { get; set; } = monthsToShow - 1;

        [DependsOn(nameof(Months), nameof(CurrentPage))]
        public int RowsInCurrentMonth => Months[CurrentPage].RowCount;

        public List<CalendarPageViewModel> Months { get; } = new List<CalendarPageViewModel>();

        public IObservable<ReportsDateRangeParameter> SelectedDateRangeObservable
            => selectedDateRangeSubject.AsObservable();

        public List<CalendarBaseQuickSelectShortcut> QuickSelectShortcuts { get; private set; }

        public IMvxCommand<CalendarDayViewModel> CalendarDayTappedCommand { get; }

        public IMvxCommand<CalendarBaseQuickSelectShortcut> QuickSelectCommand { get; }

        public ReportsCalendarViewModel(
            ITimeService timeService, IDialogService dialogService, ITogglDataSource dataSource)
        {
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));
            Ensure.Argument.IsNotNull(dialogService, nameof(dialogService));
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));

            this.timeService = timeService;
            this.dialogService = dialogService;
            this.dataSource = dataSource;

            CalendarDayTappedCommand = new MvxCommand<CalendarDayViewModel>(calendarDayTapped);
            QuickSelectCommand = new MvxCommand<CalendarBaseQuickSelectShortcut>(quickSelect);

            disposableBag = new CompositeDisposable();
        }

        private void calendarDayTapped(CalendarDayViewModel tappedDay)
        {
            if (startOfSelection == null)
            {
                var date = tappedDay.DateTimeOffset;

                var dateRange = ReportsDateRangeParameter
                    .WithDates(date, date)
                    .WithSource(ReportsSource.Calendar);
                startOfSelection = tappedDay;
                highlightDateRange(dateRange);
            }
            else
            {
                var startDate = startOfSelection.DateTimeOffset;
                var endDate = tappedDay.DateTimeOffset;

                // if (System.Math.Abs((endDate - startDate).Days) > 365)
                if (System.Math.Abs((endDate - startDate).Days) > 4)
                {
                    dialogService.Alert("Too long", "You can do a year max", "Okay, lol", true).First();
                }
                else
                {
                    var dateRange = ReportsDateRangeParameter
                        .WithDates(startDate, endDate)
                        .WithSource(ReportsSource.Calendar);
                    startOfSelection = null;
                    changeDateRange(dateRange);
                }
            }
        }

        public override void Prepare()
        {
            base.Prepare();

            var now = timeService.CurrentDateTime;
            initialMonth = new CalendarMonth(now.Year, now.Month).AddMonths(-(monthsToShow - 1));
        }

        public override async Task Initialize()
        {
            await base.Initialize();

            BeginningOfWeek = (await dataSource.User.Current.FirstAsync()).BeginningOfWeek;
            fillMonthArray();
            RaisePropertyChanged(nameof(CurrentMonth));

            QuickSelectShortcuts = createQuickSelectShortcuts();

            QuickSelectShortcuts
                .Select(quickSelectShortcut => SelectedDateRangeObservable.Subscribe(
                    quickSelectShortcut.OnDateRangeChanged))
                .ForEach(disposableBag.Add);

            var initialShortcut = QuickSelectShortcuts.Single(shortcut => shortcut is CalendarThisWeekQuickSelectShortcut);
            changeDateRange(initialShortcut.GetDateRange().WithSource(ReportsSource.Initial));
        }

        public void OnToggleCalendar() => selectStartOfSelectionIfNeeded();

        public void OnHideCalendar() => selectStartOfSelectionIfNeeded();

        public string DayHeaderFor(int index)
            => dayHeaders[(index + (int)BeginningOfWeek + 7) % 7];

        private void selectStartOfSelectionIfNeeded()
        {
            if (startOfSelection == null) return;

            var date = startOfSelection.DateTimeOffset;
            var dateRange = ReportsDateRangeParameter
                .WithDates(date, date)
                .WithSource(ReportsSource.Calendar);
            changeDateRange(dateRange);
        }

        private void fillMonthArray()
        {
            var monthIterator = initialMonth;
            for (int i = 0; i < monthsToShow; i++, monthIterator = monthIterator.Next())
                Months.Add(new CalendarPageViewModel(monthIterator, BeginningOfWeek, timeService.CurrentDateTime));
        }

        private List<CalendarBaseQuickSelectShortcut> createQuickSelectShortcuts()
            => new List<CalendarBaseQuickSelectShortcut>
            {
                new CalendarTodayQuickSelectShortcut(timeService),
                new CalendarYesterdayQuickSelectShortcut(timeService),
                new CalendarThisWeekQuickSelectShortcut(timeService, BeginningOfWeek),
                new CalendarLastWeekQuickSelectShortcut(timeService, BeginningOfWeek),
                new CalendarThisMonthQuickSelectShortcut(timeService),
                new CalendarLastMonthQuickSelectShortcut(timeService),
                new CalendarThisYearQuickSelectShortcut(timeService)
            };

        private CalendarMonth convertPageIndexTocalendarMonth(int pageIndex)
            => initialMonth.AddMonths(pageIndex);

        private void changeDateRange(ReportsDateRangeParameter newDateRange)
        {
            startOfSelection = null;

            highlightDateRange(newDateRange);

            selectedDateRangeSubject.OnNext(newDateRange);
        }

        private void quickSelect(CalendarBaseQuickSelectShortcut quickSelectShortCut)
            => changeDateRange(quickSelectShortCut.GetDateRange());

        private void highlightDateRange(ReportsDateRangeParameter dateRange)
        {
            Months.ForEach(month => month.Days.ForEach(day => day.OnSelectedRangeChanged(dateRange)));
        }
    }
}
