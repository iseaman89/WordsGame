namespace WordsGame.Dtos;

public class WordCheckResult
{
    public string Word { get; set; } = string.Empty;
    public bool IsValid { get; set; }
    public int Points { get; set; }
}