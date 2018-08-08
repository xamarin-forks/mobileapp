using System;
using UIKit;

namespace Toggl.Daneel.Extensions
{
    public static partial class UIKitRxExtensions
    {
        public static Action<bool> IsOn<T>(this Reactive<T> reactive) where T : UISwitch
            => isOn => reactive.Base.SetState(isOn, true);
    }
}
