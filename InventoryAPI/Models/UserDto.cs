using System.ComponentModel.DataAnnotations;

public class UserDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
}