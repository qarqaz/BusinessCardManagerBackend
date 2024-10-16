using API.Controllers;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBusinessCard.Controllers
{
    public class BusinessCardControllerTests
    {
        private readonly BusinessCardController _controller;
        private readonly Mock<IBusinessCardService> _mockService;

        public BusinessCardControllerTests()
        {
            _mockService = new Mock<IBusinessCardService>();
            _controller = new BusinessCardController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfBusinessCards()
        {
            // Arrange
            var businessCards = new List<BusinessCard>
            {
                new BusinessCard { Id = 1, Name = "John Doe", Gender = true, DateOfBirth = new DateTime(1990, 1, 1), Email = "john@example.com", Phone = "123456789", Address = "123 Main St" },
                new BusinessCard { Id = 2, Name = "Jane Doe", Gender = false, DateOfBirth = new DateTime(1985, 5, 10), Email = "jane@example.com", Phone = "987654321", Address = "456 Side St" }
            };

            _mockService.Setup(service => service.GetAllBusinessCardsAsync()).ReturnsAsync(businessCards);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCards = Assert.IsType<List<BusinessCard>>(okResult.Value);
            Assert.Equal(2, returnCards.Count);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WithValidId()
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

            _mockService.Setup(service => service.GetBusinessCardByIdAsync(1)).ReturnsAsync(businessCard);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnCard = Assert.IsType<BusinessCard>(okResult.Value);
            Assert.Equal("John Doe", returnCard.Name);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WithInvalidId()
        {
            // Arrange
            _mockService.Setup(service => service.GetBusinessCardByIdAsync(It.IsAny<int>())).ReturnsAsync((BusinessCard)null);

            // Act
            var result = await _controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WithValidBusinessCard()
        {
            // Arrange
            var newCard = new BusinessCard
            {
                Id = 1,
                Name = "John Doe",
                Gender = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                Phone = "123456789",
                Address = "123 Main St"
            };

            _mockService.Setup(service => service.AddBusinessCardAsync(newCard)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(newCard);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenUpdatedSuccessfully()
        {
            // Arrange
            var updatedCard = new BusinessCard
            {
                Id = 1,
                Name = "John Doe",
                Gender = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                Phone = "123456789",
                Address = "123 Main St"
            };

            _mockService.Setup(service => service.UpdateBusinessCardAsync(updatedCard)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, updatedCard);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var updatedCard = new BusinessCard
            {
                Id = 2,
                Name = "John Doe",
                Gender = true,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "john@example.com",
                Phone = "123456789",
                Address = "123 Main St"
            };

            // Act
            var result = await _controller.Update(1, updatedCard);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenDeletedSuccessfully()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteBusinessCardAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ExportBusinessCardByIdAsync_ShouldReturnFile_WhenSuccessful()
        {
            // Arrange
            byte[] fileContent = Encoding.UTF8.GetBytes("file content");
            string mimeType = "text/csv";
            string fileName = "BusinessCard_JohnDoe.csv";

            _mockService.Setup(service => service.ExportBusinessCardByIdAsync(1, 1))
                        .ReturnsAsync((fileContent, mimeType, fileName));

            // Act
            var result = await _controller.ExportBusinessCardByIdAsync(1, 1);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("text/csv", fileResult.ContentType);
            Assert.Equal("BusinessCard_JohnDoe.csv", fileResult.FileDownloadName);
        }

        [Fact]
        public async Task ExportBusinessCardByIdAsync_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            _mockService.Setup(service => service.ExportBusinessCardByIdAsync(999, 1))
                        .ThrowsAsync(new Exception("Business card not found."));

            // Act
            var result = await _controller.ExportBusinessCardByIdAsync(999, 1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var badRequestMessage = Assert.IsType<string>(badRequestResult.Value);

            // Assert that the message matches
            Assert.Equal("Business card not found.", badRequestMessage);
        }
    }
}
