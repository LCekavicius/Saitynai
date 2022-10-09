namespace WebApi.Data.Entities
{
    public class Work
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public bool IsPaused { get; set; }
        public ProductionOrder ProductionOrder { get; set; }
    }
}
