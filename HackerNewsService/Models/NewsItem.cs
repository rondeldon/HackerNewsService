namespace HackerNewsService.Models
{
    public class NewsItem
    {
        public int id { get; set; }
        public string by { get; set; }
        public int time { get; set; }
        public List<int> kids { get; set; }
        public string type { get; set; }
        public bool deleted { get; set; }
        public bool dead { get; set; }

    }
}
