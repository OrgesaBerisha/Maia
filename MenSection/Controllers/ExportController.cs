using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using MenSection.Data;
using MenSection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace MenSection.Controllers;

[ApiController]
[Route("api/export")]
public class ExportController : ControllerBase
{
    private readonly DataContext _context;

    public ExportController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("men-products/csv")]
    public async Task<IActionResult> ExportCsv()
    {
        var products = await _context.MenCards
            .Include(p => p.MenCategory)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Price,
                Category = p.MenCategory != null ? p.MenCategory.Name : "",
                p.Description
            }).ToListAsync();

        var sb = new StringBuilder();
        await using var writer = new StringWriter(sb);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(products);

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "men_products.csv");
    }

    [HttpGet("men-products/excel")]
    public async Task<IActionResult> ExportExcel()
    {
        var products = await _context.MenCards
            .Include(p => p.MenCategory)
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Men Products");

        sheet.Cell(1, 1).Value = "Id";
        sheet.Cell(1, 2).Value = "Title";
        sheet.Cell(1, 3).Value = "Price";
        sheet.Cell(1, 4).Value = "Category";
        sheet.Cell(1, 5).Value = "Description";
        sheet.Row(1).Style.Font.Bold = true;

        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            sheet.Cell(i + 2, 1).Value = p.Id;
            sheet.Cell(i + 2, 2).Value = p.Title;
            sheet.Cell(i + 2, 3).Value = (double)p.Price;
            sheet.Cell(i + 2, 4).Value = p.MenCategory?.Name ?? "";
            sheet.Cell(i + 2, 5).Value = p.Description;
        }

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "men_products.xlsx");
    }

    [HttpGet("men-products/json")]
    public async Task<IActionResult> ExportJson()
    {
        var products = await _context.MenCards
            .Include(p => p.MenCategory)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Price,
                Category = p.MenCategory != null ? p.MenCategory.Name : "",
                p.Description
            }).ToListAsync();

        return Ok(products);
    }

    [HttpPost("men-products/import/json")]
    public async Task<IActionResult> ImportJson([FromBody] List<ImportMenProductDto> items)
    {
        if (items == null || items.Count == 0) return BadRequest("No items provided.");
        var categories = await _context.MenCategories.ToListAsync();
        int imported = 0;

        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Title)) continue;
            var cat = categories.FirstOrDefault(c => c.Name.Equals(item.Category, StringComparison.OrdinalIgnoreCase));
            if (cat == null) continue;

            _context.MenCards.Add(new MenCards
            {
                Title = item.Title,
                Price = item.Price,
                MenCategoryId = cat.Id,
                Description = item.Description ?? "",
                ImageUrl = item.ImageUrl ?? ""
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }

    [HttpPost("men-products/import/csv")]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided.");
        var categories = await _context.MenCategories.ToListAsync();
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

            _context.MenCards.Add(new MenCards
            {
                Title = title,
                Price = price,
                MenCategoryId = cat.Id,
                Description = Val("Description"),
                ImageUrl = Val("ImageUrl")
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }

    [HttpPost("men-products/import/excel")]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided.");
        var categories = await _context.MenCategories.ToListAsync();
        int imported = 0;

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        using var workbook = new XLWorkbook(stream);
        var sheet = workbook.Worksheets.First();
        var headers = sheet.Row(1).CellsUsed().Select(c => c.Value.ToString()).ToList();

        for (int row = 2; row <= sheet.LastRowUsed()?.RowNumber(); row++)
        {
            string Get(string col) => sheet.Cell(row, headers.IndexOf(col) + 1).Value.ToString();
            var title = Get("Title");
            if (string.IsNullOrWhiteSpace(title)) continue;
            var cat = categories.FirstOrDefault(c => c.Name.Equals(Get("Category"), StringComparison.OrdinalIgnoreCase));
            if (cat == null) continue;
            decimal.TryParse(Get("Price"), NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            _context.MenCards.Add(new MenCards
            {
                Title = title,
                Price = price,
                MenCategoryId = cat.Id,
                Description = Get("Description"),
                ImageUrl = Get("ImageUrl")
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }
}

public class ImportMenProductDto
{
    public string Title { get; set; } = "";
    public decimal Price { get; set; }
    public string Category { get; set; } = "";
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}
