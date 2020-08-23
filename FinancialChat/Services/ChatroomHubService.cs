using AutoMapper;
using FinancialChat.Data;
using FinancialChat.Hubs;
using FinancialChat.Models;
using FinancialChat.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Services
{
    public class ChatroomHubService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatroomHub> _hub;
        private readonly QueuePublisherService _queuePublisherService;

        public ChatroomHubService(ApplicationDbContext context, IMapper mapper, IHubContext<ChatroomHub> hub, QueuePublisherService queuePublisherService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
            _queuePublisherService = queuePublisherService ?? throw new ArgumentNullException(nameof(queuePublisherService));
        }

        public async Task SendData(IClientProxy client, int chatroomId)
        {
            await SendChatroomInfo(client, chatroomId);
            await SendLastMessages(client, chatroomId);
        }

        public async Task SendChatroomInfo(IClientProxy client, int chatroomId)
        {
            var chatroom = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == chatroomId);
            var chatroomViewModel = _mapper.Map<ChatroomViewModel>(chatroom);

            await client.SendAsync("ChatroomInfo", chatroomViewModel);
        }

        public async Task SendLastMessages(IClientProxy client, int chatroomId)
        {
            var messages = await _context.Messages
                .Where(x => x.ChatroomId == chatroomId)
                .OrderByDescending(x => x.CreatedOn)
                .Take(50)
                .Include(x => x.User)
                .ToListAsync();
            messages.Reverse();
            var messagesViewModel = _mapper.Map<IEnumerable<MessageViewModel>>(messages);

            await client.SendAsync("LastMessages", messagesViewModel);
        }

        public async Task<MessageViewModel> ProcessMessage(string messageContent, int chatroomId, string userId)
        {
            var message = new Message(messageContent, chatroomId, userId);

            if (message.IsCommand())
            {
                ProcessCommand(message);
                return null;
            }
            return await ProcessUserMessage(message);
        }

        public async Task PublishCommandResult(MessageViewModel messageViewModel, string chatroomId)
        {
            await _hub.Clients.Group(chatroomId).SendAsync("SendMessage", messageViewModel);
        }

        private async Task<MessageViewModel> ProcessUserMessage(Message message)
        {
            var messageEntity = await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            var savedMessage = await _context.Messages
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == messageEntity.Entity.Id);

            return _mapper.Map<MessageViewModel>(savedMessage);
        }

        private void ProcessCommand(Message message)
        {
            var stockCode = message.GetCommandParameter();
            _queuePublisherService.Publish($"{stockCode}|{message.ChatroomId}");
        }
    }
}
