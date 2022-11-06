using Microsoft.AspNetCore.Identity;
using WebApi.Auth.Model;

namespace WebApi.Data
{
    public class AuthDbSeeder
    {
        private UserManager<ERPUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AuthDbSeeder(UserManager<ERPUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRoles();
            await AddAdminUser();
        }

        private async Task AddAdminUser()
        {
            var newAdminUser = new ERPUser()
            {
                UserName = "Admin",
                Email = "admin@admin.com"
            };

            var existingAdmin = await _userManager.FindByNameAsync(newAdminUser.UserName);
            if (existingAdmin == null)
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "Admin1!");
                if (createAdminUserResult.Succeeded)
                {
                    await _userManager.AddToRolesAsync(newAdminUser, ERPRoles.All);
                }
            }

        }

        private async Task AddDefaultRoles()
        {
            foreach (var item in ERPRoles.All)
            {
                var roleExists = await _roleManager.RoleExistsAsync(item);
                if (!roleExists)
                    await _roleManager.CreateAsync(new IdentityRole(item));
            }
        }


    }
}
