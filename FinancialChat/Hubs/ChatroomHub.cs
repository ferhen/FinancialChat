using FinancialChat.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FinancialChat.Hubs
{
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
    }
}
