namespace WordsGame.Dtos;

public class WordCheckResponse
{
    public List<WordCheckResult> Results { get; set; } = [];
    public string? LongestWord { get; set; }
}