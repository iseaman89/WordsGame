using WordsGame.Dtos;
using WordsGame.Models;

namespace WordsGame.Services;

public interface IPointService
{
    GameResultsDto CheckResults(List<Player> players);
}