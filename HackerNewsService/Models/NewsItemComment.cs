namespace HackerNewsService.Models
{
    public class NewsItemComment : NewsItem
    {
        public int parent { get; set; }
        public string text { get; set; }

    }
}