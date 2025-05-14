using System.ComponentModel.DataAnnotations;

namespace WordsGame.Dtos.Users;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}