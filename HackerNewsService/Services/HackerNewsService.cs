using HackerNewsService.Exceptions;
using HackerNewsService.Models;
using Newtonsoft.Json;

namespace HackerNewsService.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HackerNewsService> _logger;
        private  SemaphoreSlim _concurrentRequests = new SemaphoreSlim(50);
        public HackerNewsService(HttpClient httpClient, ILogger<HackerNewsService> logger) 
        {
            ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
            ArgumentNullException.ThrowIfNull( logger, nameof(logger));

           _httpClient = httpClient;
            _logger = logger;
        }
        public async Task<List<NewsStory>> GetNewsStoriesAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/topstories.json");
            List<int> storyItems = ConvertTo<List<int>>(response);
            List<NewsStory> stories = await GetNewsItems<NewsStory>(storyItems, (item) => item.type == "story" && !item.deleted && !item.dead);
            return stories;
        }

        public async Task<List<NewsJob>> GetNewsJobsAsync()
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

        public async Task<List<NewsPoll>> GetNewsPollsAsync()
        {
            string response = await _httpClient.GetStringAsync("v0/maxitem.json");

            int maxId;
            List<NewsPoll> polls;
            if ( int.TryParse(response, out maxId) )
            {
                List<int> items = new List<int>();
                for (int x = maxId < 1000 ? maxId : maxId - 1000; x <= maxId; ++x )
                {
                    items.Add(x);
                }
                polls = await GetNewsItems<NewsPoll>(items, (item) => item.type == "poll" && !item.deleted && !item.dead);
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
                await _concurrentRequests.WaitAsync().ConfigureAwait(false);
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
