using Blog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentAPIController : ControllerBase
    {

        // get information about the database
        private readonly BlogDbContext _context;
        // referred to the example
        public CommentAPIController(BlogDbContext context)
        {
            _context = context;
        }

        // Comment Create, Read, Update, Delete

        /// <summary>
        /// Gets all comments written by a particular author
        /// </summary>
        /// <param name="AuthorId">The author ID foreign key</param>
        /// <returns>An array of Comment Objects</returns>
        /// <example>
        /// GET: localhost:7272/api/CommentAPI/ListCommentsForAuthor/20 ->
        /// [{"CommentId":101,"CommentDesc":"That was a great article!"},{"CommentId":702,"CommentDesc":"Fun read!"},..]
        /// </example>
        /// <remarks>
        /// check:
        /// select count(commentid), authorid from comments group by authorid having count(commentid)>3 order by authorid desc; 
        /// </remarks>
        [HttpGet]
        [Route(template: "/api/CommentAPI/ListCommentsForAuthor/{AuthorId}")]
        public List<Comment> ListCommentsForAuthor(int AuthorId)
        {
            // a connection from the variable
            MySqlConnection Connection = _context.AccessDatabase();
            Connection.Open();

            // run a query against that database
            string query = "select * from comments where authorid=@authorid";

            // run a command
            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("@authorid", AuthorId);

            Command.CommandText = query;
            MySqlDataReader ResultSet = Command.ExecuteReader();


            List<Comment> Comments = new List<Comment>();

            // iterate through the response results
            while (ResultSet.Read())
            {
                // Put that information into a List<string>
                string CommentText = ResultSet["commentdesc"].ToString();
                Comment NewComment = new Comment();
                NewComment.CommentDesc = CommentText;
                NewComment.CommentId = Convert.ToInt32(ResultSet["commentid"]);

                Comments.Add(NewComment);
            }



            return Comments;
        }

        /// <summary>
        /// Gets all comments written on an Article
        /// </summary>
        /// <param name="AuthorId">The Article ID foreign key</param>
        /// <returns>An array of Comment Objects</returns>
        /// <example>
        /// GET: localhost:7272/api/CommentAPI/ListCommentsForArticle/50 ->
        /// [{"CommentId":201,"CommentDesc":"Nice Story!"},{"CommentId":502,"CommentDesc":"I learned a lot, thanks for sharing!"},..]
        /// </example>
        /// <remarks>
        /// check:
        /// select count(commentid), articleid from comments group by articleid having count(commentid)>3 order by articleid desc; 
        /// </remarks>
        [HttpGet]
        [Route(template: "/api/CommentAPI/ListCommentsForArticle/{ArticleId}")]
        public List<Comment> ListCommentsForArticle(int ArticleId)
        {
            

            // a connection from the variable
            MySqlConnection Connection = _context.AccessDatabase();
            Connection.Open();

            // run a query against that database
            string query = "select * from comments where articleid=@articleid";

            // run a command
            MySqlCommand Command = Connection.CreateCommand();
            Command.Parameters.AddWithValue("@articleid", ArticleId);

            Command.CommandText = query;
            MySqlDataReader ResultSet = Command.ExecuteReader();


            List<Comment> Comments = new List<Comment>();

            // iterate through the response results
            while (ResultSet.Read())
            {
                // Put that information into a List<string>
                string CommentText = ResultSet["commentdesc"].ToString();
                Comment NewComment = new Comment();
                NewComment.CommentDesc = CommentText;
                NewComment.CommentId = Convert.ToInt32(ResultSet["commentid"]);

                Comments.Add(NewComment);
            }



            return Comments;
        }

    }
}
