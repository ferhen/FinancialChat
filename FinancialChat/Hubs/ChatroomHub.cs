using FinancialChat.Models;
using FinancialChat.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FinancialChat.Hubs
{
    [Authorize(AuthenticationSchemes = "IdentityServerJwtBearer")]
    public class ChatroomHub : Hub
    {
        private readonly ChatroomHubService _service;

        public ChatroomHub(ChatroomHubService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HubMethodName("SetChatroom")]
        public async Task SetChatroom(string chatroomIdString)
        {
            int chatroomId = int.Parse(chatroomIdString);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatroomIdString);
            await _service.SendData(Clients.Caller, chatroomId);
        }

        [HubMethodName("ReceiveMessage")]
        public async Task ReceiveMessage(string message, string chatroomIdString)
        {
            int chatroomId = int.Parse(chatroomIdString);
            var messageViewModel = await _service.ProcessMessage(message, chatroomId, Context.UserIdentifier);
            if (!(messageViewModel is null))
                await Clients.Group(chatroomIdString).SendAsync("SendMessage", messageViewModel);
        }
    }
}
