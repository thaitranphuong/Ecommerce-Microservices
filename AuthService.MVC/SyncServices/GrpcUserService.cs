using AuthService.MVC.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AuthService.MVC.SyncServices
{
    public class GrpcUserService : GrpcUser.GrpcUserBase
    {
        private readonly AppDbContext _context;

        public GrpcUserService(AppDbContext context)
        {
            _context = context;
        }

        public async override Task<UserResponse> GetUser(UserRequest request, ServerCallContext context)
        {
            AppUser user = await _context.Users.FindAsync(request.Id);
            var response = new UserResponse()
            {
                Id = user.Id,
                UserName = user.UserName,
                Avatar = user.Avatar
            };

            return response;
        }
    }
}
