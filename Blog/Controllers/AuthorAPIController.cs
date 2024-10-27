using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using System;
using MySql.Data.MySqlClient;

namespace Blog.Controllers
{
    [Route("api/Author")]
    [ApiController]
    public class AuthorAPIController : ControllerBase
    {
        private readonly BlogDbContext _context;
        // dependency injection of database context
        public AuthorAPIController(BlogDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Returns a list of Authors in the system
        /// </summary>
        /// <example>
        /// GET api/Author/ListAuthorNames -> ["Brian Smith","Jillian Montgomery",..]
        /// </example>
        /// <returns>
        /// A list of strings, formatted "{First Name} {Last Name}"
        /// </returns>
        [HttpGet]
        [Route(template:"ListAuthorNames")]
        public List<string> ListAuthorNames()
        {
            // Create an empty list of Author Names
            List<string> AuthorNames = new List<string>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "select * from authors";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        string AuthorFName = ResultSet["authorfname"].ToString();
                        string AuthorLName = ResultSet["authorlname"].ToString();
                        //Access Column information by the DB column name as an index
                        string AuthorName = $"{AuthorFName} {AuthorLName}";
                        //Add the Author Name to the List
                        AuthorNames.Add(AuthorName);
                    }
                }                    
            }
            

            //Return the final list of author names
            return AuthorNames;
        }
    }
}
