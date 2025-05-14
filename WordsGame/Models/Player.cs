using WordsGame.Dtos.WordCheck;

namespace WordsGame.Models;

public class Player
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public CheckedWordsDto CheckedWords { get; set; }
    public int GamePoints { get; set; }
}