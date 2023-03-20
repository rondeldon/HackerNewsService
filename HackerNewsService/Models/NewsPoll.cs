namespace HackerNewsService.Models
{
    public class NewsPoll : NewsItem
    {
        public string title { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public List<int> parts { get; set; }
        public int score { get; set; }
    }
}
