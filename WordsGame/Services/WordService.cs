using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WordsGame.Data;
using WordsGame.Enums;

namespace WordsGame.Services;

public class WordService : IWordService
{
    private readonly IMemoryCache _cache;
    private readonly AppDbContext _context;
    private readonly ILogger<WordService> _logger;

    public WordService(IMemoryCache cache, AppDbContext context, ILogger<WordService> logger)
    {
        _cache = cache;
        _context = context;
        _logger = logger;
    }

    public async Task<List<string>?> GetWordsByLanguageAndLetterAsync(Language language, string firstLetter)
    {
        var cacheKey = $"{language}_{firstLetter.ToUpper()}";

        if (_cache.TryGetValue(cacheKey, out List<string>? words)) return words;
        _logger.LogInformation("Cache miss for {CacheKey}, querying DB...", cacheKey);

        words = await _context.Words
            .Where(w => w.Language == language && w.FirstLetter.ToUpper() == firstLetter.ToUpper())
            .Select(w => w.Value)
            .ToListAsync();

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSize(1)
            .SetSlidingExpiration(TimeSpan.FromMinutes(30));

        _cache.Set(cacheKey, words, cacheOptions);

        return words;
    }
}