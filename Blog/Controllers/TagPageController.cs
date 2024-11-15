using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using System.Diagnostics;

namespace Blog.Controllers
{
    public class TagPageController : Controller
    {
        // get access to the component which retrieves data
        // the component is the TagAPIController
        private readonly TagAPIController _api;
        public TagPageController(TagAPIController api)
        {
            _api = api;
        }


        // GET: /TagPage/List?SearchKey={Key} -> A webpage which shows all tags in the system filtered by the optional search criteria
        /// GET: /TagPage/List -> a webpage which shows all tags
        [HttpGet]
        public IActionResult List(string SearchKey=null)
        {
            //log the search key
            Debug.WriteLine($"received a key of {SearchKey}");

            // get this information from the database
            List<Tag> Tags = _api.ListTags(SearchKey);

            // a list of tags to provide to the view

            // direct to /Views/TagPage/List.cshtml
            return View(Tags);
        }

        // GET /TagPage/Show/{id} -> A webpage which shows one specific tag by its id
        [HttpGet]
        public IActionResult Show(int id)
        {
            Tag SelectedTag = _api.FindTag(id);

            //How do we get information about one particular tag if we know the tag id?


            // direct to /Views/Tag/Show.cshtml
            return View(SelectedTag);
        }
    }
}