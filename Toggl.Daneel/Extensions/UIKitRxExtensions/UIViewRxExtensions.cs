using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CoreGraphics;
using Toggl.Foundation.MvvmCross.Helper;
using UIKit;

namespace Toggl.Daneel.Extensions
{
    public static partial class UIKitRxExtensions
    {
        public static IObservable<Unit> Tapped<T>(this Reactive<T> reactive) where T : UIView
            => Observable.Create<Unit>(observer =>
            {
                var gestureRecognizer = new UITapGestureRecognizer(() => observer.OnNext(Unit.Default));
                gestureRecognizer.ShouldRecognizeSimultaneously = (recognizer, otherRecognizer) => true;
                reactive.Base.AddGestureRecognizer(gestureRecognizer);

                return Disposable.Create(() => reactive.Base.RemoveGestureRecognizer(gestureRecognizer));
            });

        public static Action<bool> BindIsVisible<T>(this Reactive<T> reactive) where T: UIView
            => isVisible => reactive.Base.Hidden = !isVisible;

        public static Action<nfloat> BindAnimatedAlpha<T>(this Reactive<T> reactive) where T : UIView
            => alpha =>
            {
                AnimationExtensions.Animate(
                    Animation.Timings.EnterTiming,
                    Animation.Curves.EaseIn,
                    () =>
                    {
                        reactive.Base.Alpha = alpha;
                    });
            };

        public static Action<UIColor> BindTintColor<T>(this Reactive<T> reactive) where T : UIView
            => color => reactive.Base.TintColor = color;

        public static Action<bool> BindAnimatedIsVisible<T>(this Reactive<T> reactive) where T : UIView
            => isVisible =>
            {
                reactive.Base.Transform = CGAffineTransform.MakeTranslation(0, 20);

                AnimationExtensions.Animate(
                    Animation.Timings.EnterTiming,
                    Animation.Curves.SharpCurve,
                    () =>
                    {
                        reactive.Base.Hidden = !isVisible;
                        reactive.Base.Transform = CGAffineTransform.MakeTranslation(0, 0);
                    }
                );
            };
    }
}
