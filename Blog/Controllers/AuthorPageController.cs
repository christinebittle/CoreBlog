using Blog.Models;
using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;

namespace Blog.Controllers
{

    public class AuthorPageController : Controller
    {
        // currently relying on the API to retrieve author/article/comment information
        // this is a simplified example. In practice, both the AuthorAPI and AuthorPage controllers
        // should rely on a unified "Service", with an explicit interface
        private readonly AuthorAPIController _authorapi;
        private readonly CommentAPIController _commentapi;
        private readonly ArticleAPIController _articleapi;

        public AuthorPageController(AuthorAPIController authorapi, ArticleAPIController articleapi, CommentAPIController commentapi)
        {
            _authorapi = authorapi;
            _articleapi = articleapi;
            _commentapi = commentapi;
        }

        //GET : AuthorPage/List/SearchKey={SearchKey}
        [HttpGet]
        public IActionResult List(string SearchKey)
        {
            List<Author> Authors = _authorapi.ListAuthors(SearchKey);
            return View(Authors);
        }

        //GET : AuthorPage/Show/{id}
        [HttpGet]
        public IActionResult Show(int id)
        {
            Author SelectedAuthor = _authorapi.FindAuthor(id);
            IEnumerable<Article> Articles = _articleapi.ListArticlesForAuthor(id);
            IEnumerable<Comment> Comments = _commentapi.ListCommentsForAuthor(id);

            ShowAuthor ViewModel = new ShowAuthor();
            ViewModel.Author = SelectedAuthor;
            ViewModel.ArticlesWritten = Articles;
            ViewModel.CommentsWritten = Comments;



            return View(ViewModel);
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
            int AuthorId = _authorapi.AddAuthor(NewAuthor);

            // redirects to "Show" action on "Author" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = AuthorId });
        }

        // GET : AuthorPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Author SelectedAuthor = _authorapi.FindAuthor(id);
            return View(SelectedAuthor);
        }

        // POST: AuthorPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int AuthorId = _authorapi.DeleteAuthor(id);
            // redirects to list action
            return RedirectToAction("List");
        }

        // GET : AuthorPage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Author SelectedAuthor = _authorapi.FindAuthor(id);
            return View(SelectedAuthor);
        }

        // POST: AuthorPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string AuthorFName, string AuthorLName, string AuthorEmail, string AuthorBio)
        {
            Author UpdatedAuthor = new Author();
            UpdatedAuthor.AuthorFName = AuthorFName;
            UpdatedAuthor.AuthorLName = AuthorLName;
            UpdatedAuthor.AuthorBio = AuthorBio;
            UpdatedAuthor.AuthorEmail = AuthorEmail;

            // not doing anything with the response
            _authorapi.UpdateAuthor(id, UpdatedAuthor);
            // redirects to show author
            return RedirectToAction("Show", new{id = id});
        }

    }
}
