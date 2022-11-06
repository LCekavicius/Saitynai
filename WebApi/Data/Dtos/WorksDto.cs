using WebApi.Data.Entities;

namespace WebApi.Data.Dtos
{
    public record WorksDto(int Id, string type, string description, DateTime Creationdate, DateTime? modifiedDate
        , DateTime? startDateTime, DateTime? endDateTime, bool isPaused, int productionOrderId);
    public record CreateWorksDto(string userId, string type, string description);
    public record UpdateWorksDto(string? type, string? description, DateTime? startDateTime, DateTime? endDatetime, bool? isPaused);
}
