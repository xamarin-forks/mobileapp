using System;
using Xamarin.UITest.Queries;

namespace Toggl.Tests.UI
{
    public static class Onboarding
    {
        public const string FirstOnboardingElement = "";
        public const string SkipButton = "";
        public const string NextButton = "";
        public const string FirstLabel = "";
        public const string SecondLabel = "";
        public const string ThirdLabel = "";
        public const string PreviousButton = "";
    }

    public static class Login
    {
        public static readonly Func<AppQuery, AppQuery> EmailText = x => x.Id("LoginEmailTextField");
        public static readonly Func<AppQuery, AppQuery> ErrorLabel = x => x.Id("InfoTextField");
        public const string LoginButton = "LoginButton";
        public static readonly Func<AppQuery, AppQuery> PasswordText = x => x.Id("LoginPasswordTextField");
        public const string ShowPasswordButton = "";
        public const string SwitchToSignUpLabel = "LoginSwitchToSignUp";
        public const string ForgotPasswordButton = "";
    }

    public static class SignUp
    {
        public const string EmailText = "SignUpEmail";
        public const string SignUpButton = "SignUpButton";
        public const string PasswordText = "SignUpPassword";
        public const string GdprButton = "SignUpGdprButton";
    }

    public static class Main
    {
        public static readonly Func<AppQuery, AppQuery> StartTimeEntryButton = x => x.Id("MainPlayButton");
        public static readonly Func<AppQuery, AppQuery> StopTimeEntryButton = x => x.Id("MainStopButton");
    }

    public static class StartTimeEntry
    {
        public static readonly Func<AppQuery, AppQuery> DoneButton = x => x.Id("StartTimeEntryDoneButton");
        public static readonly Func<AppQuery, AppQuery> DescriptionText = x => x.Id("StartTimeEntryDescriptionTextField");
    }
}
