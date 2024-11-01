using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Blog.Controllers
{
    [Route("api/Article")]
    [ApiController]
    public class ArticleAPIController : ControllerBase
    {

        // get information about the database
        private readonly BlogDbContext _context;
        // referred to the example
        public ArticleAPIController(BlogDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method will return article titles
        /// </summary>
        /// <example>
        /// GET: api/Article/ListArticleTitles -> ["My great adventure in Spain","My harrowing ordeal in the United States"]
        /// </example>
        /// <returns>A list of strings</returns>
        [HttpGet]
        [Route(template:"ListArticleTitles")]
        public List<string> ListArticleTitles()
        {
            //create an empty list for the article titles
            List<string> ArticleTitles = new List<string>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                //Create a connection to the database

                //open the connection to the database
                Connection.Open();

                //create a database command
                MySqlCommand Command = Connection.CreateCommand();

                //create a string for the query ""
                string query = "select articletitle, articledate from articles";

                //set the database command text to the query
                Command.CommandText = query;

                // Gather Result Set of Query into a variable



                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //read through the results in a loop
                    while (ResultSet.Read())
                    {
                        // for each result, gather the article title
                        string ArticleTitle = ResultSet["articletitle"].ToString();

                        ArticleTitles.Add(ArticleTitle);
                    }

                }

            }


            //return the article titles

            return ArticleTitles;
        }

    }
}
