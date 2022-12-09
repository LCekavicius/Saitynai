using WebApi.Auth.Model;
using WebApi.Data.Dtos;
using WebUi.Data.Helpers.Wrappers;

namespace WebUi.Data.Services.UserService
{
    public interface IOrderService
    {
        Task<Result<List<ProductionOrderDto>>> GetOrders(int companyId);
        Task<Result<ProductionOrderDto>> GetOrder(int companyId, int orderId);
        Task<Result<ProductionOrderDto>> InsertOrder(int companyId, CreateProductionOrderDto dto);
        Task<Result<ProductionOrderDto>> UpdateOrder(int companyId, int orderId, UpdateProductionOrderDto dto);
        Task<Result> DeleteOrder(int companyId, int orderId);

    }
}