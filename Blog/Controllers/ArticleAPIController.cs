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

        // article cread, read, update, delete

        /// <summary>
        /// Receives a Tag Id, returns all articles with that tag
        /// </summary>
        /// <param name="TagId">The foreign key Tag ID</param>
        /// <returns>All articles associated to that tag</returns>
        /// <example>
        /// GET api/TagAPI/ListArticlesForTag/20 ->
        /// [{"ArticleId":"200","ArticleTitle":"My encounter in mexico","ArticleBody":"I had a great time travelling"},{"ArticleId":"201","ArticleTitle":"My encounter in the united states","ArticleBody":"I had a not so great time on my travels"},..]
        /// </example>
        [HttpGet(template: "ListArticlesForTag/{TagId}")]
        public List<Article> ListArticlesForTag(int TagId)
        {
            //todo: implement
            return new List<Article>();
        }

        /// <summary>
        /// Receives an Author Id, returns all articles written by that author
        /// </summary>
        /// <param name="AuthorId">The foreign key Author ID</param>
        /// <returns>All articles written by the author</returns>
        /// <example>
        /// GET api/TagAPI/ListArticlesForAuthor/300 ->
        /// [{"ArticleId":"200","ArticleTitle":"My great travels","ArticleBody":"I had a great time travelling"},{"ArticleId":"400","ArticleTitle":"My nice trip","ArticleBody":"I enjoyed the weather"},..]
        /// </example>
        /// <remarks>
        /// check:
        /// select count(articleid), authorid from articles group by authorid having count(articleid)>3 order by authorid desc; 
        /// </remarks>
        [HttpGet(template:"ListArticlesForAuthor/{AuthorId}")]
        public List<Article> ListArticlesForAuthor(int AuthorId)
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
                string query = "select * from articles where authorid=@authorid";

                //set the database command text to the query
                Command.CommandText = query;

                Command.Parameters.AddWithValue("@authorid", AuthorId);

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
            return Articles;
        }
    }
}
