using Microsoft.AspNetCore.Mvc;
using Blog.Models;

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


        // GET: /TagPage/List -> A webpage which shows all tags in the system
        [HttpGet]
        public IActionResult List()
        {
            // get this information from the database
            List<Tag> Tags = _api.ListTags();

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
