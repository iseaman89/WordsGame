using WordsGame.Dtos;
using WordsGame.Models;

namespace WordsGame.Services;

public class PointService : IPointService
{
    public GameResultsDto CheckResults(List<Player> players)
    {
        var allWords = players
            .SelectMany(p => p.CheckedWords.WordResults
                .Where(w => w.IsValid)
                .Select(w => new { Player = p, Word = w.Word.ToLower(), WordResult = w }))
            .ToList();

        var wordGroups = allWords
            .GroupBy(x => x.Word)
            .ToDictionary(g => g.Key, g => g.ToList());

        // 1. Підрахунок основних балів
        foreach (var player in players)
        {
            foreach (var wordResult in player.CheckedWords.WordResults)
            {
                wordResult.Points = 0;

                if (!wordResult.IsValid)
                    continue;

                var word = wordResult.Word.ToLower();
                var sameWordEntries = wordGroups[word];

                if (sameWordEntries.Count == 1)
                {
                    wordResult.Points = 5;
                    wordResult.IsSame = false;
                }
                else
                {
                    wordResult.Points = 2;
                    wordResult.IsSame = true;
                }
            }
        }

        // 2. Бонус 10 балів за найдовше унікальне слово > 5 літер
        var uniqueLongWords = allWords
            .Where(w => wordGroups[w.Word].Count == 1 && w.Word.Length > 5)
            .ToList();

        if (uniqueLongWords.Any())
        {
            var longestWord = uniqueLongWords
                .OrderByDescending(w => w.Word.Length)
                .First();

            longestWord.WordResult.Points += 10;
        }

        // 3. Сума балів гравця
        foreach (var player in players)
        {
            player.GamePoints = player.CheckedWords.WordResults.Sum(w => w.Points);
        }
        
        var maxPoint = players.Max(p => p.GamePoints);
        var winnerId = players.First(p => p.GamePoints == maxPoint).UserId;

        return new GameResultsDto
        {
            Players = players,
            WinnerId = winnerId
        };
    }
}