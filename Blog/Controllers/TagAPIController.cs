using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Blog.Controllers
{
    [Route("api/Tag")]
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
        /// Returns a list of tag names in the blog database
        /// </summary>
        /// <returns>A list of strings</returns>
        /// <example>
        /// GET api/Tag/ListTagNames -> ["Cooking","LifeStyle","Adventure","Travel"]
        /// </example>
        [HttpGet]
        [Route(template:"ListTagNames")]
        public List<string> ListTagNames()
        {
            // create a list of strings for the tag names
            List<string> TagNames = new List<string>();

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
                        // for each tag, add to a list of tag names
                        string TagName = ResultSet["tagname"].ToString();

                        TagNames.Add(TagName);
                    }
                }
            }
                
            // return the list of tag names

            return TagNames;
        }

    }
}
