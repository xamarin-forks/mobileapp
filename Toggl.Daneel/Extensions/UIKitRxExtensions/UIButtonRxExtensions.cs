using System;
using System.Reactive;
using System.Reactive.Linq;
using Toggl.Foundation.MvvmCross.Helper;
using Toggl.Multivac.Extensions;
using UIKit;

namespace Toggl.Daneel.Extensions
{
    public static partial class UIKitRxExtensions
    {
        public static IObservable<Unit> Tap<T>(this Reactive<T> reactive)
            where T : UIButton
            => Observable
                .FromEventPattern(e => reactive.Base.TouchUpInside += e, e => reactive.Base.TouchUpInside -= e)
                .SelectUnit();

        public static Action<string> Title<T>(this Reactive<T> reactive)
            where T : UIButton
            => title => reactive.Base.SetTitle(title, UIControlState.Normal);

        public static Action<UIColor> TitleColor<T>(this Reactive<T> reactive)
            where T : UIButton
            => color => reactive.Base.SetTitleColor(color, UIControlState.Normal);

        public static Action<string> AnimatedTitle<T>(this Reactive<T> reactive)
            where T : UIButton
            => title =>
            {
                UIView.Transition(
                    reactive.Base,
                    Animation.Timings.EnterTiming,
                    UIViewAnimationOptions.TransitionCrossDissolve,
                    () => reactive.Base.SetTitle(title, UIControlState.Normal),
                    null
                );
            };

        public static void BindToAction<TInput, TOutput>(this UIButton button, RxAction<TInput, TOutput> action, Func<UIButton, TInput> transform)
        {
            button.Tapped()
                .Select(_ => transform(button))
                .Subscribe(action.Inputs)
                .DisposedBy(action.DisposeBag);

            action.Enabled
                .Subscribe(e => { button.Enabled = e; })
                .DisposedBy(action.DisposeBag);
        }

        public static void BindToAction<TInput, TOutput>(this UIButton button, RxAction<TInput, TOutput> action, TInput input)
        {
            button.BindToAction(action, _ => input);
        }

        public static void BindToAction<TOutput>(this UIButton button, RxAction<Unit, TOutput> action)
        {
            button.BindToAction(action, Unit.Default);
        }

        public static void BindToAction(this UIButton button, UIAction action)
        {
            button.BindToAction(action, Unit.Default);
        }
    }
}
