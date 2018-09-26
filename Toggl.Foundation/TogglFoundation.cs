﻿using System;
using System.Reactive.Concurrency;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.Login;
using Toggl.Foundation.Services;
using Toggl.Foundation.Shortcuts;
using Toggl.Foundation.Suggestions;
using Toggl.Multivac;
using Toggl.PrimeRadiant;
using Toggl.Ultrawave;
using Toggl.Ultrawave.Network;

namespace Toggl.Foundation
{
    public sealed class TogglFoundation
    {
        public Version Version { get; }
        public UserAgent UserAgent { get; }
        public IApiFactory ApiFactory { get; }
        public ITogglDatabase Database { get; }
        public ITimeService TimeService { get; }
        public IScheduler Scheduler { get; }
        public IMailService MailService { get; }
        public PlatformInfo PlatformInfo { get; }
        public IGoogleService GoogleService { get; }
        public IRatingService RatingService { get; }
        public ApiEnvironment ApiEnvironment { get; }
        public ILicenseProvider LicenseProvider { get; }
        public IAnalyticsService AnalyticsService { get; }
        public IBackgroundService BackgroundService { get; }
        public ISchedulerProvider SchedulerProvider { get; }
        public IPlatformConstants PlatformConstants { get; }
        public IRemoteConfigService RemoteConfigService { get; }
        public IApplicationShortcutCreator ShortcutCreator { get; }
        public IIntentDonationService IntentDonationService { get; }
        public IPrivateSharedStorageService PrivateSharedStorageService { get; }
        public ISuggestionProviderContainer SuggestionProviderContainer { get; }

        public static Builder ForClient(UserAgent userAgent, Version version)
            => new Builder(userAgent, version);

        private TogglFoundation(Builder builder)
        {
            builder.EnsureValidity();

            Version = builder.Version;
            Database = builder.Database;
            UserAgent = builder.UserAgent;
            Scheduler = builder.Scheduler;
            ApiFactory = builder.ApiFactory;
            TimeService = builder.TimeService;
            MailService = builder.MailService;
            PlatformInfo = builder.PlatformInfo;
            GoogleService = builder.GoogleService;
            RatingService = builder.RatingService;
            ApiEnvironment = builder.ApiEnvironment;
            LicenseProvider = builder.LicenseProvider;
            ShortcutCreator = builder.ShortcutCreator;
            AnalyticsService = builder.AnalyticsService;
            PlatformConstants = builder.PlatformConstants;
            BackgroundService = builder.BackgroundService;
            SchedulerProvider = builder.SchedulerProvider;
            RemoteConfigService = builder.RemoteConfigService;
            IntentDonationService = builder.IntentDonationService;
            PrivateSharedStorageService = builder.PrivateSharedStorageService;
            SuggestionProviderContainer = builder.SuggestionProviderContainer;
        }

        public class Builder
        {
            public Version Version { get; internal set; }
            public UserAgent UserAgent { get; internal set; }
            public IApiFactory ApiFactory { get; internal set; }
            public ITogglDatabase Database { get; internal set; }
            public ITimeService TimeService { get; internal set; }
            public IScheduler Scheduler { get; internal set; }
            public IMailService MailService { get; internal set; }
            public PlatformInfo PlatformInfo { get; internal set; }
            public IRatingService RatingService { get; internal set; }
            public IGoogleService GoogleService { get; internal set; }
            public ApiEnvironment ApiEnvironment { get; internal set; }

            public ILicenseProvider LicenseProvider { get; internal set; }
            public IAnalyticsService AnalyticsService { get; internal set; }
            public ISchedulerProvider SchedulerProvider { get; internal set; }
            public IBackgroundService BackgroundService { get; internal set; }
            public IPlatformConstants PlatformConstants { get; internal set; }
            public IRemoteConfigService RemoteConfigService { get; internal set; }
            public IApplicationShortcutCreator ShortcutCreator { get; internal set; }
            public IIntentDonationService IntentDonationService { get; internal set; }
            public ISuggestionProviderContainer SuggestionProviderContainer { get; internal set; }
            public IPrivateSharedStorageService PrivateSharedStorageService { get; internal set; }

            public Builder(UserAgent agent, Version version)
            {
                UserAgent = agent;
                Version = version;
            }

            public Builder WithScheduler(IScheduler scheduler)
            {
                Scheduler = scheduler;
                return this;
            }

            public Builder WithSchedulerProvider(ISchedulerProvider schedulerProvider)
            {
                SchedulerProvider = schedulerProvider;
                return this;
            }

            public Builder WithDatabase(ITogglDatabase database)
            {
                Database = database;
                return this;
            }

            public Builder WithGoogleService(IGoogleService googleService)
            {
                GoogleService = googleService;
                return this;
            }

            public Builder WithApiEnvironment(ApiEnvironment apiEnvironment)
            {
                ApiEnvironment = apiEnvironment;
                return this;
            }

            public Builder WithApiFactory(IApiFactory apiFactory)
            {
                ApiFactory = apiFactory;
                return this;
            }

            public Builder WithMailService(IMailService mailService)
            {
                MailService = mailService;
                return this;
            }

            public Builder WithTimeService(ITimeService timeService)
            {
                TimeService = timeService;
                return this;
            }

            public Builder WithBackgroundService(IBackgroundService backgroundService)
            {
                BackgroundService = backgroundService;
                return this;
            }

            public Builder WithLicenseProvider(ILicenseProvider licenseProvider)
            {
                LicenseProvider = licenseProvider;
                return this;
            }

