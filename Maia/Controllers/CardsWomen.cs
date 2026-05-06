using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsWomenController : ControllerBase
    {
        private readonly ICardsWomenService _service;

        public CardsWomenController(ICardsWomenService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetByCategory(string category)
        {
            return Ok(await _service.GetByCategoryAsync(category));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCardsWomenDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
    }
}