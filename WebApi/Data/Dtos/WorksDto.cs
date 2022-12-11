using WebApi.Data.Entities;

namespace WebApi.Data.Dtos
{
    public class WorksDto : IComparable<WorksDto>
    {
        public int Id { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime Creationdate { get; set; }
        public DateTime? modifiedDate { get; set; }
        public DateTime? startDateTime { get; set; }
        public DateTime? endDateTime { get; set; }
        public bool isPaused { get; set; }
        public int productionOrderId { get; set; }
        public string userId { get; set; }

        public WorksDto(int id, string type, string description, DateTime creationdate,
            DateTime? modifiedDate, DateTime? startDateTime, DateTime? endDateTime,
            bool isPaused, int productionOrderId, string userId)
        {
            Id = id;
            this.type = type;
            this.description = description;
            Creationdate = creationdate;
            this.modifiedDate = modifiedDate;
            this.startDateTime = startDateTime;
            this.endDateTime = endDateTime;
            this.isPaused = isPaused;
            this.productionOrderId = productionOrderId;
            this.userId = userId;
        }



        public int CompareTo(WorksDto other)
        {
            if (GetWeight() > other.GetWeight())
                return 1;
            return GetWeight() == other.GetWeight() ? 0 : -1;
        }
        private int GetWeight()
        {
            if (isPaused)
                return 1;
            if (startDateTime.HasValue && endDateTime.HasValue)
                return 2;
            if (startDateTime.HasValue)
                return 0;

            return 1;
        }

        public override bool Equals(object? obj)
        {
            return obj is WorksDto dto &&
                   Id == dto.Id &&
                   productionOrderId == dto.productionOrderId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, productionOrderId);
        }
    }
    //public record WorksDto(int Id, string type, string description, DateTime Creationdate, DateTime? modifiedDate
    //    , DateTime? startDateTime, DateTime? endDateTime, bool isPaused, int productionOrderId, string userId);
    public record CreateWorksDto(string userId, string type, string description);
    public record UpdateWorksDto(string? type, string? description, DateTime? startDateTime, DateTime? endDatetime, bool? isPaused);
}
