using AutoMapper;
using BlogService.Dtos;
using BlogService.Models;
using BlogService.Repositories;
using BlogService.SyncServices;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogService.Services.Implements
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly IGrpcUserService _grpcUserService;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IGrpcUserService grpcUserService)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _grpcUserService = grpcUserService;
        }

        public async Task<List<CommentDto>> FindByBlogId(string blogId)
        {
            var comments = await _commentRepository.FindByBlogId(blogId);
            var commentDtos = new List<CommentDto>();
            foreach (var comment in comments)
            {
                var commentDto = _mapper.Map<CommentDto>(comment);
                var reply = await _grpcUserService.GetUser(comment.UserId);
                commentDto.UserName = reply.UserName;
                commentDto.UserAvatar = reply.Avatar;
                commentDtos.Add(commentDto);
            }
            return commentDtos;
        }

        public async Task<bool> Save(CommentDto comment)
        {
            comment.ExternalId = Guid.NewGuid().ToString();
            comment.CreatedTime = DateTime.Now;
            return await _commentRepository.CreateOne(_mapper.Map<Comment>(comment)) > 0;
        }
    }
}
