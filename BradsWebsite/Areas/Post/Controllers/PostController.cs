using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BradsWebsite.Areas.Post.Models;

namespace BradsWebsite.Areas.Post.Controllers
{
    [Authorize]
    [Area("Post")]
    public class PostController : Controller
    {
        PostManager Manager;
        public PostController(PostManager manager)
        {
            Manager = manager;
        }
        // GET: PostController
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(PostPageModel PostPage)
        {
            ModelState.Remove(nameof(PostPage));
            ModelState.Remove("Tag");
            ModelState.Remove("Mention");
            ModelState.Remove("Post");
            ModelState.Remove("ParentID");
            ModelState.Remove("Parent");
            var posts = Manager.GetPosts(HttpContext, PostPage);
            ViewBag.Posts = posts;
            return View(PostPage);
        }
        [HttpGet]
        public ActionResult _PostPage(PostPageModel PostPage)
        {
            if (ViewBag.Posts == null)
            {
                var posts = Manager.GetPosts(HttpContext, PostPage);
                ViewBag.Posts = posts;
            }
            return PartialView(PostPage);
        }

        public ActionResult Create()
        {
            return View(new CreatePostModel());
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreatePostModel Post, string ReturnUrl)
        {
            ModelState.Remove(nameof(ReturnUrl));
            ViewBag.ReturnUrl = ReturnUrl;
            if (!ModelState.IsValid)
            {
                return View(Post);
            }
            try
            {
                var filter = new ProfanityFilter.ProfanityFilter();
                foreach(var link in Post.Links)
                {
                    if (filter.ContainsProfanity(link.Url))
                    {
                        ModelState.AddModelError("summary", "Profanity is not allowed in links");
                        return View(Post);
                    }
                }
                if (!Manager.CreatePost(HttpContext, Post))
                {
                    ModelState.AddModelError("summary", "Invalid Post Information");
                    return View(Post);
                }
                else
                {
                    if (ReturnUrl != null)
                        return Redirect(ReturnUrl);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("summary", e.Message);
                return View(Post);
            }
        }
        [HttpPost]
        public ActionResult Delete(int id, [FromRoute] string ReturnUrl)
        {
            if (Manager.DeletePost(HttpContext, id))
            {
                ModelState.AddModelError("summary", "Failed to delete post");
                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);
                return RedirectToAction("Index");
            }
            ModelState.Remove(nameof(ReturnUrl));
            ModelState.AddModelError("summary", "Post Deleted");
            if (ReturnUrl != null)
                return Redirect(ReturnUrl);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Reply(int id)
        {
            var postPage = new PostPageModel();
            postPage.ParentID = id;
            return RedirectToAction("Index", postPage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply([FromRoute] int id, [FromRoute] string ReturnUrl, CreatePostModel Post)
        {
            ModelState.Remove(nameof(ReturnUrl));
            ModelState.AddModelError("summary", "Post Deleted");
            if (ReturnUrl != null)
                return Redirect(ReturnUrl);
            return RedirectToAction("Index");
        }
    }
}
