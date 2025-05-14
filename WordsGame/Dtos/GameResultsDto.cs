using WordsGame.Models;

namespace WordsGame.Dtos;

public class GameResultsDto
{
    public List<Player> Players { get; set; }
    public string WinnerId { get; set; }
}