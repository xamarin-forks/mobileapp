﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Toggl.Foundation.Interactors;
using Toggl.Foundation.MvvmCross.Extensions;
using Toggl.Foundation.MvvmCross.Services;
using Toggl.Multivac;
using Toggl.Multivac.Extensions;

namespace Toggl.Foundation.MvvmCross.ViewModels
{
    [Preserve(AllMembers = true)]
    public sealed class SendFeedbackViewModel : MvxViewModelResult<bool>
    {
        private readonly IDialogService dialogService;
        private readonly IInteractorFactory interactorFactory;
        private readonly IMvxNavigationService navigationService;

        // Internal States
        private readonly ISubject<bool> isLoadingSubject = new BehaviorSubject<bool>(false);
        private readonly ISubject<Exception> currentErrorSubject = new BehaviorSubject<Exception>(null);

        // Actions
        public UIAction CloseButtonTapped { get; }
        public UIAction ErrorViewTapped { get; }
        public UIAction SendButtonTapped { get; }

        // Inputs
        public ISubject<string> FeedbackText { get; } = new BehaviorSubject<string>(string.Empty);

        // Outputs
        public IObservable<bool> IsFeedbackEmpty { get; }
        public IObservable<bool> SendEnabled { get; }
        public IObservable<bool> IsLoading { get; }
        public IObservable<Exception> Error { get; }

        private IObservable<bool> isEmptyObservable => FeedbackText.Select(string.IsNullOrEmpty);
        private IObservable<bool> sendingIsEnabledObservable =>
            isEmptyObservable.CombineLatest(
                isLoadingSubject.AsObservable(),
                (isEmpty, isLoading) => !isEmpty && !isLoading);

        public SendFeedbackViewModel(
            IMvxNavigationService navigationService,
            IInteractorFactory interactorFactory,
            IDialogService dialogService,
            ISchedulerProvider schedulerProvider)
        {
            Ensure.Argument.IsNotNull(interactorFactory, nameof(interactorFactory));
            Ensure.Argument.IsNotNull(navigationService, nameof(navigationService));
            Ensure.Argument.IsNotNull(dialogService, nameof(dialogService));
            Ensure.Argument.IsNotNull(schedulerProvider, nameof(schedulerProvider));

            this.dialogService = dialogService;
            this.interactorFactory = interactorFactory;
            this.navigationService = navigationService;

            IsFeedbackEmpty = isEmptyObservable.DistinctUntilChanged().AsDriver(schedulerProvider);
            SendEnabled = sendingIsEnabledObservable.DistinctUntilChanged().AsDriver(schedulerProvider);

            CloseButtonTapped = new UIAction(cancel);
            ErrorViewTapped = new UIAction(dismissError);
            SendButtonTapped = new UIAction(sendFeedback, sendingIsEnabledObservable);

            IsLoading = isLoadingSubject.AsDriver(false, schedulerProvider);
            Error = currentErrorSubject.AsDriver(default(Exception), schedulerProvider);
        }

        private IObservable<Unit> dismissError()
        {
            currentErrorSubject.OnNext(null);
            return Observable.Return(Unit.Default);
        }

        private IObservable<Unit> cancel()
            => FeedbackText.FirstAsync()
                .Select(string.IsNullOrEmpty)
                .SelectMany(isEmpty => isEmpty
                    ? Observable.Return(true)
                    : dialogService.ConfirmDestructiveAction(ActionType.DiscardFeedback))
                .DoIf(shouldBeClosed => shouldBeClosed, _ => navigationService.Close(this, false))
                .SelectUnit();

        private IObservable<Unit> sendFeedback()
            => FeedbackText.FirstAsync()
                .Do(_ =>
                {
                    isLoadingSubject.OnNext(true);
                    currentErrorSubject.OnNext(null);
                })
                .SelectMany(text => interactorFactory
                    .SendFeedback(text)
                    .Execute()
                    .Materialize())
                .Do(notification =>
                {
                    switch (notification.Kind)
                    {
                        case NotificationKind.OnError:
                            isLoadingSubject.OnNext(false);
                            currentErrorSubject.OnNext(notification.Exception);
                            break;

                        default:
                            navigationService.Close(this, true);
                            break;
                    }
                })
                .SelectUnit();
    }
}
