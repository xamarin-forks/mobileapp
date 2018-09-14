using System;
using System.Collections.Generic;
using MvvmCross.Plugin.Color.Platforms.Ios;
using MvvmCross.UI;
using Toggl.Foundation;
using Toggl.Foundation.Calendar;
using Toggl.Foundation.Interactors;
using Toggl.Foundation.MvvmCross.Extensions;
using Toggl.Multivac;
using UIKit;

namespace Toggl.Daneel.ViewControllers.Calendar
{
    public sealed partial class CalendarItemPeekViewController : UIViewController
    {
        private static readonly Dictionary<CalendarIconKind, UIImage> images = new Dictionary<CalendarIconKind, UIImage>
            {
                { CalendarIconKind.Unsynced, templateImage("icUnsynced") },
                { CalendarIconKind.Unsyncable, templateImage("icErrorSmall") }
            };

        private readonly CalendarItem calendarItem;
        private readonly IInteractorFactory interactorFactory;

        public override IUIPreviewActionItem[] PreviewActionItems => new IUIPreviewActionItem[]
        {
            UIPreviewAction.Create(
                Resources.Delete,
                UIPreviewActionStyle.Destructive,
                handleDeletePreviewAction)
        };

        public CalendarItemPeekViewController(CalendarItem calendarItem, IInteractorFactory interactorFactory)
            : base(nameof(CalendarItemPeekViewController), null)
        {
            Ensure.Argument.IsNotNull(interactorFactory, nameof(interactorFactory));

            this.calendarItem = calendarItem;
            this.interactorFactory = interactorFactory;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var foregroundColor = calendarItem.ForegroundColor().ToNativeColor();
            Label.Text = calendarItem.Description;
            Label.TextColor = foregroundColor;
            View.BackgroundColor = MvxColor.ParseHexString(calendarItem.Color).ToNativeColor();

            if (calendarItem.IconKind == CalendarIconKind.None || calendarItem.IconKind == CalendarIconKind.Event)
            {
                ImageView.Hidden = true;
                LabelLeadingConstraint.Constant = 8;
                return;
            }

            ImageView.Image = images[calendarItem.IconKind];
            ImageView.TintColor = foregroundColor;
        }

        private static UIImage templateImage(string iconName)
            => UIImage.FromBundle(iconName)
                  .ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

        private void handleDeletePreviewAction(
            UIPreviewAction action, UIViewController previewViewController)
        {
            if (calendarItem.TimeEntryId == null)
                return;

            interactorFactory.DeleteTimeEntry(calendarItem.TimeEntryId.Value).Execute().Subscribe();
        }
    }
}

