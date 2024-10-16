using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBusinessCardService
    {
        Task<IEnumerable<BusinessCard>> GetAllBusinessCardsAsync();
        Task<BusinessCard> GetBusinessCardByIdAsync(int id);
        Task AddBusinessCardAsync(BusinessCard businessCard);
        Task UpdateBusinessCardAsync(BusinessCard businessCard);
        Task DeleteBusinessCardAsync(int id);
        Task<(byte[] fileContent, string mimeType, string fileName)> ExportBusinessCardByIdAsync(int id, int format);
    }
}
