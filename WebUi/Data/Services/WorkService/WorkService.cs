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
    public class WorkService : IWorkService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        string baseUrl;

        public WorkService(IConfiguration configuration, HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            baseUrl = _configuration.GetSection("WebApiBaseUrl").Value;
        }

        public async Task<Result<List<WorksDto>>> GetWorks(int companyId, int orderId)
        {
            var resultJson = await this._httpClient.GetStringAsync($"{baseUrl}api/companies/{companyId}/productionorders/{orderId}/works");
            var result = JsonConvert.DeserializeObject<List<WorksDto>>(resultJson);
            return new Result<List<WorksDto>>(result, true);
        }

        public async Task<Result<WorksDto>> GetWork(int companyId, int orderId, int WorkId)
        {
            var resultJson = await this._httpClient.GetStringAsync($"{baseUrl}api/companies/{companyId}/productionorders/{orderId}/works/{WorkId}");
            var result = JsonConvert.DeserializeObject<WorksDto>(resultJson);
            return new Result<WorksDto>(result, true);
        }

        public async Task<Result<WorksDto>> InsertWork(int companyId, int orderId, CreateWorksDto dto)
        {
            var response = await _httpClient.PostAsync($"{baseUrl}api/companies/{companyId}/productionorders/{orderId}/works", RequestHelper.GetStringContentFromObject(dto));
            if (!response.IsSuccessStatusCode)
                return new Result<WorksDto>(null, false, "yea");
            var work = JsonConvert.DeserializeObject<WorksDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return new Result<WorksDto>(work, true);
        }

        public async Task<Result<WorksDto>> UpdateWork(int companyId, int orderId, int WorkId, UpdateWorksDto dto)
        {
            var response = await _httpClient.PutAsync($"{baseUrl}api/companies/{companyId}/productionorders/{orderId}/works/{WorkId}", RequestHelper.GetStringContentFromObject(dto));
            if (!response.IsSuccessStatusCode)
                return new Result<WorksDto>(null, false, response.ReasonPhrase);
            var work = JsonConvert.DeserializeObject<WorksDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return new Result<WorksDto>(work, true);
        }

        public async Task<Result> DeleteWork(int companyId, int orderId, int WorkId)
        {
            var result = await this._httpClient.DeleteAsync($"{baseUrl}api/companies/{companyId}/productionorders/{orderId}/works/{WorkId}");
            return result.IsSuccessStatusCode ? new Result(true) : new Result(false);
        }
    }
}
