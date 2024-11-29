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

        /// <summary>
        /// Receives a GET request and list the comments from the database
        /// </summary>
        /// <returns>A response of comment objects</returns>
        /// <example>
        /// GET : api/CommentAPI/ListComments -> 
        /// [{"CommentId":"1","CommentDesc":"That was a great article","CommentRating":3},{"CommentId":"2","CommentDesc":"I learned a lot","CommentRating":4},..]
        /// </example>
        [HttpGet]
        [Route(template: "/api/CommentAPI/ListComments")]
        public List<Comment> ListComments()
        {
            List<Comment> Comments = new List<Comment>();

            // get the database context
            BlogDbContext Blog = new BlogDbContext();

            // Create a connection to the database
            MySqlConnection Connection = Blog.AccessDatabase();

            // open the connection
            Connection.Open();

            // create a command
            MySqlCommand Command = Connection.CreateCommand();
            Command.CommandText = "SELECT * FROM comments WHERE commentid > 10 and commentid < 30";

            // execute the command
            MySqlDataReader ResultSet = Command.ExecuteReader();

            // read through the results
            while (ResultSet.Read())
            {
                int CommentId = Convert.ToInt32(ResultSet["commentid"]);
                string CommentDesc = ResultSet["commentdesc"].ToString();
                int Rating = Convert.ToInt32(ResultSet["commentrating"]);

                Comment NewComment = new Comment();

                NewComment.CommentId = CommentId;
                NewComment.CommentDesc = CommentDesc;
                NewComment.Rating = Rating;

                // add them to a list
                Comments.Add(NewComment);
            }

            

            // return the list

            return Comments;
        }

    }
}
