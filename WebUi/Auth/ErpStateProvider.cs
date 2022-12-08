using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace WebUi.Auth
{
	public static class StorageConstants
	{
		public static class Local
		{
			public static string AuthToken = "authToken";
			public static string RefreshToken = "refreshToken";
		}
	}
	public class ErpStateProvider : AuthenticationStateProvider
	{
		private readonly HttpClient _httpClient;
		private readonly ProtectedLocalStorage _localStorage;

		public ErpStateProvider(HttpClient httpClient, ProtectedLocalStorage localStorage)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
		}

		public async Task StateChangedAsync()
		{
			var authState = Task.FromResult(await GetAuthenticationStateAsync());

			NotifyAuthenticationStateChanged(authState);
		}

		// -----------------------------

		public async Task StateChangedAsyncAfterMicrosoftLogin(string token)
		{
			var authState = Task.FromResult(await GetAuthenticationStateAsyncAfterMicrosoftLogin(token));

			NotifyAuthenticationStateChanged(authState);
		}

		public async Task LogOut()
		{
			await _localStorage.DeleteAsync(StorageConstants.Local.AuthToken);
            await StateChangedAsync();
        }

        public async Task SetAuthToken(string token)
        {
			await _localStorage.SetAsync(StorageConstants.Local.AuthToken, token);
			await StateChangedAsync();
        }
		public async Task<AuthenticationState> GetAuthenticationStateAsyncAfterMicrosoftLogin(string token)
		{
			await _localStorage.SetAsync(StorageConstants.Local.AuthToken, token);
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(token), "jwt")));
			AuthenticationStateUser = state.User;

			return state;
		}

		// -----------------------------

		public void MarkUserAsLoggedOut()
		{
			var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
			var authState = Task.FromResult(new AuthenticationState(anonymousUser));

			NotifyAuthenticationStateChanged(authState);
		}

		public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
		{
			var state = await this.GetAuthenticationStateAsync();
			var authenticationStateProviderUser = state.User;

			return authenticationStateProviderUser;
		}

		public ClaimsPrincipal AuthenticationStateUser { get; set; }

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var token = await _localStorage.GetAsync<string>(StorageConstants.Local.AuthToken);
			if (string.IsNullOrWhiteSpace(token.Value))
			{
				return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
			}

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
			var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(token.Value), "jwt")));
			AuthenticationStateUser = state.User;

			return state;
		}

		private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
		{
			var claims = new List<Claim>();
			var payload = jwt.Split('.')[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);
            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
		}

		private byte[] ParseBase64WithoutPadding(string payload)
		{
			payload = payload.Trim().Replace('-', '+').Replace('_', '/');
			var base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
			return Convert.FromBase64String(base64);
		}
	}
}
