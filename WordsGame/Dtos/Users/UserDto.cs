using System.ComponentModel.DataAnnotations;

namespace WordsGame.Dtos.Users;

public class UserDto : LoginUserDto
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Role { get; set; }
}