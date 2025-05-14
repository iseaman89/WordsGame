using WordsGame.Enums;

namespace WordsGame.Dtos;

public class WordCheckRequest
{
    public List<string> Words { get; set; } = [];
    public Language Language { get; set; }
}