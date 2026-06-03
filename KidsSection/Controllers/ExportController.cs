using ClosedXML.Excel;
using CsvHelper;
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
}
