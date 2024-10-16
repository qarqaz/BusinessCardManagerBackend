using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public class BusinessCardRepository : IBusinessCardRepository
    {
        private readonly BusinessCardDbContext _context;

        public BusinessCardRepository(BusinessCardDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BusinessCard>> GetAllAsync()
        {
            return await _context.BusinessCards.ToListAsync();
        }

        public async Task<BusinessCard> GetByIdAsync(int id)
        {
            return await _context.BusinessCards.FindAsync(id);
        }

        public async Task AddAsync(BusinessCard businessCard)
        {
            await _context.BusinessCards.AddAsync(businessCard);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BusinessCard businessCard)
        {
            _context.BusinessCards.Update(businessCard);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var businessCard = await _context.BusinessCards.FindAsync(id);
            if (businessCard != null)
            {
                _context.BusinessCards.Remove(businessCard);
                await _context.SaveChangesAsync();
            }
        }
    }
}
