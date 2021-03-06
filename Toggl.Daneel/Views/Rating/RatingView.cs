﻿using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CoreGraphics;
using Foundation;
using MvvmCross.Commands;
using MvvmCross.Platforms.Ios.Binding.Views;
using ObjCRuntime;
using Toggl.Daneel.Extensions;
using Toggl.Daneel.Extensions.Reactive;
using Toggl.Foundation.MvvmCross.ViewModels;
using UIKit;
using static Toggl.Multivac.Extensions.CommonFunctions;

namespace Toggl.Daneel
{
    public partial class RatingView : MvxView, IReactiveBindingHolder
    {
        private readonly UIStringAttributes descriptionStringAttributes = new UIStringAttributes
        {
            ParagraphStyle = new NSMutableParagraphStyle
            {
                MaximumLineHeight = 22,
                MinimumLineHeight = 22,
                Alignment = UITextAlignment.Center,
            }
        };

        private NSLayoutConstraint heightConstraint;

        public IMvxCommand CTATappedCommand { get; set; }
        public IMvxCommand DismissTappedCommand { get; set; }
        public IMvxCommand<bool> ImpressionTappedCommand { get; set; }

        public new RatingViewModel DataContext
        {
            get => base.DataContext as RatingViewModel;
            set
            {
                base.DataContext = value;
                updateBindings();
            }
        }

        public CompositeDisposable DisposeBag { get; } = new CompositeDisposable();

        public RatingView (IntPtr handle) : base (handle)
        {
        }

        public static RatingView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(RatingView), null, null);
            return Runtime.GetNSObject<RatingView>(arr.ValueAt(0));
        }

        private void updateBindings()
        {
            this.Bind(DataContext.CtaTitle, CtaTitle.Rx().Text());
            this.Bind(DataContext.CtaButtonTitle, CtaButton.Rx().Title());
            this.Bind(
                DataContext.Impression.Select(impression => impression.HasValue),
                CtaView.Rx().IsVisibleWithFade());
            this.Bind(
                DataContext.CtaDescription.Select(attributedDescription),
                CtaDescription.Rx().AttributedText());
            this.Bind(
                DataContext
                    .Impression
                    .Select(impression => impression.HasValue)
                    .Select(Invert),
                QuestionView.Rx().IsVisibleWithFade());

            this.Bind(
                DataContext
                    .Impression
                    .Select(impression => impression.HasValue),
                CtaViewBottomConstraint.Rx().Active());

            this.Bind(
                DataContext
                    .Impression
                    .Select(impression => impression.HasValue)
                    .Select(Invert),
                QuestionViewBottomConstraint.Rx().Active());

            this.BindVoid(YesView.Rx().Tap(), () => DataContext.RegisterImpression(true));
            this.BindVoid(NotReallyView.Rx().Tap(), () => DataContext.RegisterImpression(false));
            this.Bind(CtaButton.Rx().Tap(), DataContext.PerformMainAction);
            this.BindVoid(DismissButton.Rx().Tap(), DataContext.Dismiss);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            DisposeBag.Dispose();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            SetupAsCard(QuestionView);
            SetupAsCard(CtaView);

            CtaButton.Layer.CornerRadius = 8;
            CtaView.Layer.MasksToBounds = false;
        }

        private NSAttributedString attributedDescription(string text)
            => new NSAttributedString(text, descriptionStringAttributes);

        private void SetupAsCard(UIView view)
        {
            var shadowPath = UIBezierPath.FromRect(view.Bounds);
            view.Layer.ShadowPath?.Dispose();
            view.Layer.ShadowPath = shadowPath.CGPath;

            view.Layer.CornerRadius = 8;
            view.Layer.ShadowRadius = 4;
            view.Layer.ShadowOpacity = 0.1f;
            view.Layer.MasksToBounds = false;
            view.Layer.ShadowOffset = new CGSize(0, 2);
            view.Layer.ShadowColor = UIColor.Black.CGColor;
        }
    }
}
