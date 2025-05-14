using WordsGame.Data;
using WordsGame.Enums;
using WordsGame.Models;

namespace WordsGame.Utils;

public class WordImporter(AppDbContext context)
{
    public async Task ImportWordsFromFile(string filePath, Language language)
    {
        var lines = await File.ReadAllLinesAsync(filePath);
        var words = lines
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => new Word
            {
                Value = line.Trim().ToLower(),
                FirstLetter = line[0].ToString().ToLower(),
                Language = language
            });

        context.Words.AddRange(words);
        await context.SaveChangesAsync();
    }
}