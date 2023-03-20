using HackerNewsService.Models;
using HackerNewsService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HackerNewsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService; 

        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }

        /// <summary>
        /// Returns the latest Hacker News Stories
        /// </summary>
        /// <returns>A list of the latest news stories.</returns>
        [HttpGet("stories")]
        [ProducesResponseType(typeof(IEnumerable<NewsStory>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NewsErrorResponse), (int)HttpStatusCode.BadGateway)]
        public async Task<IEnumerable<NewsStory>> GetStories()
        {
            List<NewsStory> newsStories = await _hackerNewsService.GetNewsStoriesAsync();
            return newsStories;
        }
        /// <summary>
        /// Returns the latest Hacker Job Stories
        /// </summary>
        /// <returns>A list of the latest job stories.</returns>
        [HttpGet("jobs")]
        [ProducesResponseType(typeof(IEnumerable<NewsJob>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NewsErrorResponse), (int)HttpStatusCode.BadGateway)]
        public async Task<IEnumerable<NewsJob>> GetJobs()
        {
            List<NewsJob> newsJobs = await _hackerNewsService.GetNewsJobsAsync();
            return newsJobs;
        }

        /// <summary>
        /// Returns the latest Hacker News Polls
        /// </summary>
        /// <returns>A list of the latest polls if any are found.</returns>
        [HttpGet("polls")]
        public async Task<IEnumerable<NewsPoll>> GetPolls()
        {
            List<NewsPoll> newsPolls = await _hackerNewsService.GetNewsPollsAsync();
            return newsPolls;
        }

        /// <summary>
        /// Returns comments for an item(i.e. Stories, jobs, Polls )
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A list of comments for a news item.</returns>
        // GET api/<HackerNewsController>/5/comments
        [HttpGet("{parentId}/comments")]
        [ProducesResponseType(typeof(NewsErrorResponse), (int)HttpStatusCode.BadGateway)]
        public async Task<IEnumerable<NewsItemComment>> GetNewsItemComments(int parentId)
        {
            List<NewsItemComment> newsItemComments = await _hackerNewsService.GetNewsItemCommentsAsync(parentId);
            return newsItemComments;
        }
    }
}
