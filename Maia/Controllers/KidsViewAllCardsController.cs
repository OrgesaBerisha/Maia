using Maia.Data.DTO;
using Maia.Data.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Maia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidsViewAllCardsController : ControllerBase
    {
        private readonly IKidsViewAllCards _service;

        public KidsViewAllCardsController(IKidsViewAllCards service)
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
        public async Task<IActionResult> Create(CreateKidsViewAllCardsDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }
    }
}
