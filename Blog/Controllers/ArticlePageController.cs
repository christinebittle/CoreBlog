using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class ArticlePageController : Controller
    {

        private readonly ArticleAPIController _api;
        // dependency
        public ArticlePageController(ArticleAPIController api)
        {
            _api = api;
        }

        // GET : ArticlePage/List?SearchKey={SearchKey} -> A webpage that shows all articles in the database
        [HttpGet]
        public IActionResult List(string SearchKey=null)
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

            // direct to /views/article/show.cshtml
            return View(SelectedArticle);
        }
    }
}