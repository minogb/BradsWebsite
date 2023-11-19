using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BradsWebsite.Authentication;
using BradsWebsite.Models;

namespace BradsWebsite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        UserManager _userManager;
        
        public AccountController(UserManager userManager)
        {
            _userManager = userManager;
        }
        [AllowAnonymous]
        public IActionResult LogIn(string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);
                return RedirectToAction("Index", "Home", null);
            }
            ModelState.Remove(nameof(ReturnUrl));
            return View(new LoginModel());
        }
        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult LogIn(string ReturnUrl, LoginModel form)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);
                return RedirectToAction("Index", "Home", null);
            }
            ModelState.Remove(nameof(ReturnUrl));
            if (!ModelState.IsValid)
                return View(form);
            try
            {
                //authenticate
                var user = new AuthUser()
                {
                    Email = form.Email,
                    Password = form.Password
                };
                if(_userManager.SignIn(this.HttpContext, user))
                {
                    form.Password = "banana";
                    form.Password = null;
                    if(ReturnUrl != null)
                        return Redirect(ReturnUrl);
                    return RedirectToAction("Index", "Home", null);
                }
                else
                {
                    form.Password = "banana";
                    form.Password = null;
                    ModelState.AddModelError("summary", "Invalid Login Information");
                    return View(form);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("summary", ex.Message);
                return View(form);
            }
        }
        [AllowAnonymous]
        public IActionResult SignUp(string ReturnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);
                return RedirectToAction("Index", "Home", null);
            }
            ModelState.Remove(nameof(ReturnUrl));
            return View(new SignUpModel());
        }
        [AllowAnonymous]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SignUp(string ReturnUrl, SignUpModel form)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);
                return RedirectToAction("Index", "Home", null);
            }
            ModelState.Remove(nameof(ReturnUrl));
            if (!ModelState.IsValid)
                return View(form);
            var filter = new ProfanityFilter.ProfanityFilter();
            if(filter.ContainsProfanity(form.Name))
            {
                ModelState.AddModelError("summary", "User name cannot contain profanity.");
                return View(form);
            }
            if (filter.ContainsProfanity(form.Email))
            {
                ModelState.AddModelError("summary", "Email cannot contain profanity.");
                return View(form);
            }
            try
            {
                //authenticate
                var user = new AuthUser()
                {
                    Email = form.Email,
                    Password = form.Password,
                    Name = form.Name
                };
                if (_userManager.SignUp(this.HttpContext, user))
                {
                    form.Password = "banana";
                    form.Password = null;
                    if (ReturnUrl != null)
                        return Redirect(ReturnUrl);
                    return RedirectToAction("Index", "Home", null);
                }
                else
                {
                    form.Password = "banana";
                    form.Password = null;
                    ModelState.AddModelError("summary", "Invalid Login Information");
                    return View(form);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("summary", ex.Message);
                return View(form);
            }
        }

        public IActionResult LogOut()
        {
            _userManager.SignOut(HttpContext);
            return RedirectToAction("Index", "Home", null);
        }
        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }
        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
