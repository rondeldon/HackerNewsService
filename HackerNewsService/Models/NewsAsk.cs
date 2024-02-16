namespace HackerNewsService.Models
{
    /// <summary>
    /// A news ask item
    /// </summary>
    public class NewsAsk : NewsItem
    {
        public string title { get; set; }
        public string text { get; set; }
        /// <summary>
        /// Score from 1 to 99
        /// </summary>
        public int score { get; set; }
    }
}
