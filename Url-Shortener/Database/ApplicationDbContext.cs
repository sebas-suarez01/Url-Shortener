using Microsoft.EntityFrameworkCore;
using Url_Shortener.Models;
using Url_Shortener.Services;

namespace Url_Shortener.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Code).HasMaxLength(UrlShorteningService.NumbersOfCharsInShortLink);
            builder.HasIndex(b => b.Code).IsUnique();
        });
    }
}