using WordsGame.Enums;

namespace WordsGame.Services;

public interface IWordService
{
    Task<List<string>?> GetWordsByLanguageAndLetterAsync(Language language, string firstLetter);
}