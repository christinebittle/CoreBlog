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
        public IActionResult List(string SearchKey = null)
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

        // GET: /TagPage/New
        [HttpGet]
        public IActionResult New()
        {
            // directs to /Views/TagPage/New.cshtml
            return View();
        }

        // POST: /TagPage/Create
        // Request Header: Content-Type: application/x-www-form-urlencoded
        // Request Body: TagName={name}&TagColor={color}
        // -> A page showing the tag we just added
        [HttpPost]
        public IActionResult Create(string TagName, string TagColor)
        {
            Debug.WriteLine($"{TagName} {TagColor}");

            Tag NewTag = new Tag();
            NewTag.TagName = TagName;
            NewTag.TagColor = TagColor;

            // we want to enter the tag into the database
            int TagId = _api.AddTag(NewTag);

            // where do we go after creating a tag?
            // /TagPage/Show/{id}
            return RedirectToAction("Show", new { id = TagId });
        }

        // GET: /TagPage/DeleteConfirm/{id} -> A webpage which confirms the tag to delete
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            // we have an id of a tag
            Tag SelectedTag = _api.FindTag(id);
            // we need information about that tag

            // Directs to /Views/Tag/DeleteConfirm.cshtml
            return View(SelectedTag);
        }

        // POST: /TagPage/Delete/{id} -> To the list of tags
        [HttpPost]
        public IActionResult Delete(int id)
        {
            // todo: delete the tag

            _api.DeleteTag(id);

            // redirect to the list
            return RedirectToAction("List");
        }

        // GET : /TagPage/Edit/{id} -> A webpage which presents the user with existing tag information and asks them how they would like to change it
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Tag SelectedTag = _api.FindTag(id);

            // directs to /Views/TagPage/Edit.cshtml
            return View(SelectedTag);
        }

        // POST: /TagPage/Update/{id}
        // Content-Type: application/x-www-url-formencoded
        // Request Body: TagName={tagname}&TagColor={color}
        // -> A webpage about the tag we have just updated

        [HttpPost]
        public IActionResult Update(int id, string TagName, string TagColor)
        {
            //todo: update the tag information given the tag object
            Tag UpdatedTag = new Tag();
            UpdatedTag.TagName = TagName;
            UpdatedTag.TagColor = TagColor;
            // UpdatedTag.TagId = id; //?
            _api.UpdateTag(id, UpdatedTag);

            Debug.WriteLine($"The tag name is {TagName}");
            Debug.WriteLine($"The tag name is {TagColor}");

            // direct to /TagPage/Show.cshtml with {id} provided
            return RedirectToAction("Show", new { id = id });
        }

    }
}