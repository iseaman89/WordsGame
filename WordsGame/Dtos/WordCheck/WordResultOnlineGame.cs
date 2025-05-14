namespace WordsGame.Dtos.WordCheck;

public class WordResultOnlineGame
{
    public string Word { get; set; }
    public bool IsValid { get; set; }
    public bool IsSame { get; set; }
    public int Points { get; set; }
}