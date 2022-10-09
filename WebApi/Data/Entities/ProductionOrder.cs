namespace WebApi.Data.Entities
{
    public class ProductionOrder
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public Company Company { get; set; }
    }
}
