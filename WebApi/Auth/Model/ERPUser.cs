using Microsoft.AspNetCore.Identity;

namespace WebApi.Auth.Model
{
    public class ERPUser : IdentityUser
    {
        [PersonalData]
        public string? AdditionalInfo { get; set; }
        public int CompanyId { get; set; }
    }
}
