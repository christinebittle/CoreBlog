using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagAPIController : ControllerBase
    {
        // tag Create, Read, Update, Delete

        // dependency injection
        private readonly BlogDbContext _context;

        public TagAPIController(BlogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of tags in the blog database. Filtered by an optional search key criteria
        /// </summary>
        /// <param name="SearchKey">The search key to search for tag names</param>
        /// <returns>A list of tag objects</returns>
        /// <example>
        /// GET api/Tag/ListTags -> [{"TagId":1,"TagName":"Adventure","TagColor":"FF0000"},{"TagId":2,"TagName":"LifeStyle","TagColor":"0000FF"},{"TagId":3,"TagName":"Cooking","TagColor":"00FF00"},..]
        /// GET api/Tag/ListTags?SearchKey=Dining -> [{"TagId":1,"TagName":"Fine Dining","TagColor":"FF0000"},{"TagId":2,"TagName":"Casual Dining","TagColor":"0000FF"},{"TagId":3,"TagName":"Budget Dining","TagColor":"00FF00"},..]
        /// </example>
        [HttpGet]
        [Route(template: "ListTags")]
        public List<Tag> ListTags(string SearchKey = null)
        {
            Debug.WriteLine($"The api received a key {SearchKey}");

            // create a list of strings for the tag names
            List<Tag> Tags = new List<Tag>();

            // Create a connection
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // open the connection
                Connection.Open();

                // create a command with the connection
                MySqlCommand Command = Connection.CreateCommand();

                // create an sql query
                string query = "select * from tags where tagname like @searchkey";

                // set the command text to the query
                Command.CommandText = query;

                Command.Parameters.AddWithValue("@searchkey", $"%{SearchKey}%");

                Command.Prepare();

                // execute the command and  build the result set
                // using will close the result set for us
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // loop through the result set
                    while (ResultSet.Read())
                    {
                        Tag CurrentTag = new Tag();
                        // for each tag, add to a list of tags
                        CurrentTag.TagId = Convert.ToInt32(ResultSet["tagid"]);
                        CurrentTag.TagName = ResultSet["tagname"].ToString();
                        CurrentTag.TagColor = ResultSet["tagcolor"].ToString();

                        Tags.Add(CurrentTag);
                    }
                }
            }

            // return the list of tags

            return Tags;
        }

        /// <summary>
        /// Output a tag associated with the input tag id.
        /// </summary>
        /// <param name="TagId">The primary key of the tag</param>
        /// <returns>An object associated with the tag</returns>
        /// <example>
        /// GET: api/Tag/FindTag/3 -> {"TagId":"3","TagName":"LifeStyle","TagColor":"ff0000"}
        /// </example>
        [HttpGet]
        [Route(template: "FindTag/{TagId}")]
        public Tag FindTag(int TagId)
        {
            Tag SelectedTag = new Tag();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // opening the connection
                Connection.Open();

                // setting up an sql query
                string query = "select count(articles.articleid) as numarticles, tags.* from tags left join articlesxtags on tags.tagid=articlesxtags.tagid left join articles on articlesxtags.articleid=articles.articleid where tags.tagid=@id group by articlesxtags.tagid";

                // setting the command text to the query
                MySqlCommand Command = Connection.CreateCommand();
                Command.Parameters.AddWithValue("@id", TagId);

                Command.CommandText = query;

                // use the result set to get information about the tag
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {

                    while (ResultSet.Read())
                    {
                        //get information about the tag
                        SelectedTag.TagId = Convert.ToInt32(ResultSet["tagid"]);
                        SelectedTag.TagName = ResultSet["tagname"].ToString();
                        SelectedTag.TagColor = ResultSet["tagcolor"].ToString();
                        SelectedTag.NumArticles = Convert.ToInt32(ResultSet["numarticles"]);
                    }

                }
            }

            return SelectedTag;
        }

        /// <summary>
        /// Receives Tag information and adds it to the database
        /// </summary>
        /// <param name="NewTag">A Tag Object</param>
        /// <returns>
        /// The resource identifier of the tag we have just added. 0 if unsuccessful
        /// </returns>
        /// <example>
        /// POST: api/TagAPI/AddTag
        /// Request Header: Content-Type: application/json
        /// Request Body:
        /// {"TagName":"Coding", "TagColor":"000000"}
        /// -> 14
        /// </example>
        [HttpPost(template: "AddTag")]
        public int AddTag(Tag NewTag)
        {
            // the query to insert the new tag

            // what query allows us to add the tag into the db?
            string query = "insert into tags (tagname,tagcolor) values (@name, @color)";

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = query;
                Command.Parameters.AddWithValue("@name", NewTag.TagName);
                Command.Parameters.AddWithValue("@color", NewTag.TagColor);

                Command.ExecuteNonQuery();

                int TagId = Convert.ToInt32(Command.LastInsertedId);

                return TagId;
            }

            return 0;
        }

        /// <summary>
        /// Receive a tag id and delete the associated tag in the database
        /// </summary>
        /// <param name="TagId">The tag id primary key</param>
        /// <returns>
        /// The number of rows affected by the delete
        /// </returns>
        /// <example>
        /// DELETE api/TagAPI/DeleteTag/10 -> 1
        /// DELETE api/TagAPI/DeleteTag/-100 -> 0
        /// </example>
        [HttpDelete(template: "DeleteTag/{TagId}")]
        public int DeleteTag(int TagId)
        {
            // how do I delete a tag by its id?
            string query = "delete from tags where tagid=@id";

            using (MySqlConnection Connection = _context.AccessDatabase())
            {

                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = query;
                Command.Parameters.AddWithValue("@id", TagId);

                return Command.ExecuteNonQuery();
            }

            return 0;
        }

        /// <summary>
        /// Receives tag information and updates that tag in the database
        /// </summary>
        /// <returns>
        /// The tag which we had just updated
        /// </returns>
        /// <param name="TagId">The tag id primary key</param>
        /// <param name="UpdatedTag">Tag Data</param>
        /// <example>
        /// PUT : api/TagAPI/UpdateTag/4
        /// Headers: Content-Type: application/json
        /// Request Body: {"TagName":"Skydiving", "TagColor":"ab1212"}
        /// -> {"TagId":"4", "TagName":"Skydiving", "TagColor":"ab1212", "NumArticles":100}
        /// </example>
        [HttpPut(template: "UpdateTag/{TagId}")]
        public Tag UpdateTag(int TagId, [FromBody] Tag UpdatedTag)
        {
            string query = "update tags set tagname=@name, tagcolor=@color where tagid=@id";

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = query;
                Command.Parameters.AddWithValue("@name", UpdatedTag.TagName);
                Command.Parameters.AddWithValue("@color", UpdatedTag.TagColor);
                Command.Parameters.AddWithValue("@id", TagId);

                Command.ExecuteNonQuery();

            } // connection will close by itself

            return FindTag(TagId);
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
