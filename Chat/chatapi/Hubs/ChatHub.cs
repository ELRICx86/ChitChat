using chatapi.Dtos;
using chatapi.Services;
using Microsoft.AspNetCore.SignalR;

namespace chatapi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
           
        }
        public override async Task OnConnectedAsync()
        {
            
            await Groups.AddToGroupAsync(Context.ConnectionId, "come&chat");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "come&chat");
            var user = _chatService.GetUserByConnectionId(Context.ConnectionId);
            _chatService.RemoveUserFromList(user);
            await DisplayOnline();
            await base.OnDisconnectedAsync(e);
            
        }

        public async Task addUserConnectionId(string myname)
        {
            _chatService.AddUserConnectionId(myname, Context.ConnectionId);
            await DisplayOnline();
        }

        private async Task DisplayOnline()
        {
            var onlineUsers = _chatService.GetOnlineUsers();
            await Clients.Groups("come&chat").SendAsync("OnlineUsers", onlineUsers);
        }

        public async Task ReceivePrivate(MessageDto messageDto)
        {
            var sender = _chatService.GetConnectionIdByUser(messageDto.To);
            var receiver = _chatService.GetConnectionIdByUser(messageDto.From);
            await Clients.Clients(sender, receiver).SendAsync("privateMessage", messageDto);
            
        }



        public async Task ReceiveMessage(MessageDto message)
        {
            await Clients.Group("come&chat").SendAsync("NewMessage", message);
        }
    }
}
