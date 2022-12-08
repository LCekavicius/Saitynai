using WebApi.Auth.Model;
using WebUi.Data.Commands.Auth.Register;
using WebUi.Data.Helpers.Wrappers;

namespace WebUi.Data.Services.UserService
{
    public interface IAuthService
    {
        Task<Result<SucessfulLoginDto>> Login(LoginDto loginModel);
        Task<Result<bool>> Register(RegisterUserDto registerDto, bool worker);
    }
}