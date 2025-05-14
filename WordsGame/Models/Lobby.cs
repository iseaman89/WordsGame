namespace WordsGame.Models;

public class Lobby
{
    public string LobbyId { get; set; }
    public List<Player> Players { get; set; } = new();
    public bool GameStarted { get; set; }
}