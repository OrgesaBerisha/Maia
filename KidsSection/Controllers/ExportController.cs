using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using KidsSection.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace KidsSection.Controllers;

[ApiController]
[Route("api/export")]
public class ExportController : ControllerBase
{
    private readonly DataContext _context;

    public ExportController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("kids-products/csv")]
    public async Task<IActionResult> ExportCsv()
    {
        var products = await _context.KidsCards
            .Include(p => p.KidsCategory)
            .Include(p => p.KidsProductType)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Price,
                Category = p.KidsCategory != null ? p.KidsCategory.Name : "",
                ProductType = p.KidsProductType != null ? p.KidsProductType.Name : "",
                p.Description
            }).ToListAsync();

        var sb = new StringBuilder();
        await using var writer = new StringWriter(sb);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(products);

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "kids_products.csv");
    }

    [HttpGet("kids-products/excel")]
    public async Task<IActionResult> ExportExcel()
    {
        var products = await _context.KidsCards
            .Include(p => p.KidsCategory)
            .Include(p => p.KidsProductType)
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Kids Products");

        sheet.Cell(1, 1).Value = "Id";
        sheet.Cell(1, 2).Value = "Title";
        sheet.Cell(1, 3).Value = "Price";
        sheet.Cell(1, 4).Value = "Category";
        sheet.Cell(1, 5).Value = "Product Type";
        sheet.Cell(1, 6).Value = "Description";
        sheet.Row(1).Style.Font.Bold = true;

        for (int i = 0; i < products.Count; i++)
        {
            var p = products[i];
            sheet.Cell(i + 2, 1).Value = p.Id;
            sheet.Cell(i + 2, 2).Value = p.Title;
            sheet.Cell(i + 2, 3).Value = (double)p.Price;
            sheet.Cell(i + 2, 4).Value = p.KidsCategory?.Name ?? "";
            sheet.Cell(i + 2, 5).Value = p.KidsProductType?.Name ?? "";
            sheet.Cell(i + 2, 6).Value = p.Description;
        }

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "kids_products.xlsx");
    }

    [HttpGet("kids-products/json")]
    public async Task<IActionResult> ExportJson()
    {
        var products = await _context.KidsCards
            .Include(p => p.KidsCategory)
            .Include(p => p.KidsProductType)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Price,
                Category = p.KidsCategory != null ? p.KidsCategory.Name : "",
                ProductType = p.KidsProductType != null ? p.KidsProductType.Name : "",
                p.Description
            }).ToListAsync();

        return Ok(products);
    }

    [HttpPost("kids-products/import/csv")]
    public async Task<IActionResult> ImportCsv(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided.");
        var categories = await _context.KidsCategories.ToListAsync();
        var types = await _context.KidsProductTypes.ToListAsync();
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
            var typ = types.FirstOrDefault(t => t.Name.Equals(Val("ProductType"), StringComparison.OrdinalIgnoreCase)) ?? types.First();
            if (cat == null) continue;
            decimal.TryParse(Val("Price"), NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            _context.KidsCards.Add(new KidsCards
            {
                Title = title,
                Price = price,
                KidsCategoryId = cat.Id,
                KidsProductTypeId = typ.Id,
                Description = Val("Description"),
                ImageUrl = Val("ImageUrl")
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }

    [HttpPost("kids-products/import/excel")]
    public async Task<IActionResult> ImportExcel(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided.");
        var categories = await _context.KidsCategories.ToListAsync();
        var types = await _context.KidsProductTypes.ToListAsync();
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
            var typ = types.FirstOrDefault(t => t.Name.Equals(Get("ProductType"), StringComparison.OrdinalIgnoreCase)) ?? types.First();
            if (cat == null) continue;
            decimal.TryParse(Get("Price"), NumberStyles.Any, CultureInfo.InvariantCulture, out var price);

            _context.KidsCards.Add(new KidsCards
            {
                Title = title,
                Price = price,
                KidsCategoryId = cat.Id,
                KidsProductTypeId = typ.Id,
                Description = Get("Description"),
                ImageUrl = Get("ImageUrl")
            });
            imported++;
        }

        await _context.SaveChangesAsync();
        return Ok(new { imported });
    }
}
