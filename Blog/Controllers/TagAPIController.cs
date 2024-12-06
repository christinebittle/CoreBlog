using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagAPIController : ControllerBase
    {
        // tag Create, Read, Update, Delete


        /// <summary>
        /// Receives an Article Id, returns all tags for that article
        /// </summary>
        /// <param name="ArticleId">The foreign key Article ID</param>
        /// <returns>All tags associated to that article</returns>
        /// <example>
        /// GET api/TagAPI/ListTagsForArticle/101 ->
        /// [{"TagId":"1", "TagName":"Adventure","TagColor":"000000"},{"TagId":"2", "TagName":"Fine Dining","TagColor":"111111"}]
        /// </example>
        [HttpGet(template:"ListTagsForArticle/{ArticleId}")]
        public List<Tag> ListTagsForArticle(int ArticleId)
        {
            //todo: implement
            return new List<Tag>();
        }

        /// <summary>
        /// Receives a TagId and ArticleId, creating an association with that tag and that article
        /// </summary>
        /// <param name="TagId">Tag Id primary key</param>
        /// <param name="ArticleId">Article Id primary key</param>
        /// <returns>True if the association is created, false otherwise</returns>
        /// <example>
        /// POST: api/TagAPI/LinkTagArticle?TagId=2&ArticleId=200 -> True
        /// </example>
        [HttpPost(template:"LinkTagArticle")]
        public bool LinkTagArticle(int TagId, int ArticleId)
        {
            //todo: implement
            return false;
        }

        /// <summary>
        /// Receives a TagId and ArticleId, removing an association with that tag and that article
        /// </summary>
        /// <param name="TagId">Tag Id primary key</param>
        /// <param name="ArticleId">Article Id primary key</param>
        /// <returns>True if the association is removed, false otherwise</returns>
        /// <example>
        /// Delete: api/TagAPI/UnlinkTagArticle?TagId=3&ArticleId=202 -> True
        /// </example>
        [HttpDelete(template: "UnlinkTagArticle")]
        public bool UnlinkTagArticle(int TagId, int ArticleId)
        {
            //todo: implement
            return false;
        }

    }
}
