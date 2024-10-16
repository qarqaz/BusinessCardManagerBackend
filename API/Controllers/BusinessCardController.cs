using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardController : ControllerBase
    {
        private readonly IBusinessCardService _businessCardService;

        public BusinessCardController(IBusinessCardService businessCardService)
        {
            _businessCardService = businessCardService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusinessCard>>> GetAll()
        {
            var cards = await _businessCardService.GetAllBusinessCardsAsync();
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessCard>> GetById(int id)
        {
            var card = await _businessCardService.GetBusinessCardByIdAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] BusinessCard businessCard)
        {
            await _businessCardService.AddBusinessCardAsync(businessCard);
            return CreatedAtAction(nameof(GetById), new { id = businessCard.Id }, businessCard);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] BusinessCard businessCard)
        {
            if (id != businessCard.Id)
            {
                return BadRequest();
            }
            await _businessCardService.UpdateBusinessCardAsync(businessCard);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _businessCardService.DeleteBusinessCardAsync(id);
            return NoContent();
        }

        [HttpGet("export/{id}/{format}")]
        public async Task<IActionResult> ExportBusinessCardByIdAsync(int id, int format)
        {
            try
            {
                var (fileContent, mimeType, fileName) = await _businessCardService.ExportBusinessCardByIdAsync(id, format);

                return File(fileContent, mimeType, fileName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
