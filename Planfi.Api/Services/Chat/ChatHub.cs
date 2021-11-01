using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using PlanfiApi.Models;

namespace PlanfiApi.Services.Chat
{
    [EnableCors("AllowAll")] 
    [AllowAnonymous]
    public class ChatHub : Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMessageService _messageService;
        private int _usersOnline;

        public ChatHub(IChatRoomService chatRoomService, IMessageService messageService)
        {
            _chatRoomService = chatRoomService;
            _messageService = messageService;
        }

        public async Task SendMessage(Guid roomId, string user, string message)
        {
            var m = new Message()
            {
                RoomId = roomId,
                Contents = message,
                UserName = user
            };

            await _messageService.AddMessageToRoomAsync(roomId, m);
            await Clients.All.SendAsync("ReceiveMessage", user, message, roomId, m.Id, m.PostedAt);
        }

        public async Task AddChatRoom(string roomName)
        {
            var chatRoom = new ChatRoom()
            {
                Name = roomName
            };

            await _chatRoomService.AddChatRoomAsync(chatRoom);
            await Clients.All.SendAsync("NewRoom", roomName, chatRoom.Id);
        }

        public override async Task OnConnectedAsync()
        {
            _usersOnline++;
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _usersOnline--;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}