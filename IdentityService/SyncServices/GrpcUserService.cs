using IdentityService.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace IdentityService.SyncServices
{
    public class GrpcUserService : GrpcUser.GrpcUserBase
    {
        private readonly MyDbContext _context;

        public GrpcUserService(MyDbContext context)
        {
            _context = context;
        }

        public async override Task<UserResponse> GetUser(UserRequest request, ServerCallContext context)
        {
            User user = await _context.Users.FindAsync(request.Id);
            if (user == null) return new UserResponse()
            {
                Id = string.Empty,
                UserName = string.Empty,
                Avatar = string.Empty
            };
            var response = new UserResponse()
            {
                Id = user.Id,
                UserName = user.Name,
                Avatar = user.Avatar
            };

            return response;
        }
    }
}
