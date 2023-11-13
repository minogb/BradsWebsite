using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BradsWebsite.Models;
using System.Diagnostics;

namespace BradsWebsite.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration Configuration;

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            
            return View(new HomeDataModel(Configuration));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}