            public Builder WithAnalyticsService(IAnalyticsService analyticsService)
            {
                AnalyticsService = analyticsService;
                return this;
            }

            public Builder WithApplicationShortcutCreator(IApplicationShortcutCreator shortcutCreator)
            {
                ShortcutCreator = shortcutCreator;
                return this;
            }

            public Builder WithPlatformConstants(IPlatformConstants platformConstants)
            {
                PlatformConstants = platformConstants;
                return this;
            }

            public Builder WithSuggestionProviderContainer(ISuggestionProviderContainer suggestionProviderContainer)
            {
                SuggestionProviderContainer = suggestionProviderContainer;
                return this;
            }

            public Builder WithIntentDonationService(IIntentDonationService intentDonationService)
            {
                IntentDonationService = intentDonationService;
                return this;
            }

            public Builder WithPrivateSharedStorageService(IPrivateSharedStorageService privateSharedStorageService)
            {
                PrivateSharedStorageService = privateSharedStorageService;
                return this;
            }

            public Builder WithRemoteConfigService(IRemoteConfigService remoteConfigService)
            {
                RemoteConfigService = remoteConfigService;
                return this;
            }

            public Builder WithRatingService(IRatingService ratingService)
            {
                RatingService = ratingService;
                return this;
            }

            public Builder WithPlatformInfo(PlatformInfo platformInfo)
            {
                PlatformInfo = platformInfo;
                return this;
            }

            public Builder WithDatabase<TDatabase>()
                where TDatabase : ITogglDatabase, new()
                => WithDatabase(new TDatabase());

            public Builder WithGoogleService<TGoogleService>()
                where TGoogleService : IGoogleService, new()
                => WithGoogleService(new TGoogleService());

            public Builder WithApiFactory<TApiFactory>()
                where TApiFactory : IApiFactory, new()
                => WithApiFactory(new TApiFactory());

            public Builder WithMailService<TMailService>()
                where TMailService : IMailService, new()
                => WithMailService(new TMailService());

            public Builder WithTimeService<TTimeService>()
                where TTimeService : ITimeService, new()
                => WithTimeService(new TTimeService());

            public Builder WithBackgroundService<TBackgroundService>()
                where TBackgroundService : IBackgroundService, new()
                => WithBackgroundService(new TBackgroundService());

            public Builder WithLicenseProvider<TLicenseProvider>()
                where TLicenseProvider : ILicenseProvider, new()
                => WithLicenseProvider(new TLicenseProvider());

            public Builder WithAnalyticsService<TAnalyticsService>()
                where TAnalyticsService : IAnalyticsService, new()
                => WithAnalyticsService(new TAnalyticsService());

            public Builder WithApplicationShortcutCreator<TApplicationShortcutCreator>()
                where TApplicationShortcutCreator : IApplicationShortcutCreator, new()
                => WithApplicationShortcutCreator(new TApplicationShortcutCreator());

            public Builder WithPlatformConstants<TPlatformConstants>()
                where TPlatformConstants : IPlatformConstants, new()
                => WithPlatformConstants(new TPlatformConstants());

            public Builder WithSuggestionProviderContainer<TSuggestionProviderContainer>()
                where TSuggestionProviderContainer : ISuggestionProviderContainer, new()
                => WithSuggestionProviderContainer(new TSuggestionProviderContainer());

            public Builder WithRemoteConfigService<TRemoteConfigService>()
                where TRemoteConfigService : IRemoteConfigService, new()
                => WithRemoteConfigService(new TRemoteConfigService());

            public Builder WithRatingService<TRatingService>()
                where TRatingService : IRatingService, new()
                => WithRatingService(new TRatingService());

            public TogglFoundation Build()
                => new TogglFoundation(this);

            public void EnsureValidity()
            {
                Ensure.Argument.IsNotNull(Version, nameof(Version));
                Ensure.Argument.IsNotNull(Database, nameof(Database));
                Ensure.Argument.IsNotNull(UserAgent, nameof(UserAgent));
                Ensure.Argument.IsNotNull(Scheduler, nameof(Scheduler));
                Ensure.Argument.IsNotNull(ApiFactory, nameof(ApiFactory));
                Ensure.Argument.IsNotNull(TimeService, nameof(TimeService));
                Ensure.Argument.IsNotNull(MailService, nameof(MailService));
                Ensure.Argument.IsNotNull(PlatformInfo, nameof(PlatformInfo));
                Ensure.Argument.IsNotNull(GoogleService, nameof(GoogleService));
                Ensure.Argument.IsNotNull(RatingService, nameof(RatingService));
                Ensure.Argument.IsNotNull(LicenseProvider, nameof(LicenseProvider));
                Ensure.Argument.IsNotNull(ShortcutCreator, nameof(ShortcutCreator));
                Ensure.Argument.IsNotNull(AnalyticsService, nameof(AnalyticsService));
                Ensure.Argument.IsNotNull(BackgroundService, nameof(BackgroundService));
                Ensure.Argument.IsNotNull(SchedulerProvider, nameof(SchedulerProvider));
                Ensure.Argument.IsNotNull(PlatformConstants, nameof(PlatformConstants));
                Ensure.Argument.IsNotNull(RemoteConfigService, nameof(RemoteConfigService));
                Ensure.Argument.IsNotNull(IntentDonationService, nameof(IntentDonationService));
                Ensure.Argument.IsNotNull(SuggestionProviderContainer, nameof(SuggestionProviderContainer));
                Ensure.Argument.IsNotNull(PrivateSharedStorageService, nameof(PrivateSharedStorageService));
            }
        }
    }
}
