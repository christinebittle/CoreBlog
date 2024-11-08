using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
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
        /// GET: api/Article/ListArticleTitles -> [{"ArticleId":"200","ArticleTitle":"My encounter in mexico","ArticleBody":"I had a great more time"},{"ArticleId":"201","ArticleTitle":"My encounter in the united states","ArticleBody":"I had a not so great time"}]
        /// </example>
        /// <returns>A list of strings</returns>
        [HttpGet]
        [Route(template: "ListArticles")]
        public List<Article> ListArticles()
        {
            //create an empty list for the article titles
            List<Article> Articles = new List<Article>();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                //Create a connection to the database

                //open the connection to the database
                Connection.Open();

                //create a database command
                MySqlCommand Command = Connection.CreateCommand();

                //create a string for the query ""
                string query = "select * from articles";

                //set the database command text to the query
                Command.CommandText = query;

                // Gather Result Set of Query into a variable



                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //read through the results in a loop
                    while (ResultSet.Read())
                    {
                        Article CurrentArticle = new Article();
                        // for each result, gather the article information
                        CurrentArticle.ArticleId = Convert.ToInt32(ResultSet["articleid"]);
                        CurrentArticle.ArticleTitle = ResultSet["articletitle"].ToString();
                        CurrentArticle.ArticleBody = ResultSet["articlebody"].ToString();

                        Articles.Add(CurrentArticle);
                    }

                }

            }


            //return the articles
            return Articles;
        }

        /// <summary>
        /// Receives an Article Id and returns the associated article information
        /// </summary>
        /// <param name="ArticleId">The primary key of the article table</param>
        /// <returns>The associated article object matching the primary key</returns>
        /// <example>
        /// GET api/FindArticle/200 -> {"ArticleId":"200","ArticleTitle":"My encounter in mexico","ArticleBody":"I had a great more time"}
        /// </example>
        [HttpGet]
        [Route(template:"FindArticle/{ArticleId}")]
        public Article FindArticle(int ArticleId)
        {
            Article SelectedArticle = new Article();


            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // finding a specific article by its id
                string query = "select * from articles where articleid="+ArticleId;

                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = query;

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    

                    while (ResultSet.Read())
                    {
                        SelectedArticle.ArticleId = Convert.ToInt32(ResultSet["articleid"]);
                        SelectedArticle.ArticleTitle = ResultSet["articletitle"].ToString();
                        SelectedArticle.ArticleBody = ResultSet["articlebody"].ToString();
                    }

                }

            }

            return SelectedArticle;
        }

    }
}
