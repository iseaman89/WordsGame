using WordsGame.Models;

namespace WordsGame.Services;

public interface ILobbyService
{
    Lobby JoinOrCreateLobby(Player player);
}