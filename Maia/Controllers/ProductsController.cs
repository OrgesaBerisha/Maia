using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Maia.Models.NoSQL;
using Maia.Data.Interface;

namespace Maia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IAuditLogRepository _auditLogRepository;

    public ProductsController(
        IProductRepository productRepository,
        IAuditLogRepository auditLogRepository)
    {
        _productRepository = productRepository;
        _auditLogRepository = auditLogRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productRepository.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        var products = await _productRepository.GetByCategoryAsync(category);
        return Ok(products);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        var products = await _productRepository.SearchAsync(q);
        return Ok(products);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] ProductDocument product)
    {
        product.CreatedBy = User.Identity?.Name;
        product.UpdatedBy = User.Identity?.Name;

        var created = await _productRepository.CreateAsync(product);

        await _auditLogRepository.LogAsync(new AuditLogDocument
        {
            UserId = int.TryParse(User.FindFirst("userId")?.Value, out var uid) ? uid : null,
            Action = "CREATE",
            Entity = "Product",
            EntityId = created.Id,
            IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            CreatedBy = User.Identity?.Name
        });

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(string id, [FromBody] ProductDocument product)
    {
        if (!await _productRepository.ExistsAsync(id))
            return NotFound();

        product.UpdatedBy = User.Identity?.Name;
        await _productRepository.UpdateAsync(id, product);

        await _auditLogRepository.LogAsync(new AuditLogDocument
        {
            Action = "UPDATE",
            Entity = "Product",
            EntityId = id,
            CreatedBy = User.Identity?.Name
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!await _productRepository.ExistsAsync(id))
            return NotFound();

        await _productRepository.DeleteAsync(id);

        await _auditLogRepository.LogAsync(new AuditLogDocument
        {
            Action = "DELETE",
            Entity = "Product",
            EntityId = id,
            CreatedBy = User.Identity?.Name
        });

        return NoContent();
    }
}
