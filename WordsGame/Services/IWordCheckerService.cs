using WordsGame.Dtos;
using WordsGame.Dtos.WordCheck;

namespace WordsGame.Services;

public interface IWordCheckerService
{
    Task<WordCheckResponse> CheckWordsAsync(WordCheckRequest words);
    Task<CheckedWordsDto> CheckWordsOnlineGameAsync(WordCheckRequest words, string userId);
}