using Microsoft.AspNetCore.Mvc;
using CyberSecLabPlatform.Models;

namespace CyberSecLabPlatform.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
