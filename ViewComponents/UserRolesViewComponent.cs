using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CyberSecLabPlatform.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CyberSecLabPlatform.ViewComponents
{
    public class UserRolesViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRolesViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View(new List<string>());
            }

            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            if (user == null)
            {
                return View(new List<string>());
            }

            var roles = await _userManager.GetRolesAsync(user);
            return View(roles);
        }
    }
}
