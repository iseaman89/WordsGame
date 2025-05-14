using Microsoft.AspNetCore.Mvc;
using WordsGame.Data;
using WordsGame.Dtos;
using WordsGame.Enums;
using WordsGame.Services;
using WordsGame.Utils;

namespace WordsGame.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<WordsController> _logger;
    private readonly IWordCheckerService _wordCheckerService;
    private readonly WordImporter _wordImporter;
    
    public WordsController(AppDbContext context, ILogger<WordsController> logger, IWordCheckerService wordCheckerService, WordImporter wordImporter)
    {
        _context = context;
        _logger = logger;
        _wordCheckerService = wordCheckerService;
        _wordImporter = wordImporter;
    }
    
    [HttpGet("random-letters")]
    public async Task<ActionResult<IEnumerable<char>>> GetRandomLetters([FromQuery]Language language, [FromQuery]LetterMode letterMode, [FromQuery]int count, CancellationToken cancellationToken)
    {
        try
        {
            var letters =  LetterGenerator.GetRandomLetters(count, language, letterMode);
            return Ok(letters);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error Performing GET in {nameof(AddWords)}");
            return StatusCode(500, "Error");
        }
    }
    
    [HttpPost("check")]
    public async Task<IActionResult> CheckWords([FromBody]WordCheckRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (request?.Words == null || !request.Words.Any())
                return BadRequest("List of words is required.");

            var result = await _wordCheckerService.CheckWordsAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error Performing GET in {nameof(AddWords)}");
            return StatusCode(500, "Error");
        }
    }
    
    [HttpGet()]
    public async Task<ActionResult> AddWords(string path, Language language, CancellationToken cancellationToken)
    {
        try
        {
            await _wordImporter.ImportWordsFromFile(path, language);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error Performing GET in {nameof(AddWords)}");
            return StatusCode(500, "Error");
        }
    }
}