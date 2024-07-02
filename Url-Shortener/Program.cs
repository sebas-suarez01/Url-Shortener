using Microsoft.EntityFrameworkCore;
using Url_Shortener.Database;
using Url_Shortener.Models;
using Url_Shortener.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddScoped<UrlShorteningService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.ApplyMigrations();
}

app.MapPost("api/short-url",
    async (string url,
        UrlShorteningService service,
        ApplicationDbContext context,
        HttpContext httpContext) =>
{
    if (!Uri.TryCreate(url, UriKind.Absolute, out _))
    {
        return Results.BadRequest();
    }

    var code = await service.GenerateUniqueCode();

    var shortenedUrl = new ShortenedUrl()
    {
        Id = Guid.NewGuid(),
        Code = code,
        LongUrl = url,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
        CreatedOnUtc = DateTime.UtcNow
    };

    await context.ShortenedUrls.AddAsync(shortenedUrl);

    await context.SaveChangesAsync();
    
    return Results.Ok(shortenedUrl.ShortUrl);
});


app.UseHttpsRedirection();

app.Run();
