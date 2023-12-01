using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BradsWebsite.Areas.Pyramid.Controllers
{
    [Authorize]
    [Area("Pyramid")]
    public class PyramidController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Play()
        {
            return View();
        }
        public IActionResult Move(int id)//TODO this needs to be the direction taken
        {
            return RedirectToAction("Play");
        }
        public IActionResult Attack(int id)//TODO this needs to be the attack ie attack
        {
            return RedirectToAction("Play");
        }
        public IActionResult Use(int id)//TODO this needs to be item used and on what
        {
            return RedirectToAction("Play");
        }
        public IActionResult Take(int id)//TODO this needs to be what to take
        {
            return RedirectToAction("Play");
        }
    }
}
