using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Repositories.Implements
{
    public class UserRepository: IUserRepository
    {
        private readonly MyDbContext _context;
        public UserRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateOne(User user)
        {
            user.Roles.Add(_context.Roles.FirstOrDefault(role => role.Name.Equals("customer")));
            _context.Users.Add(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<User>> FindAll(string email, int page, int limit)
        {
            if (string.IsNullOrEmpty(email))
            {
                return await _context.Users
                .Include(u => u.Roles)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
            }
            return await _context.Users
                .Include(u => u.Roles)
                .Where(u => u.Email.Contains(email))
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync(); 
        }

        public async Task<List<User>> FindAllAdmin()
        {
            return await _context.Users
                .Where(u => u.Roles.Count == 2)
                .Include(u => u.Roles)
                .ToListAsync();
        }

        public async Task<List<User>> FindAllCustomer()
        {
            return await _context.Users
                .Where(u => u.Roles.Count == 1)
                .Include(u => u.Roles)
                .ToListAsync();
        }

        public async Task<List<User>> FindAllNoPagination(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return await _context.Users.Include(u => u.Roles)
                .ToListAsync();
            }
            return await _context.Users
                .Where(u => u.Email.Contains(email)).Include(u => u.Roles)
                .ToListAsync();
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _context.Users
                 .Include(u => u.Roles)
                 .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
