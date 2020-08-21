using AutoMapper;
using FinancialChat.Data;
using FinancialChat.Models;
using FinancialChat.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Services
{
    public class ChatroomService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ChatroomService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ChatroomViewModel>> List() => _mapper.Map<ChatroomViewModel[]>(await _context.Chatrooms.ToListAsync());

        public async Task<ChatroomViewModel> Find(int id) => _mapper.Map<ChatroomViewModel>(await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == id));

        public async Task<ChatroomViewModel> Create(string name)
        {
            var chatroom = new Chatroom(name);

            var newChatroom = await _context.Chatrooms.AddAsync(chatroom);
            await _context.SaveChangesAsync();

            var chatroomViewModel = _mapper.Map<ChatroomViewModel>(newChatroom.Entity);

            return chatroomViewModel;
        }

        public async Task<ChatroomViewModel> Update(ChatroomViewModel chatroomViewModel)
        {
            var chatroom = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == chatroomViewModel.Id);

            chatroom.Update(chatroomViewModel.Name);
            await _context.SaveChangesAsync();

            return chatroomViewModel;
        }

        public async Task<int> Delete(int chatroomId)
        {
            var chatroom = await _context.Chatrooms.FirstOrDefaultAsync(x => x.Id == chatroomId);

            _context.Chatrooms.Remove(chatroom);
            await _context.SaveChangesAsync();

            return chatroomId;
        }
    }
}
