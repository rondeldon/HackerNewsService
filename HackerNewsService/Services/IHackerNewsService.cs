using HackerNewsService.Models;

namespace HackerNewsService.Services
{
    public interface IHackerNewsService
    {
        Task<List<NewsStory>> GetNewsStoriesAsync();
        Task<List<NewsJob>> GetNewsJobsAsync();
        Task<List<NewsPoll>> GetNewsPollsAsync();
        Task<List<NewsItemComment>> GetNewsItemCommentsAsync(int parentId);
    }
}