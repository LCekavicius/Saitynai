using WebApi.Auth.Model;
using WebApi.Data.Dtos;
using WebUi.Data.Helpers.Wrappers;

namespace WebUi.Data.Services.UserService
{
    public interface IWorkService
    {
        Task<Result<List<WorksDto>>> GetWorks(int companyId, int orderId);
        Task<Result<WorksDto>> GetWork(int companyId, int orderId, int WorkId);
        Task<Result<WorksDto>> InsertWork(int companyId, int orderId, CreateWorksDto dto);
        Task<Result<WorksDto>> UpdateWork(int companyId, int orderId, int WorkId, UpdateWorksDto dto);
        Task<Result> DeleteWork(int companyId, int orderId, int WorkId);

    }
}