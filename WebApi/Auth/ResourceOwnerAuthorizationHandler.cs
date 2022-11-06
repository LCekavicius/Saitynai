using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApi.Auth.Model;

namespace WebApi.Auth
{
    public class ResourceOwnerAuthorizationHandler : AuthorizationHandler<ResourceOwnerRequirement, IUserOwnedResource>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOwnerRequirement requirement,
            IUserOwnedResource resource)
        {
            if(context.User.IsInRole(ERPRoles.Admin) ||
                context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) == resource.UserId ||
                (context.User.IsInRole(ERPRoles.Representative) &&
                    context.User.FindFirstValue("companyId") == resource.ProductionOrder.Company.Id.ToString())
                )
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class CompanyEmployeeAuthorizationHandler : AuthorizationHandler<ComplayeEmployeeRequirement, int>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ComplayeEmployeeRequirement requirement,
            int resource)
        {
            if (context.User.IsInRole(ERPRoles.Admin) || 
                context.User.FindFirstValue("companyId") == resource.ToString())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public record ResourceOwnerRequirement : IAuthorizationRequirement;
    public record ComplayeEmployeeRequirement : IAuthorizationRequirement;
}
