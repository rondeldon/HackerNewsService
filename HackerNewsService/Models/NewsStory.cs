namespace HackerNewsService.Models
{
    public class NewsStory : NewsItem
    {
        public int descendants { get; set; }
        public int score { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }
}
