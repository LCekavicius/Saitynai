using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Auth;
using WebApi.Auth.Model;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ERPUser> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        public AuthController(UserManager<ERPUser> userManager, IJwtTokenService jwtTokenService)
        {
            this._userManager = userManager;
            this._jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        [Route("registerworker")]
        public async Task<IActionResult> RegisterWorker(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null)
                return BadRequest("Username already taken");

            var newUser = new ERPUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName,
                CompanyId = registerUserDto.companyId
            };

            var createdUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!createdUserResult.Succeeded)
                return BadRequest("Could not create user");

            await _userManager.AddToRoleAsync(newUser, ERPRoles.Worker);

            return CreatedAtAction(nameof(RegisterWorker), new UserDto(newUser.Id, newUser.UserName, newUser.Email, newUser.CompanyId));
        }

        [HttpPost]
        [Route("registerrepresentative")]
        public async Task<IActionResult> RegisterRepresentative(RegisterUserDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null)
                return BadRequest("Username already taken");

            var newUser = new ERPUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName,
                CompanyId = registerUserDto.companyId
            };

            var createdUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!createdUserResult.Succeeded)
                return BadRequest("Could not create user");

            await _userManager.AddToRoleAsync(newUser, ERPRoles.Representative);

            return CreatedAtAction(nameof(RegisterRepresentative), new UserDto(newUser.Id, newUser.UserName, newUser.Email, newUser.CompanyId));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
                return BadRequest("User name or password is invalid.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordValid)
                return BadRequest("User name or password is invalid.");

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtTokenService.CreateAccessToken(user.Id, roles, user.CompanyId);

            return Ok(new SucessfulLoginDto(accessToken));
        }

    }
}
