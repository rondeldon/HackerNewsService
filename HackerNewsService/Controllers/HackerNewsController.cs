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
        /// Returns the Newest Hacker News Stories
        /// </summary>
        /// <returns>A list of the latest news stories.</returns>
        [HttpGet("newstories")]
        [ProducesResponseType(typeof(IEnumerable<NewsStory>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NewsErrorResponse), (int)HttpStatusCode.BadGateway)]
        public async Task<IEnumerable<NewsStory>> GetNewStories()
        {
            List<NewsStory> newsStories = await _hackerNewsService.GetNewStoriesAsync();
            return newsStories;
        }

        /// <summary>
        /// Returns the Top Hacker News Stories
        /// </summary>
        /// <returns>A list of the latest news stories.</returns>
        [HttpGet("topstories")]
        [ProducesResponseType(typeof(IEnumerable<NewsStory>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(NewsErrorResponse), (int)HttpStatusCode.BadGateway)]
        public async Task<IEnumerable<NewsStory>> GetTopStories()
        {
            List<NewsStory> newsStories = await _hackerNewsService.GetTopStoriesAsync();
            return newsStories;
        }

        /// <summary>
        /// Returns the Best Hacker News Stories
        /// </summary>
        /// <returns>A list of the latest polls if any are found.</returns>
        [HttpGet("beststories")]
        public async Task<IEnumerable<NewsStory>> GetBestStories()
        {
            List<NewsStory> bestStories = await _hackerNewsService.GetBestStoriesAsync();
            return bestStories;
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
            List<NewsJob> jobStories = await _hackerNewsService.GetJobStoriesAsync();
            return jobStories;
        }

        //[HttpGet("polls")]
        //public async Task<IEnumerable<NewsPoll>> GetPolls()
        //{
        //    List<NewsPoll> pollStories = await _hackerNewsService.GetPollStoriesAsync();
        //    return pollStories;
        //}


        /// <summary>
        /// Returns the latest Hacker News Asks
        /// </summary>
        /// <returns>A list of the latest polls if any are found.</returns>
        [HttpGet("asks")]
        public async Task<IEnumerable<NewsAsk>> GetAsks()
        {
            List<NewsAsk> askStories = await _hackerNewsService.GetAskStoriesAsync();
            return askStories;
        }

        /// <summary>
        /// Returns the latest Hacker News Shows
        /// </summary>
        /// <returns>A list of the latest polls if any are found.</returns>
        [HttpGet("shows")]
        public async Task<IEnumerable<NewsShow>> GetShows()
        {
            List<NewsShow> showStories = await _hackerNewsService.GetShowStoriesAsync();
            return showStories;
        }

        /// <summary>
        /// Returns comments for an item(i.e. Stories, jobs, Polls )
        /// </summary>
        /// <param name="parentId"></param>
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
