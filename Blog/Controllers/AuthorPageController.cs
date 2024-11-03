using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    
    public class AuthorPageController : Controller
    {
        // currently relying on the API to retrieve author information
        // this is a simplified example. In practice, both the AuthorAPI and AuthorPage controllers
        // should rely on a unified "Service", with an explicit interface
        private readonly AuthorAPIController _api;
        
        public AuthorPageController(AuthorAPIController api)
        {
            _api = api;
        }

        //GET : AuthorPage/List
        public IActionResult List()
        {
            List<Author> Authors = _api.ListAuthors();
            return View(Authors);
        }

        //GET : AuthorPage/Show/{id}
        public IActionResult Show(int id)
        {
            Author SelectedAuthor = _api.FindAuthor(id);
            return View(SelectedAuthor);
        }

    }
}
