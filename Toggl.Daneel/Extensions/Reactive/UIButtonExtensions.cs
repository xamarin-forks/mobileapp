﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using Toggl.Foundation.MvvmCross.Helper;
using Toggl.Foundation.MvvmCross.Reactive;
using Toggl.Multivac.Extensions;
using UIKit;

namespace Toggl.Daneel.Extensions.Reactive
{
    public static class UIButtonExtensions
    {
        public static IObservable<Unit> Tap(this IReactive<UIButton> reactive)
            => Observable
                .FromEventPattern(e => reactive.Base.TouchUpInside += e, e => reactive.Base.TouchUpInside -= e)
                .SelectUnit();

        public static Action<string> Title(this IReactive<UIButton> reactive)
            => title => reactive.Base.SetTitle(title, UIControlState.Normal);

        public static Action<UIColor> TitleColor(this IReactive<UIButton> reactive)
            => color => reactive.Base.SetTitleColor(color, UIControlState.Normal);

        public static Action<string> AnimatedTitle(this IReactive<UIButton> reactive)
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

        public static void BindToAction<TInput, TOutput>(this IReactive<UIButton> reactive, RxAction<TInput, TOutput> action, Func<UIButton, TInput> transform)
        {
            reactive.Tap()
                .Select(_ => transform(reactive.Base))
                .Subscribe(action.Inputs)
                .DisposedBy(action.DisposeBag);

            action.Enabled
                .Subscribe(e => { reactive.Base.Enabled = e; })
                .DisposedBy(action.DisposeBag);
        }

        public static void BindToAction<TInput, TOutput>(this IReactive<UIButton> reactive, RxAction<TInput, TOutput> action, TInput input)
        {
            reactive.BindToAction(action, _ => input);
        }

        public static void BindToAction<TOutput>(this IReactive<UIButton> reactive, RxAction<Unit, TOutput> action)
        {
            reactive.BindToAction(action, Unit.Default);
        }

        public static void BindToAction(this IReactive<UIButton> reactive, UIAction action)
        {
            reactive.BindToAction(action, Unit.Default);
        }
    }
}
