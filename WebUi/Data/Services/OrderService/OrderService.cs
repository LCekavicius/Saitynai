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
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        //private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IConfiguration _configuration;
        string baseUrl;

        public OrderService(IConfiguration configuration, HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            //_authenticationStateProvider = authenticationStateProvider;
            baseUrl = _configuration.GetSection("WebApiBaseUrl").Value;
        }

        public async Task<Result> DeleteOrder(int companyId, int orderId)
        {
            var result = await this._httpClient.DeleteAsync($"{baseUrl}api/companies/{companyId}/productionorders/{orderId}");
            return result.IsSuccessStatusCode ? new Result(true) : new Result(false);
        }

        public async Task<Result<List<ProductionOrderDto>>> GetOrders(int companyId)
        {
            var resultJson = await this._httpClient.GetStringAsync($"{baseUrl}api/companies/{companyId}/productionorders");
            var result = JsonConvert.DeserializeObject<List<ProductionOrderDto>>(resultJson);
            return new Result<List<ProductionOrderDto>>(result, true);
        }

        public async Task<Result<ProductionOrderDto>> GetOrder(int companyId, int orderId)
        {
            var resultJson = await this._httpClient.GetStringAsync($"{baseUrl}api/companies/{companyId}/productionorders/{orderId}");
            var result = JsonConvert.DeserializeObject<ProductionOrderDto>(resultJson);
            return new Result<ProductionOrderDto>(result, true);
        }

        public async Task<Result<ProductionOrderDto>> InsertOrder(int companyId, CreateProductionOrderDto dto)
        {
            var response = await _httpClient.PostAsync($"{baseUrl}api/companies/{companyId}/productionorders", RequestHelper.GetStringContentFromObject(dto));
            if (!response.IsSuccessStatusCode)
                return new Result<ProductionOrderDto>(null, false, "yea");
            var company = JsonConvert.DeserializeObject<ProductionOrderDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return new Result<ProductionOrderDto>(company, true);
        }

        public async Task<Result<ProductionOrderDto>> UpdateOrder(int companyId, int orderId, UpdateProductionOrderDto dto)
        {
            var response = await _httpClient.PutAsync($"{baseUrl}api/companies/{companyId}/productionorders/{orderId}", RequestHelper.GetStringContentFromObject(dto));
            if (!response.IsSuccessStatusCode)
                return new Result<ProductionOrderDto>(null, false, response.ReasonPhrase);
            var company = JsonConvert.DeserializeObject<ProductionOrderDto>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return new Result<ProductionOrderDto>(company, true);
        }
    }
}
