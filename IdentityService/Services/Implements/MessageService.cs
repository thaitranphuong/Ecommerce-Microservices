using AutoMapper;
using IdentityService.Models;
using IdentityService.Models.DTOs;
using IdentityService.Repositories;
using IdentityService.Repositories.Implements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityService.Services.Implements
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<List<MessageDto>> FindAll(string userIdFirst, string userIdSecond)
        {
            List<Message> entities = await _messageRepository.FindAll(userIdFirst, userIdSecond);
            List<MessageDto> DTOs = new List<MessageDto>();
            foreach (var entity in entities)
            {
                MessageDto dto = _mapper.Map<MessageDto>(entity);
                dto.Avatar = entity.Sender.Avatar;
                DTOs.Add(dto);
            }
            return DTOs;
        }

        public async Task Save(MessageDto message)
        {
            message.CreatedTime = System.DateTime.Now;
            var mess = _mapper.Map<Message>(message);
            await _messageRepository.Save(mess);
        }
    }
}
