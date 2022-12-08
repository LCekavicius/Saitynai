using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Auth.Model;
using WebUi.Auth;
using WebUi.Data.Commands.Auth.Register;
using WebUi.Data.Helpers.Wrappers;
using WebUi.Helpers.Http;

namespace WebUi.Data.Services.UserService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IConfiguration _configuration;
        string baseUrl;

        public AuthService(IConfiguration configuration, HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            baseUrl = _configuration.GetSection("WebApiBaseUrl").Value;
        }

        public async Task<Result<bool>> Register(RegisterUserDto registerDto, bool worker)
        {
            var response = await _httpClient.PostAsync($"{baseUrl}api/{getRegisterEndpoint(worker)}", RequestHelper.GetStringContentFromObject(registerDto)).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return new Result<bool>(false, false, response.ReasonPhrase);

            return new Result<bool>(true, true);
        }

        private string getRegisterEndpoint(bool worker)
        {
            return worker ? "registerworker" : "registerrepresentative";
        }

        public async Task<Result<SucessfulLoginDto>> Login(LoginDto loginModel)
        {
            var response = await _httpClient.PostAsync($"{baseUrl}api/Login", RequestHelper.GetStringContentFromObject(loginModel)).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                return new Result<SucessfulLoginDto>(null, false, "User not found");

            SucessfulLoginDto result = JsonConvert.DeserializeObject<SucessfulLoginDto>(response.Content.ReadAsStringAsync().Result);
            
            if (result == null)
                return new Result<SucessfulLoginDto>(null, false, "User not found") ;

            await ((ErpStateProvider)_authenticationStateProvider).SetAuthToken(result.AccessToken);
            
            return new Result<SucessfulLoginDto>(result, true);
        }


    }
}
