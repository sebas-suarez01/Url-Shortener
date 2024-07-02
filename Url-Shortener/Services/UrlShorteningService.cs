using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;
using Url_Shortener.Database;

namespace Url_Shortener.Services;

public class UrlShorteningService
{
    public const int NumbersOfCharsInShortLink = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

    private readonly Random _random = new Random();
    private readonly ApplicationDbContext _context;

    public UrlShorteningService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateUniqueCode()
    {
        while (true)
        {
            var codeChars = new char[NumbersOfCharsInShortLink];
            for (int i = 0; i < NumbersOfCharsInShortLink; i++)
            {
                int randIndex = _random.Next(NumbersOfCharsInShortLink - 1);
                codeChars[i] = Alphabet[randIndex];
            }
            var code = new string(codeChars);

            if (!(await _context.ShortenedUrls.AnyAsync(s => s.Code == code)))
            {
                return code;
            }
        }
        
    }
}