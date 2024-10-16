using Domain.Interfaces;
using Domain.Models;
using Moq;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBusinessCard.Services
{
    public class BusinessCardServiceTests
    {
        private readonly BusinessCardService _businessCardService;
        private readonly Mock<IBusinessCardRepository> _mockRepository;

        public BusinessCardServiceTests()
        {
            _mockRepository = new Mock<IBusinessCardRepository>();
            _businessCardService = new BusinessCardService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllBusinessCardsAsync_ShouldReturnAllCards()
        {
            // Arrange
            var businessCards = new List<BusinessCard>
            {
                new BusinessCard { Id = 1, Name = "John Doe", Gender = true, DateOfBirth = new DateTime(1990, 1, 1), Email = "john@example.com", Phone = "123456789", Address = "123 Main St" },
                new BusinessCard { Id = 2, Name = "Jane Doe", Gender = false, DateOfBirth = new DateTime(1985, 5, 10), Email = "jane@example.com", Phone = "987654321", Address = "456 Side St" }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(businessCards);

            // Act
            var result = await _businessCardService.GetAllBusinessCardsAsync();

            // Assert
            Assert.Equal(2, result.Count()); // Expecting 2 business cards
        }

        [Fact]
        public async Task GetBusinessCardByIdAsync_ShouldReturnBusinessCard_WhenCardExists()
        {
            // Arrange
            var businessCard = new BusinessCard
            {
                Id = 1,
                Name = "John Doe",
                Gender = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                Phone = "123456789",
                Address = "123 Main St"
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(businessCard);

            // Act
            var result = await _businessCardService.GetBusinessCardByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public async Task GetBusinessCardByIdAsync_ShouldReturnNull_WhenCardDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((BusinessCard)null);

            // Act
            var result = await _businessCardService.GetBusinessCardByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddBusinessCardAsync_ShouldCallRepositoryAdd()
        {
            // Arrange
            var businessCard = new BusinessCard
            {
                Id = 1,
                Name = "John Doe",
                Gender = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                Phone = "123456789",
                Address = "123 Main St"
            };

            // Act
            await _businessCardService.AddBusinessCardAsync(businessCard);

            // Assert
            _mockRepository.Verify(repo => repo.AddAsync(businessCard), Times.Once);
        }

        [Fact]
        public async Task DeleteBusinessCardAsync_ShouldCallRepositoryDelete()
        {
            // Arrange
            int cardId = 1;

            // Act
            await _businessCardService.DeleteBusinessCardAsync(cardId);

            // Assert
            _mockRepository.Verify(repo => repo.DeleteAsync(cardId), Times.Once);
        }

        [Fact]
        public async Task ExportBusinessCardByIdAsync_ShouldReturnCsvFileContent_WhenFormatIsCsv()
        {
            // Arrange
            var businessCard = new BusinessCard
            {
                Id = 1,
                Name = "John Doe",
                Gender = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                Phone = "123456789",
                Address = "123 Main St",
                Photo = "base64encodedphoto"
            };

            _mockRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(businessCard);

            // Act
            var result = await _businessCardService.ExportBusinessCardByIdAsync(1, 1); // Format 1 is CSV

            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/csv", result.mimeType);
            Assert.Contains("John Doe", Encoding.UTF8.GetString(result.fileContent));
        }

        [Fact]
        public async Task ExportBusinessCardByIdAsync_ShouldThrowException_WhenCardDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((BusinessCard)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _businessCardService.ExportBusinessCardByIdAsync(999, 1));
        }
    }
}
