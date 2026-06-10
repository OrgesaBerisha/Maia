using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using Maia.Data;
using Maia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace WomensSection.Controllers;

[ApiController]
[Route("api/export")]
public class ExportController : ControllerBase
{
    private readonly DataContext _context;

    public ExportController(DataContext context)
    {
        _context = context;
    }

    // ── WOMEN PRODUCTS ──────────────────────────────────────────

    [HttpGet("women-products/csv")]
    public async Task<IActionResult> ExportProductsCsv()
    {
        var products = await _context.CardsWoman
            .Include(p => p.WomanCategory)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Price,
                Category = p.WomanCategory != null ? p.WomanCategory.Name : "",
                p.Description,
                p.CreatedAt
            }).ToListAsync();

        var sb = new StringBuilder();
        await using var writer = new StringWriter(sb);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(products);

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "women_products.csv");
    }

    [HttpGet("women-products/excel")]
    public async Task<IActionResult> ExportProductsExcel()
    {
        var products = await _context.CardsWoman
            .Include(p => p.WomanCategory)
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Women Products");

        sheet.Cell(1, 1).Value = "Id";
        sheet.Cell(1, 2).Value = "Title";
        sheet.Cell(1, 3).Value = "Price";
        sheet.Cell(1, 4).Value = "Category";
        sheet.Cell(1, 5).Value = "Description";
        sheet.Cell(1, 6).Value = "Created At";
        sheet.Row(1).Style.Font.Bold = true;

        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            sheet.Cell(i + 2, 1).Value = p.Id;
            sheet.Cell(i + 2, 2).Value = p.Title;
            sheet.Cell(i + 2, 3).Value = (double)p.Price;
            sheet.Cell(i + 2, 4).Value = p.WomanCategory?.Name ?? "";
            sheet.Cell(i + 2, 5).Value = p.Description;
            sheet.Cell(i + 2, 6).Value = p.CreatedAt.ToString("yyyy-MM-dd");
        }

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "women_products.xlsx");
    }

    [HttpGet("women-products/json")]
    public async Task<IActionResult> ExportProductsJson()
    {
        var products = await _context.CardsWoman
            .Include(p => p.WomanCategory)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Price,
                Category = p.WomanCategory != null ? p.WomanCategory.Name : "",
                p.Description,
                p.CreatedAt
            }).ToListAsync();

        return Ok(products);
    }

    // ── ORDERS ──────────────────────────────────────────────────

    [HttpGet("orders/csv")]
    public async Task<IActionResult> ExportOrdersCsv()
    {
        var orders = await _context.Orders
            .Select(o => new
            {
                o.Id,
                o.UserId,
                o.TotalPrice,
                CreatedAt = o.CreatedAt.ToString("yyyy-MM-dd HH:mm")
            }).ToListAsync();

        var sb = new StringBuilder();
        await using var writer = new StringWriter(sb);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(orders);

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "orders.csv");
    }

    [HttpGet("orders/excel")]
    public async Task<IActionResult> ExportOrdersExcel()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Orders");

        sheet.Cell(1, 1).Value = "Order Id";
        sheet.Cell(1, 2).Value = "User Id";
        sheet.Cell(1, 3).Value = "Total Price";
        sheet.Cell(1, 4).Value = "Items Count";
        sheet.Cell(1, 5).Value = "Created At";
        sheet.Row(1).Style.Font.Bold = true;

        for (int i = 0; i < orders.Count; i++)
        {
            var o = orders[i];
            sheet.Cell(i + 2, 1).Value = o.Id;
            sheet.Cell(i + 2, 2).Value = o.UserId;
            sheet.Cell(i + 2, 3).Value = (double)o.TotalPrice;
            sheet.Cell(i + 2, 4).Value = o.OrderItems.Count;
            sheet.Cell(i + 2, 5).Value = o.CreatedAt.ToString("yyyy-MM-dd HH:mm");
        }

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "orders.xlsx");
    }

    [HttpGet("orders/json")]
    public async Task<IActionResult> ExportOrdersJson()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .Select(o => new
            {
                o.Id,
                o.UserId,
                o.TotalPrice,
                o.CreatedAt,
                Items = o.OrderItems.Select(i => new { i.ProductId, i.Quantity, i.Price })
            }).ToListAsync();

        return Ok(orders);
    }

    [HttpPost("women-products/import/json")]
    public async Task<IActionResult> ImportJson([FromBody] List<ImportProductDto> items)
    {
        if (items == null || items.Count == 0) return BadRequest("No items provided.");
        var categories = await _context.WomanCategories.ToListAsync();
        int imported = 0;

        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Title)) continue;
            var cat = categories.FirstOrDefault(c => c.Name.Equals(item.Category, StringComparison.OrdinalIgnoreCase));
            if (cat == null) continue;

            _context.CardsWoman.Add(new CardsWomen
            {
                Title = item.Title,
                Price = item.Price,
                WomanCategoryId = cat.Id,
                Description = item.Description ?? "",
                ImageUrl = item.ImageUrl ?? "",
                CreatedAt = DateTime.UtcNow
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }

    [HttpPost("women-products/import/csv")]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided.");
        var categories = await _context.WomanCategories.ToListAsync();
        int imported = 0;

        using var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null, MissingFieldFound = null };
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<dynamic>().ToList();

        foreach (var r in records)
        {
            var dict = (IDictionary<string, object>)r;
            string Val(string k) => dict.ContainsKey(k) ? dict[k]?.ToString() ?? "" : "";
            var title = Val("Title");
            if (string.IsNullOrWhiteSpace(title)) continue;
            var cat = categories.FirstOrDefault(c => c.Name.Equals(Val("Category"), StringComparison.OrdinalIgnoreCase));
            if (cat == null) continue;
            decimal.TryParse(Val("Price"), NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            _context.CardsWoman.Add(new CardsWomen
            {
                Title = title,
                Price = price,
                WomanCategoryId = cat.Id,
                Description = Val("Description"),
                ImageUrl = Val("ImageUrl"),
                CreatedAt = DateTime.UtcNow
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }

    [HttpPost("orders/import/json")]
    public async Task<IActionResult> ImportOrdersJson([FromBody] List<ImportOrderDto> items)
    {
        if (items == null || items.Count == 0) return BadRequest("No items provided.");
        int imported = 0;

        foreach (var item in items)
        {
            _context.Orders.Add(new Order
            {
                UserId = item.UserId,
                TotalPrice = item.TotalPrice,
                CreatedAt = DateTime.UtcNow
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }

    [HttpPost("women-products/import/excel")]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided.");
        var categories = await _context.WomanCategories.ToListAsync();
        int imported = 0;

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        using var workbook = new XLWorkbook(stream);
        var sheet = workbook.Worksheets.First();
        var headers = sheet.Row(1).CellsUsed().Select(c => c.Value.ToString()).ToList();

        for (int row = 2; row <= sheet.LastRowUsed()?.RowNumber(); row++)
        {
            string Get(string col) { var idx = headers.IndexOf(col); return idx >= 0 ? sheet.Cell(row, idx + 1).Value.ToString() : ""; }
            var title = Get("Title");
            if (string.IsNullOrWhiteSpace(title)) continue;
            var cat = categories.FirstOrDefault(c => c.Name.Equals(Get("Category"), StringComparison.OrdinalIgnoreCase));
            if (cat == null) continue;
            decimal.TryParse(Get("Price"), NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            _context.CardsWoman.Add(new CardsWomen
            {
                Title = title,
                Price = price,
                WomanCategoryId = cat.Id,
                Description = Get("Description"),
                ImageUrl = Get("ImageUrl"),
                CreatedAt = DateTime.UtcNow
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }
}

public class ImportProductDto
{
    public string Title { get; set; } = "";
    public decimal Price { get; set; }
    public string Category { get; set; } = "";
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}

public class ImportOrderDto
{
    public int UserId { get; set; }
    public decimal TotalPrice { get; set; }
}
