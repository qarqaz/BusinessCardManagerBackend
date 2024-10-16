using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBusinessCardRepository
    {
        Task<IEnumerable<BusinessCard>> GetAllAsync();
        Task<BusinessCard> GetByIdAsync(int id);
        Task AddAsync(BusinessCard businessCard);
        Task UpdateAsync(BusinessCard businessCard);
        Task DeleteAsync(int id);
    }
}
