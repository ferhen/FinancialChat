using AutoMapper;
using FinancialChat.Models;
using FinancialChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialChat.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Chatroom, ChatroomViewModel>()
                .ReverseMap();
            CreateMap<Message, MessageViewModel>()
                .ForMember(x => x.UserName, x => x.MapFrom(y => y.User.UserName));
        }
    }
}
