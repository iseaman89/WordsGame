namespace WordsGame.Dtos.WordCheck;

public class CheckedWordsDto
{
    public List<WordResultOnlineGame> WordResults { get; set; } = new();
    public string UserId { get; set; }
}