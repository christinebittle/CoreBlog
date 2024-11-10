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
        /// Returns a list of Authors in the system. If a search key is included, search for authors with a first or last name matching.
        /// </summary>
        /// <example>
        /// GET api/Author/ListAuthors?SearchKey=Sam -> [{"AuthorId":1,"AuthorFname":"Sam", "AuthorLName":"Smith"},{"AuthorId":2,"AuthorFname":"Jillian", "AuthorLName":"Samuel"},..]
        /// </example>
        /// <returns>
        /// A list of author objects 
        /// </returns>
        [HttpGet]
        [Route(template:"ListAuthors")]
        public List<Author> ListAuthors(string SearchKey=null)
        {
            // Create an empty list of Authors
            List<Author> Authors = new List<Author>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                string query = "select * from authors";
                
                // search criteria, first, last or first + last
                if (SearchKey != null)
                {
                    query += " where lower(authorfname) like @key or lower(authorlname) like @key or lower(concat(authorfname,' ',authorlname)) like @key";
                    Command.Parameters.AddWithValue("@key",$"%{SearchKey}%");
                }
                //SQL QUERY
                Command.CommandText = query;
                Command.Prepare();

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["authorid"]);
                        string FirstName = ResultSet["authorfname"].ToString();
                        string LastName = ResultSet["authorlname"].ToString();

                        DateTime AuthorJoinDate = Convert.ToDateTime(ResultSet["authorjoindate"]);
                        string AuthorBio = ResultSet["authorbio"].ToString();

                        //short form for setting all properties while creating the object
                        Author CurrentAuthor = new Author()
                        {
                            AuthorId=Id,
                            AuthorFName=FirstName,
                            AuthorLName=LastName,
                            AuthorJoinDate=AuthorJoinDate,
                            AuthorBio=AuthorBio
                        };

                        Authors.Add(CurrentAuthor);

                    }
                }                    
            }
            

            //Return the final list of authors
            return Authors;
        }


        /// <summary>
        /// Returns an author in the database by their ID
        /// </summary>
        /// <example>
        /// GET api/Author/FindAuthor/3 -> {"AuthorId":3,"AuthorFname":"Sam","AuthorLName":"Cooper","AuthorJoinDate":"2020-10-11", "AuthorBio":"Fun Guy", "NumArticles":1}
        /// </example>
        /// <returns>
        /// A matching author object by its ID. Empty object if Author not found
        /// </returns>
        [HttpGet]
        [Route(template: "FindAuthor/{id}")]
        public Author FindAuthor(int id)
        {
            
            //Empty Author
            Author SelectedAuthor = new Author();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // @id is replaced with a sanitized id
                // 'how many' 'articles' <> count(articleid)
                // 'for each' 'author' <> group by (authorid)
                Command.CommandText = "select authors.*, count(articles.articleid) as numarticles from authors left join articles on (articles.authorid=authors.authorid) where authors.authorid=@id group by authors.authorid";
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int Id = Convert.ToInt32(ResultSet["authorid"]);
                        string FirstName = ResultSet["authorfname"].ToString();
                        string LastName = ResultSet["authorlname"].ToString();

                        DateTime AuthorJoinDate = Convert.ToDateTime(ResultSet["authorjoindate"]);
                        string AuthorBio = ResultSet["authorbio"].ToString();

                        int NumArticles = Convert.ToInt32(ResultSet["numarticles"]);

                        SelectedAuthor.AuthorId = Id;
                        SelectedAuthor.AuthorFName = FirstName;
                        SelectedAuthor.AuthorLName = LastName;
                        SelectedAuthor.AuthorBio = AuthorBio;
                        SelectedAuthor.AuthorJoinDate = AuthorJoinDate;
                        SelectedAuthor.NumArticles = NumArticles;
                    }
                }
            }


            //Return the final list of author names
            return SelectedAuthor;
        }
    }
}
