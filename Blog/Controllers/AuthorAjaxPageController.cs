using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class AuthorAjaxPageController : Controller
    {
        // These components do not connect to the data source directly
        // Instead, the client script on each view will call the API.

        public IActionResult List()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
