using System.ComponentModel.DataAnnotations;
using WebApi.Auth.Model;

namespace WebApi.Data.Entities
{
    public class Work : IUserOwnedResource
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
        [Required]
        public string UserId { get; set; }
        public ERPUser User { get; set; }
    }
}
