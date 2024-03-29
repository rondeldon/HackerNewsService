﻿using HackerNewsService.Models;
using HackerNewsService.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace HackerNewsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNewsService _hackerNewsService; 
       /// <summary>
       /// The HackerNews Service powered by Brown
       /// </summary>
       /// <param name="hackerNewsService"></param>
        public HackerNewsController(IHackerNewsService hackerNewsService)
        {
            _hackerNewsService = hackerNewsService;
        }
        /// <summary>
        /// Returns the Newest Hacker News Stories
        /// </summary>
        /// <returns>A list of the latest news stories.</returns>
        /// <remarks>
        /// Sample request:
        ///     N/A
        /// </remarks>
        [HttpGet("newstories")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(NewsStory))]
        [SwaggerResponse((int)HttpStatusCode.BadGateway, "Unable to communicate with downstream service.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unexpected error.")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(NewsStory))]
        [SwaggerResponse((int)HttpStatusCode.BadGateway, "Unable to communicate with downstream service.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unexpected error.")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(NewsStory))]
        [SwaggerResponse((int)HttpStatusCode.BadGateway, "Unable to communicate with downstream service.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unexpected error.")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(NewsJob))]
        [SwaggerResponse((int)HttpStatusCode.BadGateway, "Unable to communicate with downstream service.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unexpected error.")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(NewsAsk))]
        [SwaggerResponse((int)HttpStatusCode.BadGateway, "Unable to communicate with downstream service.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unexpected error.")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(NewsShow))]
        [SwaggerResponse((int)HttpStatusCode.BadGateway, "Unable to communicate with downstream service.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unexpected error.")]
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
        [SwaggerResponse((int)HttpStatusCode.OK, "Success", typeof(NewsItemComment))]
        [SwaggerResponse((int)HttpStatusCode.BadGateway, "Unable to communicate with downstream service.")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Unexpected error.")]
        public async Task<IEnumerable<NewsItemComment>> GetNewsItemComments(int parentId)
        {
            List<NewsItemComment> newsItemComments = await _hackerNewsService.GetNewsItemCommentsAsync(parentId);
            return newsItemComments;
        }
    }
}
