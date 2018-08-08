using System;
using UIKit;

namespace Toggl.Daneel.Extensions
{
    public static partial class UIKitRxExtensions
    {
        public static Action<bool> BindIsEnabled<T>(this Reactive<T> reactive) where T: UIControl
            => enabled => reactive.Base.Enabled = enabled;
    }
}
