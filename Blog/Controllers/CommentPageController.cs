using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class CommentPageController : Controller
    {
        // GET : localhost:xx/CommentPage/List ->
        // A webpage output about comments
        public IActionResult List()
        {
            // using a toolkit that interacts with the database
            CommentAPIController toolkit = new CommentAPIController();

            List<Comment> Comments = toolkit.ListComments();

            return View(Comments);
        }

        public IActionResult ToDo()
        {
            ToDo MyToDo = new ToDo();

            MyToDo.Groceries = new List<string>() { "apples", "bananas" };
            MyToDo.Chores = new List<string>() { "homework", "cleaning" };

            return View(MyToDo);
        }
    }
}
