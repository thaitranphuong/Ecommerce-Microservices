using AutoMapper;
using ProductService.Dtos;
using ProductService.Dtos.Pagination;
using ProductService.Models;
using ProductService.Repositories;
using ProductService.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Services.Implements
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _UserRepository = userRepository;
        }

        public async Task<CommentOutput> FindAll(int productId, int page, int limit)
        {
            var comments = await _commentRepository.FindAll(productId, page, limit);
            var commentDtos = new List<CommentDto>();
            foreach (var comment in comments)
            {
                var commentDto = _mapper.Map<CommentDto>(comment);
                var userLikeIds = new List<string>();
                foreach (var userLiked in comment.Likes)
                {
                    userLikeIds.Add(userLiked.Id);
                }
                commentDto.UserLikedIds = userLikeIds;
                commentDtos.Add(commentDto);
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

        public async Task<bool> DeleteById(int id)
        {
            var comment = await _commentRepository.FindById(id);
            return await _commentRepository.Remove(comment) > 0;
        }

        public async Task<bool> LikeOrUnLike(bool isLike, int commentId, string userId)
        {
            var comment = await _commentRepository.FindById(commentId);
            var user = await _UserRepository.FindById(userId);
            if (isLike)
            {
                comment.Likes.Add(user);
            } else
            {
                comment.Likes.Remove(user);
            }
            return await _commentRepository.SaveChange() > 0;
        }
    }
}
