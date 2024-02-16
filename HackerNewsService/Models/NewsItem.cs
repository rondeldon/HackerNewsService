namespace HackerNewsService.Models
{
    public class NewsItem
    {
        /// <summary>
        /// Unique identifier of item
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Author of item
        /// </summary>     
        public string by { get; set; }
        public int time { get; set; }
        /// <summary>
        /// Kids of item if there are any
        /// </summary>
        public List<int> kids { get; set; }
        /// <summary>
        /// Type of item (e.g. News, Comment, Job )
        /// </summary>
        public string type { get; set; }
        public bool deleted { get; set; }
        public bool dead { get; set; }

    }
}
