using AutoMapper;
using BlogService.Dtos;
using BlogService.Dtos.Pagination;
using BlogService.Models;
using BlogService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogService.Services.Implements
{
    public class BlogServices : IBlogService
    {
        private readonly IMapper _mapper;
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;

        public BlogServices(IBlogRepository blogRepository, IMapper mapper, ICommentRepository commentRepository)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
            _commentRepository = commentRepository;
        }

        public async Task<bool> DeleteById(string id)
        {
            return await _blogRepository.Remove(id) > 0;
        }

        public async Task<BlogOutput> FindAll(string title, int page, int limit)
        {
            var blogs = await _blogRepository.FindAll(title, page, limit);
            var blogDtos = new List<BlogDto>();
            foreach (var blog in blogs)
            {
                var blogDto = _mapper.Map<BlogDto>(blog);
                blogDto.CommentCount = (await _commentRepository.FindByBlogId(blog.ExternalId)).Count;
                blogDtos.Add(blogDto);
            }
            BlogOutput output = new BlogOutput();
            output.Title = title;
            output.Page = page;
            output.TotalPage = (int)Math.Ceiling((double)(await _blogRepository.FindByTitle(title)).Count / limit);
            output.ListResult = blogDtos;
            return output;
        }

        public async Task<BlogOutput> FindAllOrderByView()
        {
            var blogs = await _blogRepository.FindAllOrderByView();
            var blogDtos = new List<BlogDto>();
            foreach (var blog in blogs)
            {
                var blogDto = _mapper.Map<BlogDto>(blog);
                blogDto.CommentCount = (await _commentRepository.FindByBlogId(blog.ExternalId)).Count;
                blogDtos.Add(blogDto);
            }
            BlogOutput output = new BlogOutput();
            output.ListResult = blogDtos;
            return output;
        }

        public async Task<BlogDto> FindById(string id)
        {
            return _mapper.Map<BlogDto>(await _blogRepository.FindById(id));
        }

        public async Task<bool> UpdateViewNumber(string id)
        {
            return await _blogRepository.UpdateViewNumber(id) > 0;
        }

        public async Task<BlogDto> FindBySlug(string slug)
        {
            return _mapper.Map<BlogDto>(await _blogRepository.FindBySlug(slug));
        }

        public async Task<bool> Save(BlogDto dto)
        {
            if (string.IsNullOrEmpty(dto.ExternalId))
            {
                dto.ExternalId = Guid.NewGuid().ToString();
                dto.Slug += "#" + dto.ExternalId;
                dto.ViewNumber = 0;
                dto.CreatedTime = DateTime.Now;
                dto.UpdatedTime = DateTime.Now;
                dto.Comments = new List<CommentDto>();
                return await _blogRepository.CreateOne(_mapper.Map<Blog>(dto)) > 0;
            }
            else
            {
                dto.Slug += "#" + dto.ExternalId;
                dto.UpdatedTime = DateTime.Now;
                return await _blogRepository.Update(_mapper.Map<Blog>(dto)) > 0;
            }
        }
    }
}
