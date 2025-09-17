using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapHub<ChatHub>("/chathub");
app.MapFallbackToPage("/_Host");

app.Run();

public class ChatHub : Hub
{
    private static readonly HashSet<string> Users = new();

    public override async Task OnConnectedAsync()
    {
        Users.Add(Context.ConnectionId);
        await Clients.All.SendAsync("UsersUpdated", Users.Count);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Users.Remove(Context.ConnectionId);
        await Clients.All.SendAsync("UsersUpdated", Users.Count);
        await base.OnDisconnectedAsync(exception);
    }

    // ✅ Unified SendMessage: Handles text or file
    public async Task SendMessage(string user, string text, bool isFile, string fileType)
    {
        await Clients.All.SendAsync(
            "ReceiveMessage",
            user,
            text,
            DateTime.Now.ToString("HH:mm"),
            isFile,
            fileType
        );
    }

    public async Task Typing(string user)
    {
        await Clients.Others.SendAsync("UserTyping", user);
    }

    public async Task StopTyping(string user)
    {
        await Clients.Others.SendAsync("UserStoppedTyping", user);
    }
}
