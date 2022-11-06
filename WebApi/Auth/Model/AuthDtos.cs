using System.ComponentModel.DataAnnotations;

namespace WebApi.Auth.Model
{
    public record RegisterUserDto([Required]string UserName, [EmailAddress][Required] string Email, [Required] string Password, [Required] int companyId);
    public record LoginDto([Required]string UserName, [Required] string Password);
    public record UserDto(string Id, string UserName, string Email, int companyId);
    public record SucessfulLoginDto(string AccessToken);
}
