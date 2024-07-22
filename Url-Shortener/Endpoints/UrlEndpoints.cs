using Microsoft.EntityFrameworkCore;
using Url_Shortener.Database;
using Url_Shortener.Database.Repository;
using Url_Shortener.Models;
using Url_Shortener.Services;

namespace Url_Shortener.Endpoints;

public static class UrlEndpoints
{
    public static void AddEndpoints(this WebApplication app)
    {
        app.MapPost("api/short-url",
            async (string url, UrlRepository repository) =>
            {
                if (!Uri.TryCreate(url, UriKind.Absolute, out _))
                {
                    return Results.BadRequest();
                }

                var shortUrl = await repository.ShortUrl(url);
    
                return Results.Ok(shortUrl);
            });

        app.MapGet("api/{code}", async (string code, UrlRepository repository) =>
        {
            var longUrl = await repository.LongUrl(code);

            return longUrl is null ? Results.NotFound() : Results.Redirect(longUrl);
        });

        app.MapGet("api", async (ApplicationDbContext context) =>
        {
            var results = await context.ShortenedUrls.ToListAsync();

            return Results.Ok(results);
        });
    }

}