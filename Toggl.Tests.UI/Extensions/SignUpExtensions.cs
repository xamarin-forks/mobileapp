using Xamarin.UITest;

namespace Toggl.Tests.UI.Extensions
{
    public static class SignUpExtensions
    {
        public static void WaitForSignUpScreen(this IApp app)
        {
            app.WaitForLoginScreen();
            app.Tap(Login.SwitchToSignUpLabel);
            app.WaitForElement(SignUp.SignUpButton);
        }

        public static void TrySigningUpAndFail(this IApp app)
        {
            app.Tap(Login.LoginButton);

            app.WaitForElement(Login.ErrorLabel);
        }

        public static void SignUpSuccesfully(this IApp app)
        {
            app.Tap(Login.LoginButton);

            app.WaitForElement(Main.StartTimeEntryButton);
        }
    }
}
