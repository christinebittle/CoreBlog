using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class ArticlePageController : Controller
    {
        private readonly ArticleAPIController _api;
        // dependency
        // Ideally would have an article service
        // where both the MVC and API can call the service
        public ArticlePageController(ArticleAPIController api)
        {
            _api = api;
        }

        // GET : ArticlePage/List?SearchKey={SearchKey} -> A webpage that shows all articles in the database
        [HttpGet]
        public IActionResult List(string SearchKey = null)
        {
            Debug.WriteLine($"Web Page received key of {SearchKey}");
            List<Article> Articles = _api.ListArticles(SearchKey);

            // get this information from the database

            // send to our article list view

            // direct us to the /Views/ArticlePage/List.cshtml
            return View(Articles);
        }

        // GET : ArticlePage/Show/{id} -> A webpage that shows a particular article in the database matching the given id
        [HttpGet]
        public IActionResult Show(int id)
        {
            Article SelectedArticle = _api.FindArticle(id);

            // direct to /Views/ArticlePage/Show.cshtml
            return View(SelectedArticle);
        }

        // GET : ArticlePage/New -> A webpage that prompts the user to enter new article information
        [HttpGet]
        public IActionResult New()
        {
            // direct to /Views/ArticlePage/New.cshtml
            return View();
        }

        // POST: ArticlePage/Create -> List articles page with the new article added
        // Request Header: Content-Type: application/x-www-url-formencoded
        // Request Body:
        // ArticleTitle={title}&ArticleBody={body}
        [HttpPost]
        public IActionResult Create(string ArticleTitle, string ArticleBody)
        {
            Debug.WriteLine($"Title {ArticleTitle}");
            Debug.WriteLine($"Body {ArticleBody}");

            Article NewArticle = new Article();
            NewArticle.ArticleTitle = ArticleTitle;
            NewArticle.ArticleBody = ArticleBody;

            int ArticleId = _api.AddArticle(NewArticle);

            //redirect to /ArticlePage/Show/{articleid}
            return RedirectToAction("Show", new { id=ArticleId });
        }

        // GET: /ArticlePage/DeleteConfirm/{id} -> A webpage asking the user if they are sure they want to delete this article
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            // problem: get the article information
            // given: the article id
            Article SelectedArticle = _api.FindArticle(id);

            // directs to /Views/ArticlePage/DeleteConfirm.cshtml
            return View(SelectedArticle);
        }

        // POST: ArticlePage/Delete/{id} -> A webpage that lists the articles
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int RowsAffected = _api.DeleteArticle(id);

            //todo: log rows affected

            //direct to Views/ArticlePage/List.cshtml
            return RedirectToAction("List");
        }

    }
}
