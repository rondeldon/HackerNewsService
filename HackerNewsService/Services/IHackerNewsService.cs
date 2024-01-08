using HackerNewsService.Models;

namespace HackerNewsService.Services
{
    public interface IHackerNewsService
    {
        Task<List<NewsStory>> GetNewStoriesAsync();
        Task<List<NewsStory>> GetTopStoriesAsync();
        Task<List<NewsStory>> GetBestStoriesAsync();
        Task<List<NewsJob>> GetJobStoriesAsync();
        Task<List<NewsPoll>> GetPollStoriesAsync();
        Task<List<NewsAsk>> GetAskStoriesAsync();
        Task<List<NewsShow>> GetShowStoriesAsync();
        Task<List<NewsItemComment>> GetNewsItemCommentsAsync(int parentId);
    }
}