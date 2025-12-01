using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using YeuBep.ViewModels.Notification;

namespace YeuBep.Data;

public class NotificationHub : Hub
{
    public async Task SendMessage(NotificationViewModel model)
    {
        await Clients.All.SendAsync("ReceiveMessage", JsonSerializer.Serialize(model));
    }
}