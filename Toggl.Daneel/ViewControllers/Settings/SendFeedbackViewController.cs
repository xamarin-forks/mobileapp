using MvvmCross.Plugin.Color.Platforms.Ios;
using Toggl.Daneel.Extensions;
using Toggl.Daneel.Presentation.Attributes;
using Toggl.Foundation.MvvmCross.Helper;
using Toggl.Foundation.MvvmCross.ViewModels;
using System.Reactive.Linq;
using Toggl.Daneel.Views;
using Toggl.Multivac.Extensions;
using UIKit;

namespace Toggl.Daneel.ViewControllers.Settings
{
    [ModalCardPresentation]
    public sealed partial class SendFeedbackViewController : ReactiveViewController<SendFeedbackViewModel>
    {

        public SendFeedbackViewController()
            : base(nameof(SendFeedbackViewController))
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            prepareViews();
            prepareIndicatorView();

            this.Bind(CloseButton.Rx().Tapped(), ViewModel.CloseButtonTapped);
            this.Bind(FeedbackTextView.Rx().Text(), ViewModel.FeedbackText);
            this.Bind(ErrorView.Rx().Tapped(), ViewModel.ErrorViewTapped);

            this.Bind(SendButton.Rx().Tapped(), ViewModel.SendButtonTapped);
            SendButton.TouchUpInside += (sender, args) => { FeedbackTextView.ResignFirstResponder(); };

            this.Bind(ViewModel.IsFeedbackEmpty, FeedbackPlaceholderTextView.Rx().IsVisible());
            this.Bind(ViewModel.ErrorViewVisible, ErrorView.Rx().AnimatedIsVisible());
            this.Bind(ViewModel.SendEnabled, SendButton.Rx().IsEnabled());

            var isLoading = ViewModel.IsLoading.AsDriver(false);
            this.Bind(isLoading.Invert(), SendButton.Rx().IsVisible());
            this.Bind(isLoading, IndicatorView.Rx().IsVisible());
            this.Bind(isLoading, UIApplication.SharedApplication.Rx().NetworkActivityIndicatorVisible());
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            IndicatorView.StartAnimation();
        }

        private void prepareViews()
        {
            ErrorView.Hidden = true;
            FeedbackTextView.TintColor = Color.Feedback.Cursor.ToNativeColor();
            FeedbackPlaceholderTextView.TintColor = Color.Feedback.Cursor.ToNativeColor();
        }

        private void prepareIndicatorView()
        {
            IndicatorView.Image = IndicatorView.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            IndicatorView.TintColor = Color.Feedback.ActivityIndicator.ToNativeColor();
            IndicatorView.Hidden = true;
        }
    }
}

