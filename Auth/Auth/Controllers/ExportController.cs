using Auth.Data;
using ClosedXML.Excel;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Auth.Controllers;

[ApiController]
[Route("api/export")]
[Authorize(Roles = "Admin")]
public class ExportController : ControllerBase
{
    private readonly DataContext _context;

    public ExportController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("users/csv")]
    public async Task<IActionResult> ExportUsersCsv()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Select(u => new
            {
                u.UserID,
                u.FirstName,
                u.LastName,
                u.Email,
                Role = u.Role != null ? u.Role.RoleType.ToString() : "",
                u.IsActive,
                u.CreatedAt
            }).ToListAsync();

        var sb = new StringBuilder();
        await using var writer = new StringWriter(sb);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(users);

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "users.csv");
    }

    [HttpGet("users/excel")]
    public async Task<IActionResult> ExportUsersExcel()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Users");

        sheet.Cell(1, 1).Value = "Id";
        sheet.Cell(1, 2).Value = "First Name";
        sheet.Cell(1, 3).Value = "Last Name";
        sheet.Cell(1, 4).Value = "Email";
        sheet.Cell(1, 5).Value = "Role";
        sheet.Cell(1, 6).Value = "Active";
        sheet.Cell(1, 7).Value = "Created At";
        sheet.Row(1).Style.Font.Bold = true;

        for (int i = 0; i < users.Count; i++)
        {
            var u = users[i];
            sheet.Cell(i + 2, 1).Value = u.UserID;
            sheet.Cell(i + 2, 2).Value = u.FirstName;
            sheet.Cell(i + 2, 3).Value = u.LastName;
            sheet.Cell(i + 2, 4).Value = u.Email;
            sheet.Cell(i + 2, 5).Value = u.Role?.RoleType.ToString() ?? "";
            sheet.Cell(i + 2, 6).Value = u.IsActive ? "Yes" : "No";
            sheet.Cell(i + 2, 7).Value = u.CreatedAt.ToString("yyyy-MM-dd");
        }

        sheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "users.xlsx");
    }

    [HttpGet("users/json")]
    public async Task<IActionResult> ExportUsersJson()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Select(u => new
            {
                u.UserID,
                u.FirstName,
                u.LastName,
                u.Email,
                Role = u.Role != null ? u.Role.RoleType.ToString() : "",
                u.IsActive,
                u.CreatedAt
            }).ToListAsync();

        return Ok(users);
    }
}
