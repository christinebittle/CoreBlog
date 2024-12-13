namespace Blog.Models.ViewModels
{
    public class ShowTag
    {
        // what informationm do I need on the Show Tag page?

        // the tag we want to show
        public Tag SelectedTag { get; set; }
        
        // the articles related to this tag
        public IEnumerable<Article> ArticlesRelated { get; set; }

    }
}
