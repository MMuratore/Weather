﻿using MediatR;

namespace Weather.SharedKernel.Event;

internal sealed class CustomTaskWhenAllPublisher : INotificationPublisher
{
    public async Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification,
        CancellationToken cancellationToken)
    {
        var tasks = handlerExecutors
            .Select(handler => handler.HandlerCallback(notification, cancellationToken))
            .ToArray();

        try
        {
            await Task.WhenAll(tasks);
        }
        catch
        {
            var exceptions = tasks.Where(t => t.IsFaulted).Select(t => (System.Exception)t.Exception!).ToArray();
            throw new AggregateException(exceptions);
        }
    }
}