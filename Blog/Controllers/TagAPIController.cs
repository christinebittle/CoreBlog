using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using MySql.Data.MySqlClient;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagAPIController : ControllerBase
    {
        // tag Create, Read, Update, Delete

        // get information about the database
        private readonly BlogDbContext _context;
        // referred to the example
        public TagAPIController(BlogDbContext context)
        {
            _context = context;
        }

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
            

            List<Tag> Tags = new List<Tag>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                //Create a connection to the database

                //open the connection to the database
                Connection.Open();

                //create a database command
                MySqlCommand Command = Connection.CreateCommand();

                //create a string for the query ""
                string query = "SELECT articleid, tags.tagname, tags.tagcolor, tags.tagid FROM `articlesxtags` inner join tags on tags.tagid = articlesxtags.tagid WHERE articleid=@articleid";

                //set the database command text to the query
                Command.CommandText = query;

                Command.Parameters.AddWithValue("@articleid", ArticleId);

                Command.Prepare();

                // Gather Result Set of Query into a variable



                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //read through the results in a loop
                    while (ResultSet.Read())
                    {
                        Tag CurrentTag = new Tag();
                        // for each result, gather the article information

                        CurrentTag.TagId = Convert.ToInt32(ResultSet["tagid"]);

                        CurrentTag.TagName = ResultSet["tagname"].ToString();

                        CurrentTag.TagColor = ResultSet["tagcolor"].ToString(); ;
                        
                        Tags.Add(CurrentTag);
                    }

                }

            }


            //return the articles
            return Tags;

            
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
