namespace HackerNewsService.Models
{
    public class NewsShow : NewsItem
    {
        public string title { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public int score { get; set; }
    }
}
