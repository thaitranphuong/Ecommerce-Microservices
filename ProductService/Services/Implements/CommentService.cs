using AutoMapper;
using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using ProductService.Models;
using ProductService.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Services.Implements
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentOutput> FindAll(int productId, int page, int limit)
        {
            var comments = await _commentRepository.FindAll(productId, page, limit);
            var commentDtos = new List<CommentDto>();
            foreach (var comment in comments)
            {
                commentDtos.Add(_mapper.Map<CommentDto>(comment));
            }
            CommentOutput output = new CommentOutput();
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling(
                (double)(await _commentRepository.FindByProductId(productId)).Count / limit);
            output.ListResult = commentDtos;
            return output;
        }

        public async Task<bool> Save(CommentDto dto)
        {
            dto.CreatedTime = DateTime.Now;
            return await _commentRepository.CreateOne(_mapper.Map<Comment>(dto)) > 0;
        }
    }
}
