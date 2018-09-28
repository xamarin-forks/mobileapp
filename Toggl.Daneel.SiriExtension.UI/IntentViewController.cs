﻿using System;
using CoreGraphics;
using Foundation;
using Intents;
using IntentsUI;
using UIKit;
using CoreFoundation;
using CoreAnimation;
using Toggl.Daneel.Intents;

namespace Toggl.Daneel.SiriExtension.UI
{
    public partial class IntentViewController : UIViewController, IINUIHostedViewControlling
    {

        private UIStringAttributes boldAttributes;
        private UIStringAttributes regularAttributes;

        protected IntentViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        [Export("configureViewForParameters:ofInteraction:interactiveBehavior:context:completion:")]
        public void ConfigureView(
                 NSSet<INParameter> parameters,
                 INInteraction interaction,
                 INUIInteractiveBehavior interactiveBehavior,
                 INUIHostedViewContext context,
                 INUIHostedViewControllingConfigureViewHandler completion)
        {
            var paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.LineSpacing = 2;

            boldAttributes = new UIStringAttributes
            {
                Font = UIFont.BoldSystemFontOfSize(15),
                ForegroundColor = UIColor.Black,
                ParagraphStyle = paragraphStyle
            };

            regularAttributes = new UIStringAttributes
            {
                Font = UIFont.SystemFontOfSize(15),
                ForegroundColor = UIColor.Black,
                ParagraphStyle = paragraphStyle
            };

            foreach (var view in View.Subviews)
            {
                view.RemoveFromSuperview();
            }

            var success = true;
            var desiredSize = CGSize.Empty;

            switch (interaction.Intent)
            {
                case StartTimerIntent startTimerIntent:
                    if (interaction.IntentHandlingStatus == INIntentHandlingStatus.Success)
                    {
                        desiredSize = showStartTimerSuccess(startTimerIntent.EntryDescription);                         
                    }

                    if (interaction.IntentHandlingStatus == INIntentHandlingStatus.Ready)
                    {
                        desiredSize = showConfirmation($"Start {startTimerIntent.EntryDescription}?");
                    }

                    break;
                case StopTimerIntent _:
                    if (interaction.IntentHandlingStatus == INIntentHandlingStatus.Success)
                    {
                        var response = interaction.IntentResponse as StopTimerIntentResponse;
                        if (!(response is null))
                        {
                            desiredSize = showStopResponse(response);
                        }
                    }

                    break;
                default:
                    success = false;
                    break;
            }

            completion(success, parameters, desiredSize);
        }

        private CGSize showStartTimerSuccess(string description)
        {
            entryInfoView.DescriptionLabel.Text = "";
            entryInfoView.TimeLabel.Text = "";

            var attributedString = new NSMutableAttributedString(string.IsNullOrEmpty(description) ? "No Description" : description);
            entryInfoView.DescriptionLabel.AttributedText = attributedString;

            var start = DateTimeOffset.Now;
            var displayLink = CADisplayLink.Create(() => {
                var passed = DateTimeOffset.Now - start;
                entryInfoView.TimeLabel.Text = secondsToString(passed.Seconds);
            });
            displayLink.AddToRunLoop(NSRunLoop.Current, NSRunLoopMode.Default);

            View.AddSubview(entryInfoView);
            var width = this.ExtensionContext?.GetHostedViewMaximumAllowedSize().Width ?? 320;
            var frame = new CGRect(0, 0, width, 60);
            entryInfoView.Frame = frame;

            return frame.Size;
        }

        private CGSize showConfirmation(string confirmationText)
        {
            confirmationView.ConfirmationLabel.Text = "";

            var attributedString = new NSMutableAttributedString(confirmationText, boldAttributes);

            var width = ExtensionContext?.GetHostedViewMaximumAllowedSize().Width ?? 320;
            var boundingRect = attributedString.GetBoundingRect(new CGSize(width - 16 * 2, nfloat.MaxValue),
                NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

            var frame = new CGRect(0, 0, width, boundingRect.Height + 12 * 2);

            confirmationView.ConfirmationLabel.AttributedText = attributedString;
            confirmationView.Frame = frame;

            return frame.Size;
        }

        private CGSize showStopResponse(StopTimerIntentResponse response)
        {
            entryInfoView.TimeLabel.Text = secondsToString(response.EntryDuration.DoubleValue);

            var attributedString = new NSMutableAttributedString("This is an entry with a very long description in three or for or many many lines of text", boldAttributes);

            var startTime = DateTimeOffset.FromUnixTimeSeconds(response.EntryStart.LongValue).ToLocalTime();
            var endTime = DateTimeOffset.FromUnixTimeSeconds(response.EntryStart.LongValue + response.EntryDuration.LongValue).ToLocalTime();
            var fromTime = startTime.ToString("HH:mm");
            var toTime = endTime.ToString("HH:mm");
            var timeFrameString = new NSAttributedString($"\n{fromTime} - {toTime}", regularAttributes);

            attributedString.Append(timeFrameString);
            entryInfoView.DescriptionLabel.AttributedText = attributedString;

            var width = this.ExtensionContext?.GetHostedViewMaximumAllowedSize().Width ?? 320;
            var boundingRect = attributedString.GetBoundingRect(new CGSize(width - 135 - 16 * 2, nfloat.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

            View.AddSubview(entryInfoView);
            var frame = new CGRect(0, 0, width, boundingRect.Height + 12 * 2);
            entryInfoView.Frame = frame;

            return frame.Size;
        }

        [Export("configureWithInteraction:context:completion:")]
        public void Configure(INInteraction interaction, INUIHostedViewContext context, Action<CGSize> completion)
        {
            throw new NotImplementedException();
        }

        private string secondsToString(Double seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
    }
}
