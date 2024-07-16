using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Repositories.Implements
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly AppDbContext _context;

        public VoucherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOne(Voucher voucher)
        {
            await _context.Vouchers.AddAsync(voucher);
            return await _context.SaveChangesAsync();
        }

        public async Task<Voucher> FindById(int id)
        {
            return await _context.Vouchers.FindAsync(id);
        }

        public async Task<List<Voucher>> FindAll(string name, int page, int limit)
        {
            if (string.IsNullOrEmpty(name))
                return await _context.Vouchers
                    .Where(v => v.IsRemoved == false)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

            return await _context.Vouchers
                    .Where(v => v.IsRemoved == false && v.Name.Contains(name))
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
        }

        public async Task<List<Voucher>> FindAllOfCustomerPage(string name, int page, int limit)
        {
            if (string.IsNullOrEmpty(name))
                return await _context.Vouchers
                    .Where(v => v.IsRemoved == false
                                && v.StartTime <= DateTime.Now
                                && v.EndTime >= DateTime.Now)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

            return await _context.Vouchers
                    .Where(v => v.IsRemoved == false
                                && v.StartTime <= DateTime.Now
                                && v.EndTime >= DateTime.Now
                                && v.Name.Contains(name))
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();
        }

        public async Task<int> SaveChange()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Remove(int id)
        {
            var voucher = _context.Vouchers.Find(id);
            voucher.IsRemoved = true;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Voucher>> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await _context.Vouchers
                    .Where(v => v.IsRemoved == false)
                    .ToListAsync();

            return await _context.Vouchers
                    .Where(v => v.IsRemoved == false && v.Name.Contains(name))
                    .ToListAsync();
        }

        public async Task<List<Voucher>> FindByNameOfCustomerPage(string name)
        {
            if (string.IsNullOrEmpty(name))
                return await _context.Vouchers
                    .Where(v => v.IsRemoved == false
                                && v.StartTime <= DateTime.Now
                                && v.EndTime >= DateTime.Now)
                    .ToListAsync();

            return await _context.Vouchers
                    .Where(v => v.IsRemoved == false
                                && v.StartTime <= DateTime.Now
                                && v.EndTime >= DateTime.Now
                                && v.Name.Contains(name))
                    .ToListAsync();
        }
    }
}
