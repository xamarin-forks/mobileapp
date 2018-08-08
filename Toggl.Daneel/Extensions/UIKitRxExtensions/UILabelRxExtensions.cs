using System;
using Foundation;
using UIKit;

namespace Toggl.Daneel.Extensions
{
    public static partial class UIKitRxExtensions
    {
        public static Action<string> BindText<T>(this Reactive<T> reactive) where T : UILabel
            => text => reactive.Base.Text = text;

        public static Action<NSAttributedString> BindAttributedText<T>(this Reactive<T> reactive) where T : UILabel
            => text => reactive.Base.AttributedText = text;
    }
}
