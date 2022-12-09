using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using WebApi.Auth.Model;
using WebApi.Data.Dtos;
using WebUi.Auth;
using WebUi.Data.Commands.Auth.Register;
using WebUi.Data.Helpers.Wrappers;
using WebUi.Helpers.Http;
using WebUi.Pages;

namespace WebUi.Data.Services.UserService
{
    public class CompanyService : ICompanyService
    {
        private readonly HttpClient _httpClient;
        //private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IConfiguration _configuration;
        string baseUrl;

        public CompanyService(IConfiguration configuration, HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            //_authenticationStateProvider = authenticationStateProvider;
            baseUrl = _configuration.GetSection("WebApiBaseUrl").Value;
        }

        public async Task<Result> DeleteCompany(int companyId)
        {
            var result = await this._httpClient.DeleteAsync($"{baseUrl}api/companies/{companyId}");
            return result.IsSuccessStatusCode ? new Result(true) : new Result(false);
        }

        public async Task<Result<List<CompanyDto>>> GetCompanies()
        {
            var resultJson = await this._httpClient.GetStringAsync($"{baseUrl}api/companies");
            var result = JsonConvert.DeserializeObject<List<CompanyDto>>(resultJson);
            return new Result<List<CompanyDto>>(result, true);
        }

        public async Task<Result<CompanyDto>> GetCompany(int companyId)
        {
            var resultJson = await this._httpClient.GetStringAsync($"{baseUrl}api/companies/{companyId}");
            var result = JsonConvert.DeserializeObject<CompanyDto>(resultJson);
            return new Result<CompanyDto>(result, true);
        }

        public async Task<Result<CompanyDto>> InsertCompany(CreateCompanyDto dto)
        {
            var response = await _httpClient.PostAsync($"{baseUrl}api/companies", RequestHelper.GetStringContentFromObject(dto));
            if (!response.IsSuccessStatusCode)
                return new Result<CompanyDto>(null, false, "yea");
            var company = JsonConvert.DeserializeObject<CompanyDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return new Result<CompanyDto>(company, true);
        }

        public async Task<Result<CompanyDto>> UpdateCompany(int companyId, UpdateCompanyDto dto)
        {
            var response = await _httpClient.PutAsync($"{baseUrl}api/companies/{companyId}", RequestHelper.GetStringContentFromObject(dto));
            if (!response.IsSuccessStatusCode)
                return new Result<CompanyDto>(null, false, response.ReasonPhrase);
            var company = JsonConvert.DeserializeObject<CompanyDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return new Result<CompanyDto>(company, true);
        }
    }
}
