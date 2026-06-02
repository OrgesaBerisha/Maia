// Messaging/NotificationConsumer.cs
using Azure.Messaging.ServiceBus;

public class NotificationConsumer : BackgroundService
{
    private readonly ServiceBusClient _client;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _config;

    public NotificationConsumer(ServiceBusClient client,
        IServiceScopeFactory scopeFactory, IConfiguration config)
    {
        _client = client;
        _scopeFactory = scopeFactory;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        var processor = _client.CreateProcessor(
            _config["ServiceBus:QueueName"],
            new ServiceBusProcessorOptions());

        processor.ProcessMessageAsync += HandleMessage;
        processor.ProcessErrorAsync += HandleError;

        await processor.StartProcessingAsync(ct);
        await Task.Delay(Timeout.Infinite, ct);
        await processor.StopProcessingAsync();
    }

    private async Task HandleMessage(ProcessMessageEventArgs args)
    {
        var evt = args.Message.Body.ToObjectFromJson<NotificationEvent>();

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        var mailer = scope.ServiceProvider.GetRequiredService<IEmailService>();

        // 1. Persist in-app notification
        db.Notifications.Add(new Notification
        {
            UserId = evt.UserId,
            Title = evt.Title,
            Message = evt.Message,
            Type = evt.Type
        });
        await db.SaveChangesAsync();

        // 2. Send email if requested
        if (evt.Type == "email" && !string.IsNullOrEmpty(evt.Email))
            await mailer.SendAsync(evt.Email, evt.Title, evt.Message);

        await args.CompleteMessageAsync(args.Message);
    }

    private Task HandleError(ProcessErrorEventArgs args)
    {
        Console.Error.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}