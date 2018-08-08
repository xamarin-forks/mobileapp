using System;
using System.Reactive.Linq;
using UIKit;

namespace Toggl.Daneel.Extensions
{
    public static partial class UIKitRxExtensions
    {
        public static IObservable<DateTimeOffset> Date<T>(this Reactive<T> reactive) where T : UIDatePicker
            => Observable
                .FromEventPattern(e => reactive.Base.ValueChanged += e, e => reactive.Base.ValueChanged -= e)
                .Select(e => ((UIDatePicker) e.Sender).Date.ToDateTimeOffset());

        public static IObservable<DateTimeOffset> DateComponent<T>(this Reactive<T> reactive) where T : UIDatePicker
            => reactive.Base.Rx().Date()
                .StartWith(reactive.Base.Date.ToDateTimeOffset())
                .DistinctUntilChanged(d => d.Date)
                .Skip(1);

        public static IObservable<DateTimeOffset> TimeComponent<T>(this Reactive<T> reactive) where T : UIDatePicker
            => reactive.Base.Rx().Date()
                .StartWith(reactive.Base.Date.ToDateTimeOffset())
                .DistinctUntilChanged(d => d.TimeOfDay)
                .Skip(1);
    }
}
