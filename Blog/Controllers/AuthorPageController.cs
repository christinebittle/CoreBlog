using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;

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

        //GET : AuthorPage/List/SearchKey={SearchKey}
        [HttpGet]
        public IActionResult List(string SearchKey)
        {
            List<Author> Authors = _api.ListAuthors(SearchKey);
            return View(Authors);
        }

        //GET : AuthorPage/Show/{id}
        [HttpGet]
        public IActionResult Show(int id)
        {
            Author SelectedAuthor = _api.FindAuthor(id);
            return View(SelectedAuthor);
        }

        // GET : AuthorPage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        // POST: AuthorPage/Create
        [HttpPost]
        public IActionResult Create(Author NewAuthor)
        {
            int AuthorId = _api.AddAuthor(NewAuthor);

            // redirects to "Show" action on "Author" cotroller with id parameter supplied
            return RedirectToAction("Show", new {id = AuthorId});
        }

        // GET : AuthorPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Author SelectedAuthor = _api.FindAuthor(id);
            return View(SelectedAuthor);
        }

        // POST: AuthorPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int AuthorId = _api.DeleteAuthor(id);
            // redirects to list action
            return RedirectToAction("List");
        }

    }
}
