using BradsWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BradsWebsite.Controllers
{
    public class DailyController : Controller
    {
        IConfiguration Configuration;

        public DailyController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(new DailyMessageCategoryModel(Configuration));
        }
        public IActionResult View(int id)
        {
            if(id == 0)
                return RedirectToAction("Index");
            return View(new DailyMessageModel(id, Configuration));
        }
        public PartialViewResult _Details(DailyMessageModel model)
        {
            return PartialView(model);
        }
        public IActionResult Category(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");
            return View(new DailyMessageCategoryModel(id,Configuration));
        }
        [Authorize(Roles = "Daily")]
        [HttpPost]
        public IActionResult Delete(string id)
        {
            //TODO delte
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Daily")]
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Daily")]
        public IActionResult New(DailyMessageModel dailyMessage)
        {
            if(dailyMessage == null)
                return RedirectToAction("Index");
            if (!ModelState.IsValid)
                return View(dailyMessage);
            if (dailyMessage.Category == null)
                dailyMessage.Category = "Safety";
            if (dailyMessage.SaveNew(Configuration))
                return RedirectToAction("View", dailyMessage.Id);
            ModelState.AddModelError("summary", "Could not save new daily message.");
            return View(dailyMessage);
        }
    }
}
