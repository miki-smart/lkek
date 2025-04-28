using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace Lkek.Server.Controllers;

[ApiController]
[Route("api")]
public class TryOnController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;

    public TryOnController(AppDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    [HttpPost("upload-portrait")]
    public async Task<IActionResult> UploadPortrait(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var path = Path.Combine("uploads", fileName);
        Directory.CreateDirectory("uploads");

        using (var stream = new FileStream(path, FileMode.Create))
            await file.CopyToAsync(stream);

        var record = new ImageRecord { Path = path, Type = "portrait" };
        _context.Images.Add(record);
        await _context.SaveChangesAsync();

        return Ok(new { path });
    }

    [HttpPost("upload-outfit")]
    public async Task<IActionResult> UploadOutfit(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var path = Path.Combine("uploads", fileName);
        Directory.CreateDirectory("uploads");

        using (var stream = new FileStream(path, FileMode.Create))
            await file.CopyToAsync(stream);

        var record = new ImageRecord { Path = path, Type = "outfit" };
        _context.Images.Add(record);
        await _context.SaveChangesAsync();

        return Ok(new { path });
    }

    [HttpPost("generate-try-on")]
    public async Task<IActionResult> GenerateTryOn([FromBody] TryOnRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5000/try-on", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TryOnResponse>();
            return Ok(new { resultImageUrl = result?.Result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "AI service failed: " + ex.Message);
        }
    }
}

public class TryOnRequest
{
    public string Portrait { get; set; } = string.Empty;
    public string Outfit { get; set; } = string.Empty;
}

public class TryOnResponse
{
    public string Result { get; set; } = string.Empty;
}