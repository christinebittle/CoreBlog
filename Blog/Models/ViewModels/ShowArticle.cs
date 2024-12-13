namespace Blog.Models.ViewModels
{
    public class ShowArticle
    {
        // This model represents the content for viewing an article

        // The article itself
        public Article SelectedArticle { get; set; }

        // The comments written on the article
        public IEnumerable<Comment> CommentsWritten { get; set; }

        //Tags written on the article
        public IEnumerable<Tag> TagsAssociated { get; set; }

    }
}
