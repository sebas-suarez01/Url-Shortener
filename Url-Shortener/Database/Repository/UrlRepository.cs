using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Url_Shortener.Models;
using Url_Shortener.Services;

namespace Url_Shortener.Database.Repository;

public class UrlRepository
{
    private readonly UrlShorteningService _service;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMemoryCache _cache;

    public UrlRepository(UrlShorteningService service, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMemoryCache cache)
    {
        _service = service;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _cache = cache;
    }

    public async Task<string> ShortUrl(string url)
    {
        var code = await _service.GenerateUniqueCode();

        var shortenedUrl = new ShortenedUrl()
        {
            Id = Guid.NewGuid(),
            Code = code,
            LongUrl = url,
            ShortUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/{code}",
            CreatedOnUtc = DateTime.UtcNow
        };

        await _context.ShortenedUrls.AddAsync(shortenedUrl);

        await _context.SaveChangesAsync();

        return shortenedUrl.ShortUrl;
    }

    public async Task<string?> LongUrl(string code)
    {
        string key = $"{code}";

        var shortenedUrl = await _cache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                return _context.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);
            })!;

        return shortenedUrl?.LongUrl;
    }
}