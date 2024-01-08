using HackerNewsService.Exceptions;
using HackerNewsService.Models;
using Newtonsoft.Json;

namespace HackerNewsService.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HackerNewsService> _logger;
        private  SemaphoreSlim _concurrentRequests = new SemaphoreSlim(100);
        public HackerNewsService(HttpClient httpClient, ILogger<HackerNewsService> logger) 
        {
            ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
            ArgumentNullException.ThrowIfNull( logger, nameof(logger));

           _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<List<NewsStory>> GetNewStoriesAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/newstories.json");
            List<int> storyItems = ConvertTo<List<int>>(response);
            List<NewsStory> stories = await GetNewsItems<NewsStory>(storyItems, (item) => item.type == "story" && !item.deleted && !item.dead);
            return stories;
        }
        public async Task<List<NewsStory>> GetTopStoriesAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/topstories.json");
            List<int> storyItems = ConvertTo<List<int>>(response);
            List<NewsStory> stories = await GetNewsItems<NewsStory>(storyItems, (item) => item.type == "story" && !item.deleted && !item.dead);
            return stories;
        }

        public async Task<List<NewsStory>> GetBestStoriesAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/beststories.json");
            List<int> storyItems = ConvertTo<List<int>>(response);
            List<NewsStory> stories = await GetNewsItems<NewsStory>(storyItems, (item) => item.type == "story" && !item.deleted && !item.dead);
            return stories;
        }
        public async Task<List<NewsJob>> GetJobStoriesAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/jobstories.json");
            List<int> jobItems = ConvertTo<List<int>>(response);
            List<NewsJob> jobs = await GetNewsItems<NewsJob>(jobItems, (item) => item.type == "job" && !item.deleted && !item.dead);
            return jobs;
        }

        public async Task<List<NewsItemComment>> GetNewsItemCommentsAsync(int parentId)
        {
            NewsItem newsItem = await GetNewsItem<NewsStory>(parentId);
            List<NewsItemComment> comments =  await GetNewsItems<NewsItemComment>(newsItem.kids, (kid) => kid.type == "comment" && kid.parent == parentId && !kid.deleted && !kid.dead);
            return comments;
        }

        public async Task<List<NewsAsk>> GetAskStoriesAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/askstories.json");
            List<int> jobItems = ConvertTo<List<int>>(response);
            List<NewsAsk> asks = await GetNewsItems<NewsAsk>(jobItems, (item) => !item.deleted && !item.dead);
            return asks;
        }

        public async Task<List<NewsShow>> GetShowStoriesAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/showstories.json");
            List<int> jobItems = ConvertTo<List<int>>(response);
            List<NewsShow> shows = await GetNewsItems<NewsShow>(jobItems, (item) => !item.deleted && !item.dead);
            return shows;
        }

        public async Task<List<NewsPoll>> GetPollStoriesAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/maxitem.json");

            int current;
            List<NewsPoll> polls = new List<NewsPoll>();
            if ( int.TryParse(response, out current) )
            {
                bool more = true;
                while (more)
                {
                    List<int> items = new List<int>();
                    for (int bottom = current < 5000 ? 1 : current - 5000; current >= bottom; --current)
                    {
                        items.Add(current);
                    }
                    polls = new List<NewsPoll>();
                    if (items.Count > 0)
                        polls = await GetNewsItems<NewsPoll>(items, (item) => item.type == "poll" && !item.deleted && !item.dead);

                    if (polls.Count > 0 || items.Count == 0)
                        more = false;
                }
            }
            else
            {
                throw new HackerNewsException("Poll max item failure", HackerNewsErrorCode.MaxItemError);
            }

            return polls;
        }
        private async Task <List<T>> GetNewsItems<T>(List<int> itemIds, Func<T, bool> predicate) where T: NewsItem, new()
        {

            List<Task<T>> newsItemsTasks = new List<Task<T>>();
            List<Task> runningRequests = itemIds.Select(async item =>
            {
                await _concurrentRequests.WaitAsync();
                newsItemsTasks.Add(GetNewsItem<T>(item));
            }).ToList();

            List<T> newsItems = new List<T>();
            try
            {
                await Task.WhenAll(runningRequests);
                newsItems = (await Task.WhenAll(newsItemsTasks)).Where(predicate).ToList();
            }
            catch( Exception ex )

            {
                EventId GetItemErrorEvent = new EventId((int)HackerNewsErrorCode.GetItemError, HackerNewsErrorCode.GetItemError.ToString());
                newsItems = newsItemsTasks.Where(task => task.IsCompletedSuccessfully).Select(task => task.Result).Where(predicate).ToList();
                foreach( Task t in newsItemsTasks)
                {
                    if( t.IsFaulted)
                    {
                        _logger.LogError(GetItemErrorEvent, t.Exception, $"Error GetNewsItem");
                    }
                }
            }
            return newsItems;
        }

        private async Task<T> GetNewsItem<T>( int itemId) where T: new()
        {
            try
            {
                string response = await _httpClient.GetStringAsync($"v0/item/{itemId}.json");
                T item = ConvertTo<T>(response);
                return item;
            }
            finally
            {
                _concurrentRequests.Release();
            }
        }
        private T ConvertTo<T>(string jsonString )  where T : new() 
        {
            T item = string.IsNullOrEmpty(jsonString) ? new T() : JsonConvert.DeserializeObject<T>(jsonString);
            return item;
        }


    }
}
