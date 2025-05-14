using WordsGame.Models;

namespace WordsGame.Services;

public class LobbyService : ILobbyService
{
    private readonly List<Lobby> _lobbies = new();
    
    public Lobby JoinOrCreateLobby(Player player)
    {
        var openLobby = _lobbies.FirstOrDefault(l => !l.GameStarted && l.Players.Count < 4);

        if (openLobby is null)
        {
            var newLobby = new Lobby();
        }

        return openLobby;
    }
}