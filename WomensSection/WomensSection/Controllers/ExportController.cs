using ClosedXML.Excel;
using CsvHelper;
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
}
