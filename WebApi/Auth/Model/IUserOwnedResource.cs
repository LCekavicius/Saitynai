using WebApi.Data.Entities;

namespace WebApi.Auth.Model
{
    public interface IUserOwnedResource
    {
        public string UserId { get; }
        public ProductionOrder ProductionOrder { get; }
    }
}
