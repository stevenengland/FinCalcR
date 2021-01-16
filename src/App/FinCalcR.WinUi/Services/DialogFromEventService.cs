using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StEn.FinCalcR.Common.LanguageResources;
using StEn.FinCalcR.WinUi.Events;
using StEn.FinCalcR.WinUi.Extensions;
using StEn.FinCalcR.WinUi.LibraryMapper.DialogHost;
using StEn.FinCalcR.WinUi.Platform;

namespace StEn.FinCalcR.WinUi.Services
{
    public class DialogFromEventService : INotificationHandler<ErrorEvent>, INotificationHandler<HintEvent>
    {
        private readonly IDialogHostMapper dialogHostMapper;
        private readonly IMediator mediator;

        public DialogFromEventService(IDialogHostMapper dialogHostMapper, IMediator mediator)
        {
            this.dialogHostMapper = dialogHostMapper;
            this.mediator = mediator;
        }

        public async Task Handle(HintEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null)
            {
                return;
            }

            await Execute.OnUiThreadAsync(() => this.dialogHostMapper.ShowAsync(
                this.dialogHostMapper.GetHintView(notification.Message),
                "RootDialog")).ConfigureAwait(true);
        }

        public async Task Handle(ErrorEvent notification, CancellationToken cancellationToken)
        {
            if (notification == null)
            {
                notification = new ErrorEvent(new ArgumentException(Resources.EXC_ARGUMENT_NULL + nameof(notification)), Resources.EXC_ARGUMENT_NULL + nameof(notification), false);
            }

            if (notification.ApplicationMustShutdown)
            {
                await Execute.OnUiThreadAsync(() => this.dialogHostMapper.ShowAsync(
                    this.dialogHostMapper.GetErrorView(notification.ErrorMessage, notification.Exception?.Message, $"{Resources.EXC_ERROR_EVENT_GENERAL_APP_NEEDS_SHUTDOWN}"),
                    "RootDialog").ConfigureAwait(true)).ConfigureAwait(true);

                this.mediator.PublishOnUiThread(new ApplicationShutdownEvent());
            }
            else
            {
                await Execute.OnUiThreadAsync(() => this.dialogHostMapper.ShowAsync(
                    this.dialogHostMapper.GetErrorView(notification.ErrorMessage, notification.Exception?.Message),
                    "RootDialog").ConfigureAwait(true)).ConfigureAwait(true);
            }
        }
    }
}
