using Microsoft.EntityFrameworkCore;

namespace Lkek.Server;

public class AppDbContext : DbContext
{
    public DbSet<ImageRecord> Images { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}

public class ImageRecord
{
    public int Id { get; set; }
    public string Path { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "portrait" or "outfit"
}