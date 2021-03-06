﻿using System;
using Foundation;
using Intents;
using ObjCRuntime;

namespace Toggl.Daneel.Intents
{
    // @interface ShowReportIntent : INIntent
    // [Watch(5, 0), iOS(12, 0)]
    [BaseType(typeof(INIntent))]
    interface ShowReportIntent
    {
    }

    // @protocol ShowReportIntentHandling <NSObject>
    // [Watch(5, 0), iOS(12, 0)]
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface ShowReportIntentHandling
    {
        // @required -(void)handleShowReport:(ShowReportIntent * _Nonnull)intent completion:(void (^ _Nonnull)(ShowReportIntentResponse * _Nonnull))completion;
        [Abstract]
        [Export("handleShowReport:completion:")]
        void HandleShowReport(ShowReportIntent intent, Action<ShowReportIntentResponse> completion);

        // @optional -(void)confirmShowReport:(ShowReportIntent * _Nonnull)intent completion:(void (^ _Nonnull)(ShowReportIntentResponse * _Nonnull))completion;
        [Export("confirmShowReport:completion:")]
        void ConfirmShowReport(ShowReportIntent intent, Action<ShowReportIntentResponse> completion);
    }

    // @interface ShowReportIntentResponse : INIntentResponse
    // [Watch(5, 0), iOS(12, 0)]
    [BaseType(typeof(INIntentResponse))]
    [DisableDefaultCtor]
    interface ShowReportIntentResponse
    {
        // -(instancetype _Nonnull)initWithCode:(ShowReportIntentResponseCode)code userActivity:(NSUserActivity * _Nullable)userActivity __attribute__((objc_designated_initializer));
        [Export("initWithCode:userActivity:")]
        [DesignatedInitializer]
        IntPtr Constructor(ShowReportIntentResponseCode code, [NullAllowed] NSUserActivity userActivity);

        // @property (readonly, nonatomic) ShowReportIntentResponseCode code;
        [Export("code")]
        ShowReportIntentResponseCode Code { get; }
    }

    // @interface ShowReportPeriodIntent : INIntent
    // [Watch(5, 0), iOS(12, 0)]
    [BaseType(typeof(INIntent))]
    interface ShowReportPeriodIntent
    {
        // @property (assign, readwrite, nonatomic) ShowReportPeriodReportPeriod period;
        [Export("period", ArgumentSemantic.Assign)]
        ShowReportPeriodReportPeriod Period { get; set; }
    }

    // @protocol ShowReportPeriodIntentHandling <NSObject>
    // [Watch(5, 0), iOS(12, 0)]
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface ShowReportPeriodIntentHandling
    {
        // @required -(void)handleShowReportPeriod:(ShowReportPeriodIntent * _Nonnull)intent completion:(void (^ _Nonnull)(ShowReportPeriodIntentResponse * _Nonnull))completion;
        [Abstract]
        [Export("handleShowReportPeriod:completion:")]
        void HandleShowReportPeriod(ShowReportPeriodIntent intent, Action<ShowReportPeriodIntentResponse> completion);

        // @optional -(void)confirmShowReportPeriod:(ShowReportPeriodIntent * _Nonnull)intent completion:(void (^ _Nonnull)(ShowReportPeriodIntentResponse * _Nonnull))completion;
        [Export("confirmShowReportPeriod:completion:")]
        void ConfirmShowReportPeriod(ShowReportPeriodIntent intent, Action<ShowReportPeriodIntentResponse> completion);
    }

    // @interface ShowReportPeriodIntentResponse : INIntentResponse
    // [Watch(5, 0), iOS(12, 0)]
    [BaseType(typeof(INIntentResponse))]
    [DisableDefaultCtor]
    interface ShowReportPeriodIntentResponse
    {
        // -(instancetype _Nonnull)initWithCode:(ShowReportPeriodIntentResponseCode)code userActivity:(NSUserActivity * _Nullable)userActivity __attribute__((objc_designated_initializer));
        [Export("initWithCode:userActivity:")]
        [DesignatedInitializer]
        IntPtr Constructor(ShowReportPeriodIntentResponseCode code, [NullAllowed] NSUserActivity userActivity);

        // +(instancetype _Nonnull)successIntentResponseWithPeriod:(ShowReportPeriodReportPeriod)period;
        [Static]
        [Export("successIntentResponseWithPeriod:")]
        ShowReportPeriodIntentResponse SuccessIntentResponseWithPeriod(ShowReportPeriodReportPeriod period);

        // @property (assign, readwrite, nonatomic) ShowReportPeriodReportPeriod period;
        [Export("period", ArgumentSemantic.Assign)]
        ShowReportPeriodReportPeriod Period { get; set; }

        // @property (readonly, nonatomic) ShowReportPeriodIntentResponseCode code;
        [Export("code")]
        ShowReportPeriodIntentResponseCode Code { get; }
    }

    // @interface StartTimerIntent : INIntent
    // [Watch(5, 0), iOS(12, 0)]
    [BaseType(typeof(INIntent))]
    interface StartTimerIntent
    {
        // @property (readwrite, copy, nonatomic) INObject * _Nullable workspace;
        [NullAllowed, Export("workspace", ArgumentSemantic.Copy)]
        INObject Workspace { get; set; }

        // @property (readwrite, copy, nonatomic) NSString * _Nullable entryDescription;
        [NullAllowed, Export("entryDescription")]
        string EntryDescription { get; set; }

