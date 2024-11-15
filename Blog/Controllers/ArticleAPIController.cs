using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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
        /// This method will return articles. Will find articles that match the optional search key input
        /// </summary>
        /// <param name="SearchKey">The key to search article titles and article content against</param>
        /// <example>
        /// GET: api/Article/ListArticles?SearchKey=Travel ->
        /// [{"ArticleId":"200","ArticleTitle":"My encounter in mexico","ArticleBody":"I had a great time travelling"},{"ArticleId":"201","ArticleTitle":"My encounter in the united states","ArticleBody":"I had a not so great time on my travels"}]
        /// 
        /// GET: api/Article/ListArticles ->
        /// [{"ArticleId":"200","ArticleTitle":"My encounter in mexico","ArticleBody":"I had a great time travelling"},{"ArticleId":"201","ArticleTitle":"My encounter in the united states","ArticleBody":"I had a not so great time on my travels"},..]
        /// </example>
        /// <returns>A list of article objects</returns>
        [HttpGet]
        [Route(template: "ListArticles")]
        public List<Article> ListArticles(string SearchKey=null)
        {
            Debug.WriteLine($"Received Search Key input of :{SearchKey}");

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
                string query = "select * from articles where articletitle like @key";

                //set the database command text to the query
                Command.CommandText = query;

                Command.Parameters.AddWithValue("@key", $"%{SearchKey}%");

                Command.Prepare();

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
        /// GET api/FindArticle/10000000 -> {"ArticleId":,"ArticleTitle":,"ArticleBody":}
        /// </example>
        [HttpGet]
        [Route(template: "FindArticle/{ArticleId}")]
        public Article FindArticle(int ArticleId)
        {
            Article SelectedArticle = new Article();




            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // finding a specific article by its id
                string query = "select count(commentid) as numcomments, articles.* from articles left join comments on articles.articleid=comments.articleid where articles.articleid=@id group by articles.articleid ";

                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();

                Command.Parameters.AddWithValue("@id",ArticleId);

                Command.CommandText = query;

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {


                    while (ResultSet.Read())
                    {
                        SelectedArticle.ArticleId = Convert.ToInt32(ResultSet["articleid"]);
                        SelectedArticle.ArticleTitle = ResultSet["articletitle"].ToString();
                        SelectedArticle.ArticleBody = ResultSet["articlebody"].ToString();
                        SelectedArticle.NumComments = Convert.ToInt32(ResultSet["numcomments"]);
                    }

                }

            }

            return SelectedArticle;
        }

    }
}