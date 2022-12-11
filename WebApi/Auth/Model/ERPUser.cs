using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using WebApi.Data.Dtos;

namespace WebApi.Auth.Model
{
    public class ERPUser : IdentityUser
    {
        [PersonalData]
        public string? AdditionalInfo { get; set; }
        public int CompanyId { get; set; }

        public override bool Equals(object o)
        {
            var other = o as ERPUser;
            return other?.Id == Id;
        }
        public override int GetHashCode() => Id?.GetHashCode() ?? 0;
        public override string ToString() => UserName;
    }
}
