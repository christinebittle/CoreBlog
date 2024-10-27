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

        [HttpGet]
        [Route(template:"ListAuthorNames")]
        public List<string> ListAuthorNames()
        {
            //Create an empty list of Author Names
            List<string> AuthorNames = new List<string>();

            using(MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand cmd = Connection.CreateCommand();

                //SQL QUERY
                cmd.CommandText = "select * from authors";

                //Gather Result Set of Query into a variable
                MySqlDataReader ResultSet = cmd.ExecuteReader();

                //Loop Through Each Row the Result Set
                while (ResultSet.Read())
                {
                    //Access Column information by the DB column name as an index
                    string AuthorName = ResultSet["authorfname"] + " " + ResultSet["authorlname"];
                    //Add the Author Name to the List
                    AuthorNames.Add(AuthorName);
                }
            }

            //Return the final list of author names
            return AuthorNames;
        }
    }
}
