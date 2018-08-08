using System;
using System.Reactive.Linq;
using UIKit;

namespace Toggl.Daneel.Extensions
{
    public static partial class UIKitRxExtensions
    {
        public static IObservable<string> Text<T>(this Reactive<T> reactive) where T: UITextField
            => Observable
                .FromEventPattern(handler => reactive.Base.EditingChanged += handler, handler => reactive.Base.EditingChanged -= handler)
                .Select(_ => reactive.Base.Text);
        
        public static Action<bool> SecureTextEntry<T>(this Reactive<T> reactive) where T : UITextField => isSecure =>
        {
            reactive.Base.ResignFirstResponder();
            reactive.Base.SecureTextEntry = isSecure;
            reactive.Base.BecomeFirstResponder();
        };
    }
}
