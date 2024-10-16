using Domain.Interfaces;
using Domain.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BusinessCardService : IBusinessCardService
    {
        private readonly IBusinessCardRepository _businessCardRepository;

        public BusinessCardService(IBusinessCardRepository businessCardRepository)
        {
            _businessCardRepository = businessCardRepository;
        }

        public async Task<IEnumerable<BusinessCard>> GetAllBusinessCardsAsync()
        {
            return await _businessCardRepository.GetAllAsync();
        }

        public async Task<BusinessCard> GetBusinessCardByIdAsync(int id)
        {
            return await _businessCardRepository.GetByIdAsync(id);
        }

        public async Task AddBusinessCardAsync(BusinessCard businessCard)
        {
            await _businessCardRepository.AddAsync(businessCard);
        }

        public async Task UpdateBusinessCardAsync(BusinessCard businessCard)
        {
            await _businessCardRepository.UpdateAsync(businessCard);
        }

        public async Task DeleteBusinessCardAsync(int id)
        {
            await _businessCardRepository.DeleteAsync(id);
        }

        public async Task<(byte[] fileContent, string mimeType, string fileName)> ExportBusinessCardByIdAsync(int id, int format)
        {
            var businessCard = await _businessCardRepository.GetByIdAsync(id);
            if (businessCard == null)
            {
                throw new Exception("Business card not found.");
            }

            var gender = businessCard.Gender ? "1" : "2";


            if (format == 1) // CSV
            {
                string photoData = businessCard.Photo == null ? "" : businessCard.Photo;
                if (photoData.Contains(","))
                {
                    photoData = photoData.Substring(photoData.IndexOf(",") + 1);
                }
                var csvContent = $"Name,Gender,Date of Birth,Email,Phone,Address,Photo\n" +
                                 $"{businessCard.Name},{gender},{businessCard.DateOfBirth:MM/dd/yyyy},{businessCard.Email}," +
                                 $"{businessCard.Phone},{businessCard.Address},{photoData}";

                byte[] fileContent = Encoding.UTF8.GetBytes(csvContent);
                return (fileContent, "text/csv", $"BusinessCard_{businessCard.Name}.csv");
            }
            else if (format == 2) // XML
            {
                var xmlContent = $"<BusinessCard>\n" +
                                 $"  <Name>{businessCard.Name}</Name>\n" +
                                 $"  <Gender>{gender}</Gender>\n" +
                                 $"  <DateOfBirth>{businessCard.DateOfBirth:MM/dd/yyyy}</DateOfBirth>\n" +
                                 $"  <Email>{businessCard.Email}</Email>\n" +
                                 $"  <Phone>{businessCard.Phone}</Phone>\n" +
                                 $"  <Address>{businessCard.Address}</Address>\n" +
                                 $"  <Photo>{businessCard.Photo}</Photo>\n" +
                                 $"</BusinessCard>";

                byte[] fileContent = Encoding.UTF8.GetBytes(xmlContent);
                return (fileContent, "application/xml", $"BusinessCard_{businessCard.Name}.xml");
            }
            else
            {
                throw new ArgumentException("Invalid format. Use 1 for CSV and 2 for XML.");
            }
        }

    }
}
