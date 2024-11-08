using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Mysqlx.Resultset;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagAPIController : ControllerBase
    {
        // dependency injection
        private readonly BlogDbContext _context;

        public TagAPIController(BlogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of tags in the blog database
        /// </summary>
        /// <returns>A list of tag objects</returns>
        /// <example>
        /// GET api/Tag/ListTags -> [{"TagId":1,"TagName":"Adventure","TagColor":"FF0000"},{"TagId":2,"TagName":"LifeStyle","TagColor":"0000FF"},{"TagId":3,"TagName":"Cooking","TagColor":"00FF00"},..]
        /// </example>
        [HttpGet]
        [Route(template: "ListTags")]
        public List<Tag> ListTags()
        {
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
                string query = "select * from tags";

                // set the command text to the query
                Command.CommandText = query;

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
        [Route(template:"FindTag/{TagId}")]
        public Tag FindTag(int TagId)
        {
            Tag SelectedTag = new Tag();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // opening the connection
                Connection.Open();

                // setting up an sql query
                string query = "select * from tags where tagid="+TagId;

                // setting the command text to the query
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = query;

                // use the result set to get information about the tag
                using(MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    
                    while (ResultSet.Read())
                    {
                        //get information about the tag
                        SelectedTag.TagId = Convert.ToInt32(ResultSet["tagid"]);
                        SelectedTag.TagName = ResultSet["tagname"].ToString();
                        SelectedTag.TagColor = ResultSet["tagcolor"].ToString();
                    }

                }
            }

            return SelectedTag;
        }

    }
}
