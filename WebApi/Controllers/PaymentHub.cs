using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace WebApi.Controllers;

[Authorize]
public class PaymentHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Get the user identifier from the context
        var userId = Context.UserIdentifier;

        // Check if the user identifier is not null or empty
        if (!string.IsNullOrEmpty(userId))
        {
            // Add the connection to a group with the user identifier
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            Console.WriteLine($"User {userId} connected");
        }
        else
        {
            Console.WriteLine("User identifier is null or empty");
        }

        await base.OnConnectedAsync();
    }
}