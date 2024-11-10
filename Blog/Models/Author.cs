namespace Blog.Models
{
    public class Author
    {

        public int AuthorId { get; set; }

        public string? AuthorFName { get; set; }

        public string? AuthorLName { get; set; }

        public DateTime AuthorJoinDate { get; set; }

        public string? AuthorBio { get; set; }

        // number of articles that author has written
        public int NumArticles { get; set; }
    }
}
