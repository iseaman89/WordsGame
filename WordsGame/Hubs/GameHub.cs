using Microsoft.AspNetCore.SignalR;
using WordsGame.Dtos;
using WordsGame.Models;
using WordsGame.Services;

namespace WordsGame.Hubs;

public class GameHub : Hub
{
    private readonly IWordCheckerService _wordCheckerService;
    private readonly IPointService _pointService;
    private static Dictionary<string, Lobby> _matches = new();
    private static object _lock = new();

    public GameHub(IWordCheckerService wordCheckerService, IPointService pointService)
    {
        _wordCheckerService = wordCheckerService;
        _pointService = pointService;
    }
    
    public async Task JoinOrCreateMatch(Player player)
    {
        string matchId = null;

        lock (_lock)
        {
            // шукаємо вільний матч
            var openMatch = _matches.FirstOrDefault(x => !x.Value.GameStarted && x.Value.Players.Count < 4);
            
            if (openMatch.Key != null)
            {
                matchId = openMatch.Key;
            }
            else
            {
                // створюємо новий матч
                matchId = Guid.NewGuid().ToString();
                _matches[matchId] = new Lobby(){GameStarted = false, LobbyId = matchId};
            }

            _matches[matchId].Players.Add(player);
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, matchId);
        await Clients.Group(matchId).SendAsync("PlayerJoined", player);
        await Clients.Caller.SendAsync("LobbyJoined", matchId);

        if (_matches[matchId].Players.Count == 4)
        {
            _matches[matchId].GameStarted = true;
            await Clients.Group(matchId).SendAsync("MatchStarted", matchId);

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(63));

                lock (_lock)
                {
                    Clients.Group(matchId).SendAsync("MatchEnded", matchId);
                }
            });
        }
    }
    
    public async Task SubmitWords(string lobbyId, string userId, WordCheckRequest words)
    {
        if (_matches.TryGetValue(lobbyId, out var lobby))
        {
            var player = lobby.Players.FirstOrDefault(p => p.UserId == userId);
            if (player != null)
            {
                player.CheckedWords = await _wordCheckerService.CheckWordsOnlineGameAsync(words, player.UserId);
            }

            if (lobby.Players.All(p => p.CheckedWords != null && p.CheckedWords.WordResults.Count > 0))
            {
                var gameResults = _pointService.CheckResults(lobby.Players);

                await Clients.Group(lobbyId).SendAsync("ResultReady", gameResults);

                _matches.Remove(lobbyId); // очищаємо
            }
        }
    }
}