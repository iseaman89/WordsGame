using WordsGame.Data;
using WordsGame.Dtos;
using WordsGame.Dtos.WordCheck;

namespace WordsGame.Services;

public class WordCheckerService : IWordCheckerService
{
    private readonly IWordService _wordService;

    public WordCheckerService(IWordService wordService)
    {
        _wordService = wordService;
    }

    public async Task<CheckedWordsDto> CheckWordsOnlineGameAsync(WordCheckRequest words, string userId)
    {
        var groupedByFirstLetter = GroupWordsByFirstLetter(words.Words);
        var results = new List<WordResultOnlineGame>();
        
        foreach (var (firstLetter, wordGroup) in groupedByFirstLetter)
        {
            var wordsFromDb = await _wordService.GetWordsByLanguageAndLetterAsync(words.Language, firstLetter.ToString());

            var upperWordsFromDb = wordsFromDb?.Select(w => w.ToUpper()).ToHashSet() ?? new HashSet<string>();

            foreach (var word in wordGroup)
            {
                var isValid = upperWordsFromDb.Contains(word);
                results.Add(new WordResultOnlineGame { Word = word, IsValid = isValid});
            }
        }

        return new CheckedWordsDto
        {
            UserId = userId,
            WordResults = results
        };
    }

    public async Task<WordCheckResponse> CheckWordsAsync(WordCheckRequest words)
    {
        var groupedByFirstLetter = GroupWordsByFirstLetter(words.Words);

        var results = new List<WordCheckResult>();
        string? longestWord = null;

        foreach (var (firstLetter, wordGroup) in groupedByFirstLetter)
        {
            var wordsFromDb = await _wordService.GetWordsByLanguageAndLetterAsync(words.Language, firstLetter.ToString());

            var upperWordsFromDb = wordsFromDb?.Select(w => w.ToUpper()).ToHashSet() ?? new HashSet<string>();

            foreach (var word in wordGroup)
            {
                bool isValid = upperWordsFromDb.Contains(word);
                results.Add(new WordCheckResult { Word = word, IsValid = isValid, Points = isValid ? 5 : 0});

                if (isValid && word.Length >= 5)
                {
                    if (longestWord == null || word.Length > longestWord.Length)
                        longestWord = word;
                }
            }
        }

        return new WordCheckResponse
        {
            Results = results,
            LongestWord = longestWord
        };
    }

    private Dictionary<char, List<string>> GroupWordsByFirstLetter(List<string> words)
    {
        var normalizedWords = words
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Select(w => w.Trim().ToUpper())
            .Distinct()
            .ToList();

        return normalizedWords
            .GroupBy(w => w[0])
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}