
using System.ComponentModel.DataAnnotations;

namespace api.TransferObjects;

public class RegisterOrLoginRequestDto
{
    public RegisterOrLoginRequestDto(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required]
    [EmailAddress(ErrorMessage = "Email is not a valid email")]
    public string Email { get; set; }

    [Required] [MinLength(8)] public string Password { get; set; }
}