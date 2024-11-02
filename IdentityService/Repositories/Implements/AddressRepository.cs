using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Repositories.Implements
{
    public class AddressRepository : IAddressRepository
    {
        private readonly MyDbContext _context;
        public AddressRepository(MyDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateOne(Address address)
        {
            _context.Add(address);
            await _context.SaveChangesAsync();
            return address.Id;
        }

        public async Task<List<Address>> FindAllByUserId(string userId)
        {
            return await _context.Addresses.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Address> FindById(int id)
        {
            return await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> Remove(int id)
        {
            var address = _context.Addresses.FirstOrDefault(x => x.Id == id);
            _context.Addresses.Remove(address);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
