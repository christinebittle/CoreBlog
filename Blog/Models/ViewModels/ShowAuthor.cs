namespace Blog.Models.ViewModels
{
    public class ShowAuthor
    {
        // This model packages information about the author

        // The author we want to show
        public Author Author { get; set; }

        // Articles Written by the Author
        public IEnumerable<Article> ArticlesWritten { get; set; } 

        // Comments Written by the Author
        public IEnumerable<Comment> CommentsWritten { get; set; }

        
    }
}
