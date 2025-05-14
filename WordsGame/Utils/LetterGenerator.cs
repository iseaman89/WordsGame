using WordsGame.Enums;

namespace WordsGame.Utils;

public static class LetterGenerator
{
    private static readonly Dictionary<Language, char[]> Alphabets = new()
    {
        { Language.Ukrainian, "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ".ToCharArray() },
        { Language.English, "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray() },
        { Language.German, "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜß".ToCharArray() }
    };

    private static readonly Random Random = new();

    public static List<char> GetRandomLetters(int count, Language language, LetterMode mode)
    {
        var alphabet = Alphabets[language];

        if (mode == LetterMode.NoRepeats)
        {
            if (count > alphabet.Length)
                throw new ArgumentException("Requested more letters than available in the alphabet.");

            return alphabet
                .OrderBy(_ => Random.Next())
                .Take(count)
                .ToList();
        }
        else 
        {
            return Enumerable.Range(0, count)
                .Select(_ => alphabet[Random.Next(alphabet.Length)])
                .ToList();
        }
    }
}