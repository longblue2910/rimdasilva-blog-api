using Microsoft.AspNetCore.SignalR;

namespace Post.Api.Services;

public class NotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNotificationToGroup(string groupName, string message)
    {
        await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", message);
    }

    public async Task SendNotificationToUser(string userId, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
}
