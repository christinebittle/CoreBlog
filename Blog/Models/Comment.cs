namespace Blog.Models
{
    public class Comment
    {

        public int CommentId { get; set; }

        public string CommentDesc { get; set; }

        public int Rating { get; set; }

        //todo: comment rating, article on the comment, who wrote the comment, the comment date
    }
}
