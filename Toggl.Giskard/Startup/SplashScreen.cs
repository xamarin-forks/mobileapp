using Android.App;
using Android.Content.PM;
using Android.OS;
using Toggl.Foundation.MvvmCross;
using Toggl.Foundation.MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Navigation;

namespace Toggl.Giskard
{
    [Activity(Label = "Toggl for Devs",
              MainLauncher = true,
              Icon = "@mipmap/ic_launcher",
              Theme = "@style/Theme.Splash",
              ScreenOrientation = ScreenOrientation.Portrait,
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    [IntentFilter(
        new[] { "android.intent.action.VIEW", "android.intent.action.EDIT" },
        Categories = new[] { "android.intent.category.BROWSABLE", "android.intent.category.DEFAULT" },
        DataSchemes = new[] { "toggl" },
        DataHost = "*")]
    public class SplashScreen : MvxSplashScreenAppCompatActivity<Setup, App<LoginViewModel>>
    {
        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {

        }

        protected override void RunAppStart(Bundle bundle)
        {
            base.RunAppStart(bundle);
            var navigationUrl = Intent.Data?.ToString();
            var navigationService = Mvx.Resolve<IMvxNavigationService>();
            if (string.IsNullOrEmpty(navigationUrl))
            {
                navigationService.Navigate<MainViewModel>();
                return;
            }

            navigationService.Navigate(navigationUrl);
        }
    }
}
