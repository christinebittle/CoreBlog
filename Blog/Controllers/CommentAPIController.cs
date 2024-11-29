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
        /// This method provides information from the comments table of our blog database
        /// </summary>
        /// <returns>An array of Comment Objects</returns>
        /// <example>
        /// GET: localhost:7272/api/CommentAPI/ListComments ->
        /// [{"CommentId":1,"CommentDesc":"That was a great article!"},{"CommentId":2,"CommentDesc":"Fun read!"},..]
        /// </example>
        [HttpGet]
        [Route(template: "/api/CommentAPI/ListComments")]
        public List<Comment> ListComments()
        {
            // get a variable that represents the connection to the blog database
            BlogDbContext Blog = new BlogDbContext();

            // a connection from the variable
            MySqlConnection Connection = Blog.AccessDatabase();
            Connection.Open();

            // run a query against that database
            string query = "select * from comments where commentid < 10";

            // run a command
            MySqlCommand Command = Connection.CreateCommand();
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


        // POST : /api/CommentAPI/AddComment
        // Request Header: Content-Type: application/json
        // Request Body:
        // {"CommentDesc":"I liked that article","Rating":3}
        // -> 1015
        [HttpPost]
        [Route(template:"/api/CommentAPI/AddComment")]
        public int AddComment([FromBody] Comment NewComment)
        {
            // get a variable that represents the connection to the blog database
            BlogDbContext Blog = new BlogDbContext();

            // a connection from the variable
            MySqlConnection Connection = Blog.AccessDatabase();
            Connection.Open();

            // run a query against that database
            string query = "insert into comments (articleid, authorid, commentdesc, commentdate, commentrating) values (0,0, @desc, CURRENT_DATE(), @rating)";

            // run a command
            MySqlCommand Command = Connection.CreateCommand();

            Command.Parameters.AddWithValue("@desc", NewComment.CommentDesc);
            Command.Parameters.AddWithValue("@rating", NewComment.Rating);

            Command.CommandText = query;

            Command.ExecuteNonQuery();

            return Convert.ToInt32(Command.LastInsertedId);
        }

    }
}
