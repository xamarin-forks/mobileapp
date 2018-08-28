using System;
using System.Reactive.Concurrency;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Login;
using Toggl.Foundation.Shortcuts;
using Toggl.PrimeRadiant;
using Toggl.PrimeRadiant.Settings;
using Toggl.Ultrawave;

namespace Toggl.Daneel.Services
{
    public sealed class IosLoginManager : LoginManager
    {
        public const string ApiTokenKey = "api-token";

        public IosLoginManager(
            IApiFactory apiFactory, 
            ITogglDatabase database, 
            IGoogleService googleService, 
            IApplicationShortcutCreator shortcutCreator, 
            IAccessRestrictionStorage accessRestrictionStorage, 
            IAnalyticsService analyticsService, 
            Func<ITogglApi, ITogglDataSource> createDataSource, IScheduler scheduler) 
            : base(apiFactory, database, googleService, shortcutCreator, accessRestrictionStorage, analyticsService, createDataSource, scheduler)
        {
        }
    }
}
