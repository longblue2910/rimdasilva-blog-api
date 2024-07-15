using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class NotificationHub : Hub
{
    private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

    public override Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        UserConnections.TryAdd(Context.ConnectionId, userId);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        UserConnections.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task JoinGroup(string groupName)
    {
        var userId = Context.UserIdentifier;
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
