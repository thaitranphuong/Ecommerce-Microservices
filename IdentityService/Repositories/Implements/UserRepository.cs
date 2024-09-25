using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<User> FindByEmail(string email)
        {
            return await _context.Users
                 .Include(u => u.Roles)
                 .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
