using Blazored.LocalStorage;
using Hubbetech.Client.Providers;
using Hubbetech.Shared.Auth;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace Hubbetech.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResult> Login(LoginRequest loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
            var content = await response.Content.ReadAsStringAsync();

            try
            {
                var result = System.Text.Json.JsonSerializer.Deserialize<LoginResult>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                
                if (result == null)
                {
                    return new LoginResult { Successful = false, Error = "Server returned empty response." };
                }

                if (result.Successful)
                {
                    await _localStorage.SetItemAsync("authToken", result.Token);
                    ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginRequest.Email);
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", result.Token);
                }
                return result;
            }
            catch (System.Text.Json.JsonException)
            {
                // Content is likely HTML (error page) or empty
                // Truncate content if it's too long (e.g. full HTML page)
                var errorMsg = content.Length > 200 ? content.Substring(0, 200) + "..." : content;
                return new LoginResult { Successful = false, Error = $"Server Error ({(int)response.StatusCode}) at {response.RequestMessage?.RequestUri}: {errorMsg}" };
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<LoginResult> Register(LoginRequest registerRequest)
        {
             var result = await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);
             return await result.Content.ReadFromJsonAsync<LoginResult>();
        }
    }
}
