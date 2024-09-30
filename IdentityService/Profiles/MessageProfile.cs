using AutoMapper;
using IdentityService.Models;
using IdentityService.Models.DTOs;

namespace IdentityService.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageDto, Message>();
            CreateMap<Message, MessageDto>();
        }
    }
}
