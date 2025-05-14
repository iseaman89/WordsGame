using WordsGame.Enums;

namespace WordsGame.Models;

public class Word
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public string FirstLetter { get; set; } = string.Empty;
    public Language Language { get; set; }
}