using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories.Implements
{
    public class UnitRepository : IUnitRepository
    {
        private readonly AppDbContext _context;

        public UnitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Unit unit)
        {
            await _context.Units.AddAsync(unit);
            return await _context.SaveChangesAsync();
        }

        public async Task<Unit> FindById(int id)
        {
            return await _context.Units.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Unit>> FindAll(string name, int page, int limit)
        {
            if (string.IsNullOrEmpty(name))
                return await _context.Units
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            return await _context.Units
                .Where(c => c.Name.Contains(name))
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(Unit unit)
        {
            _context.Units.Remove(unit);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Unit>> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await _context.Units
                .ToListAsync();

            return await _context.Units
                .Where(c => c.Name.Contains(name))
                .ToListAsync();
        }
    }
}
