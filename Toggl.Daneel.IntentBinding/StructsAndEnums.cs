using System;
using ObjCRuntime;

namespace Toggl.Daneel.Intents
{
//    [Watch (5,0), iOS (12,0)]
    [Native]
    public enum StopTimerIntentResponseCode : long
    {
        Unspecified = 0,
        Ready,
        ContinueInApp,
        InProgress,
        Success,
        Failure,
        FailureRequiringAppLaunch
    }
}
