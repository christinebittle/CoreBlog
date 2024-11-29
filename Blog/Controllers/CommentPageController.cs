using Microsoft.AspNetCore.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    public class CommentPageController : Controller
    {

        // GET : /CommentPage/List -> A webpage which shows the comments
        public IActionResult List()
        {
            // disclaimer: the toolkit should be an individual component that ONLY interacts with the database
            CommentAPIController toolkit = new CommentAPIController();
            List<Comment> Comments = toolkit.ListComments();
            
            // Directs to /Views/CommentPage/List.cshtml
            return View(Comments);
        }
    }
}
