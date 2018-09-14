
using System;
using System.Reactive.Linq;
using CoreGraphics;
using MvvmCross;
using Toggl.Daneel.Cells.Calendar;
using Toggl.Daneel.Extensions;
using Toggl.Daneel.Presentation.Attributes;
using Toggl.Daneel.ViewControllers.Calendar;
using Toggl.Daneel.Views.Calendar;
using Toggl.Daneel.ViewSources;
using Toggl.Foundation;
using Toggl.Foundation.MvvmCross.Extensions;
using Toggl.Foundation.Calendar;
using Toggl.Foundation.MvvmCross.ViewModels;
using Toggl.Foundation.MvvmCross.ViewModels.Calendar;
using Toggl.Multivac.Extensions;
using UIKit;
using Toggl.Foundation.Interactors;
using Toggl.Multivac;

namespace Toggl.Daneel.ViewControllers
{
    [TabPresentation]
    public sealed partial class CalendarViewController : ReactiveViewController<CalendarViewModel>, IUIViewControllerPreviewingDelegate
    {
        private const int minPeekViewHeight = 50;

        private readonly UIImageView titleImage = new UIImageView(UIImage.FromBundle("togglLogo"));

        private IInteractorFactory interactorFactory;
        private CalendarCollectionViewLayout layout;
        private CalendarCollectionViewSource dataSource;
        private CalendarCollectionViewEditItemHelper editItemHelper;
        private CalendarCollectionViewCreateFromSpanHelper createFromSpanHelper;

        private readonly UIButton settingsButton = new UIButton(new CGRect(0, 0, 40, 50));

        public CalendarViewController()
            : base(nameof(CalendarViewController))
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            interactorFactory = Mvx.Resolve<IInteractorFactory>();
            RegisterForPreviewingWithDelegate(this, CalendarCollectionView);

            settingsButton.SetImage(UIImage.FromBundle("icSettings"), UIControlState.Normal);

            ViewModel
                .ShouldShowOnboarding
                .FirstAsync()
                .Subscribe(
                    shouldShowOnboarding => OnboardingView.Alpha = shouldShowOnboarding ? 1: 0)
                .DisposedBy(DisposeBag);

            this.Bind(ViewModel.ShouldShowOnboarding, OnboardingView.BindIsVisibleWithFade());
            this.Bind(GetStartedButton.Tapped(), ViewModel.GetStartedAction);

            var timeService = Mvx.Resolve<ITimeService>();

            dataSource = new CalendarCollectionViewSource(
                CalendarCollectionView,
                ViewModel.Date,
                ViewModel.TimeOfDayFormat,
                ViewModel.CalendarItems);

            layout = new CalendarCollectionViewLayout(timeService, dataSource);

            editItemHelper = new CalendarCollectionViewEditItemHelper(CalendarCollectionView, dataSource, layout);
            createFromSpanHelper = new CalendarCollectionViewCreateFromSpanHelper(CalendarCollectionView, dataSource, layout);

            CalendarCollectionView.SetCollectionViewLayout(layout, false);
            CalendarCollectionView.Delegate = dataSource;
            CalendarCollectionView.DataSource = dataSource;
            CalendarCollectionView.ContentInset = new UIEdgeInsets(20, 0, 20, 0);

            this.Bind(dataSource.ItemTapped, ViewModel.OnItemTapped);
            this.Bind(settingsButton.Tapped(), ViewModel.SelectCalendars);
            this.Bind(editItemHelper.EditCalendarItem, ViewModel.OnUpdateTimeEntry);
            this.Bind(ViewModel.SettingsAreVisible , settingsButton.BindIsVisible());
            this.Bind(createFromSpanHelper.CreateFromSpan, ViewModel.OnDurationSelected);

            CalendarCollectionView.LayoutIfNeeded();
            var currentTimeY = layout.FrameForCurrentTime().Y;
            var scrollPointY = currentTimeY - View.Frame.Height / 2;
            var currentTimePoint = new CGPoint(0, scrollPointY.Clamp(0, CalendarCollectionView.ContentSize.Height));
            CalendarCollectionView.SetContentOffset(currentTimePoint, false);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationItem.TitleView = titleImage;
            NavigationItem.RightBarButtonItems = new[]
            {
                new UIBarButtonItem(settingsButton)
            };
        }

        public UIViewController GetViewControllerForPreview(IUIViewControllerPreviewing previewingContext, CGPoint location)
        {
            var index = CalendarCollectionView.IndexPathForItemAtPoint(location);
            if (index == null)
                return null;

            var cell = CalendarCollectionView.CellForItem(index);
            var calendarItemView = cell as CalendarItemView;
            if (calendarItemView == null)
                return null;

            if (calendarItemView.Item.Source == CalendarItemSource.Calendar)
                return null;

            previewingContext.SourceRect = cell.Frame;

            return new CalendarItemPeekViewController(calendarItemView.Item, interactorFactory)
            {
                PreferredContentSize = sizeForPeekView(cell.Frame.Size)
            };
        }

        public void CommitViewController(IUIViewControllerPreviewing previewingContext, UIViewController viewControllerToCommit)
        {
        }

        private CGSize sizeForPeekView(CGSize cellSize)
            => new CGSize(
                0, //Width is set by iOSc
                cellSize.Height < minPeekViewHeight
                    ? minPeekViewHeight
                    : cellSize.Height
            );
    }
}