        // @property (readwrite, copy, nonatomic) INObject * _Nullable billable;
        [NullAllowed, Export("billable", ArgumentSemantic.Copy)]
        INObject Billable { get; set; }

        // @property (readwrite, copy, nonatomic) INObject * _Nullable projectId;
        [NullAllowed, Export("projectId", ArgumentSemantic.Copy)]
        INObject ProjectId { get; set; }

        // @property (readwrite, copy, nonatomic) NSArray<INObject *> * _Nullable tags;
        [NullAllowed, Export("tags", ArgumentSemantic.Copy)]
        INObject[] Tags { get; set; }
    }

    // @protocol StartTimerIntentHandling <NSObject>
    // [Watch(5, 0), iOS(12, 0)]
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface StartTimerIntentHandling
    {
        // @required -(void)handleStartTimer:(StartTimerIntent * _Nonnull)intent completion:(void (^ _Nonnull)(StartTimerIntentResponse * _Nonnull))completion;
        [Abstract]
        [Export("handleStartTimer:completion:")]
        void HandleStartTimer(StartTimerIntent intent, Action<StartTimerIntentResponse> completion);

        // @optional -(void)confirmStartTimer:(StartTimerIntent * _Nonnull)intent completion:(void (^ _Nonnull)(StartTimerIntentResponse * _Nonnull))completion;
        [Export("confirmStartTimer:completion:")]
        void ConfirmStartTimer(StartTimerIntent intent, Action<StartTimerIntentResponse> completion);
    }

    // @interface StartTimerIntentResponse : INIntentResponse
    // [Watch(5, 0), iOS(12, 0)]
    [BaseType(typeof(INIntentResponse))]
    [DisableDefaultCtor]
    interface StartTimerIntentResponse
    {
        // -(instancetype _Nonnull)initWithCode:(StartTimerIntentResponseCode)code userActivity:(NSUserActivity * _Nullable)userActivity __attribute__((objc_designated_initializer));
        [Export("initWithCode:userActivity:")]
        [DesignatedInitializer]
        IntPtr Constructor(StartTimerIntentResponseCode code, [NullAllowed] NSUserActivity userActivity);

        // @property (readonly, nonatomic) StartTimerIntentResponseCode code;
        [Export("code")]
        StartTimerIntentResponseCode Code { get; }
    }

    // @interface StopTimerIntent : INIntent
    // [Watch(5, 0), iOS(12, 0)]
    [BaseType(typeof(INIntent))]
    interface StopTimerIntent
    {
    }

    // @protocol StopTimerIntentHandling <NSObject>
    // [Watch(5, 0), iOS(12, 0)]
    [Protocol, Model]
    [BaseType(typeof(NSObject))]
    interface StopTimerIntentHandling
    {
        // @required -(void)handleStopTimer:(StopTimerIntent * _Nonnull)intent completion:(void (^ _Nonnull)(StopTimerIntentResponse * _Nonnull))completion;
        [Abstract]
        [Export("handleStopTimer:completion:")]
        void HandleStopTimer(StopTimerIntent intent, Action<StopTimerIntentResponse> completion);

        // @optional -(void)confirmStopTimer:(StopTimerIntent * _Nonnull)intent completion:(void (^ _Nonnull)(StopTimerIntentResponse * _Nonnull))completion;
        [Export("confirmStopTimer:completion:")]
        void ConfirmStopTimer(StopTimerIntent intent, Action<StopTimerIntentResponse> completion);
    }

    // @interface StopTimerIntentResponse : INIntentResponse
    // [Watch(5, 0), iOS(12, 0)]
    [BaseType(typeof(INIntentResponse))]
    [DisableDefaultCtor]
    interface StopTimerIntentResponse
    {
        // -(instancetype _Nonnull)initWithCode:(StopTimerIntentResponseCode)code userActivity:(NSUserActivity * _Nullable)userActivity __attribute__((objc_designated_initializer));
        [Export("initWithCode:userActivity:")]
        [DesignatedInitializer]
        IntPtr Constructor(StopTimerIntentResponseCode code, [NullAllowed] NSUserActivity userActivity);

        // +(instancetype _Nonnull)successIntentResponseWithEntryDescription:(NSString * _Nonnull)entryDescription entryDurationString:(NSString * _Nonnull)entryDurationString;
        [Static]
        [Export("successIntentResponseWithEntryDescription:entryDurationString:")]
        StopTimerIntentResponse SuccessIntentResponseWithEntryDescription(string entryDescription, string entryDurationString);

        // @property (readwrite, copy, nonatomic) NSString * _Nullable entryDescription;
        [NullAllowed, Export("entryDescription")]
        string EntryDescription { get; set; }

        // @property (readwrite, copy, nonatomic) NSString * _Nullable entryDurationString;
        [NullAllowed, Export("entryDurationString")]
        string EntryDurationString { get; set; }

        // @property (readwrite, copy, nonatomic) NSNumber * _Nullable entryStart;
        [NullAllowed, Export("entryStart", ArgumentSemantic.Copy)]
        NSNumber EntryStart { get; set; }

        // @property (readwrite, copy, nonatomic) NSNumber * _Nullable entryDuration;
        [NullAllowed, Export("entryDuration", ArgumentSemantic.Copy)]
        NSNumber EntryDuration { get; set; }

        // @property (readonly, nonatomic) StopTimerIntentResponseCode code;
        [Export("code")]
        StopTimerIntentResponseCode Code { get; }
    }
}
