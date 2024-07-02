using System.Runtime.InteropServices.JavaScript;

namespace Url_Shortener.Services;

public class UrlShorteningService
{
    public const int NumbersOfCharsInShortLink = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

    private readonly Random _random = new Random();

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[NumbersOfCharsInShortLink];
        for (int i = 0; i < NumbersOfCharsInShortLink; i++)
        {
            int randIndex = _random.Next(NumbersOfCharsInShortLink - 1);
            codeChars[i] = Alphabet[randIndex];
        }

        var code = new string(codeChars);

        return code;
    }
}