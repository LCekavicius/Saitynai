using WebApi.Data.Entities;

namespace WebApi.Data.Dtos
{
    public record ProductionOrderDto(int Id, string productName, DateTime Creationdate, DateTime? modifiedDate
        , DateTime? startDateTime, DateTime? endDateTime, int companyId);
    public record CreateProductionOrderDto(string productName);
    public record UpdateProductionOrderDto(string? productName, DateTime? startDateTime, DateTime? endDatetime);
}
