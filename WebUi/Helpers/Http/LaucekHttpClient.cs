using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebUi.Helpers.Http
{
    public class LaucekHttpClient : ILaucekHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly NavigationManager _navigationManager;

        public LaucekHttpClient(
            HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            IConfiguration configuration,
            IAuthenticationService authenticationService,
            NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _configuration = configuration;
            _authenticationService = authenticationService;
            _navigationManager = navigationManager;
        }

        public async Task<string> GetStringAsync(string requestUri)
        {
            if (await ValidateAccessToken().ConfigureAwait(false))
            {
                return await _httpClient.GetStringAsync($"{_httpClient.BaseAddress}{requestUri}").ConfigureAwait(false);
            }

            throw new UnauthorizedAccessException($"Your access token has expired");
        }

        public async Task<Stream> GetStreamAsync(string requestUri)
        {
            if (await ValidateAccessToken().ConfigureAwait(false))
            {
                return await _httpClient.GetStreamAsync($"{_httpClient.BaseAddress}{requestUri}").ConfigureAwait(false);
            }
            throw new UnauthorizedAccessException($"Your access token has expired");
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            if (await ValidateAccessToken().ConfigureAwait(false))
            {
                return await _httpClient.PostAsync($"{_httpClient.BaseAddress}{requestUri}", content).ConfigureAwait(false);
            }

            throw new UnauthorizedAccessException($"Your access token has expired");
        }

        public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            if (await ValidateAccessToken().ConfigureAwait(false))
            {
                return await _httpClient.PutAsync($"{_httpClient.BaseAddress}{requestUri}", content).ConfigureAwait(false);
            }

            throw new UnauthorizedAccessException($"Your access token has expired");
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            if (await ValidateAccessToken().ConfigureAwait(false))
            {
                return await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}{requestUri}").ConfigureAwait(false);
            }

            throw new UnauthorizedAccessException($"Your access token has expired");
        }

        private async Task<bool> ValidateAccessToken()
        {
            try
            {
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                //var exp = user.FindFirstValue(ApplicationClaimTypes.Expiration);
                //var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
                //var timeUTC = DateTime.UtcNow;
                //var diff = expTime - timeUTC;
                //var refreshTimeInHours = double.Parse(_configuration["JwtRefreshTimeWindowInHours"]);
                //var refreshTime = TimeSpan.FromHours(refreshTimeInHours);
                //if (diff.TotalMinutes >= 1 && diff.TotalMinutes <= refreshTime.TotalMinutes && user.Identity.IsAuthenticated)
                //{
                //    await _authenticationService.RefreshToken();
                //}
                //else if (diff.TotalMinutes < 1 && user.Identity.IsAuthenticated)
                //{
                //    await Logout();
                //    return false;
                //}

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //protected async Task Logout()
        //{
        //    await _authenticationService.Logout().ConfigureAwait(false);
        //    _navigationManager.NavigateTo("/", true);
        //}
    }
}
