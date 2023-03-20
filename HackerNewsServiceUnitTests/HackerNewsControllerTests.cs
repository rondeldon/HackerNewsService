using HackerNewsService.Controllers;
using HackerNewsService.Models;
using Moq;
using HNS = HackerNewsService.Services;
namespace HackerNewsServiceUnitTests
{
    public class HackerNewsControllerTests
    {
        List<NewsStory> GetNewsStories()
        {
            List<NewsStory> stories = new List<NewsStory>() { new NewsStory()
                                                                        {
                                                                            score = 1,
                                                                            title = "titel",
                                                                            url = "url",
                                                                            id = 1,
                                                                            time = 1234343,
                                                                            by = "by",
                                                                            deleted = true,
                                                                            dead = false,
                                                                            type = "story",
                                                                            kids = new List<int>() { 1, 2, 3, 4 }
                                                                        }
                                                            };

             return stories;
        }


        [Fact]
        public async void GetStoriesSuccess()
        {
            var hns = new Mock<HNS.IHackerNewsService>();
            hns.Setup(x => x.GetNewsStoriesAsync()).ReturnsAsync(GetNewsStories() );

            HackerNewsController hnc = new HackerNewsController(hns.Object);

            var stories = await hnc.GetStories();

            Assert.Equal(stories.First().id, GetNewsStories().First().id);
            Assert.Equal(stories.First().type, GetNewsStories().First().type);
            Assert.Equal(stories.First().deleted, GetNewsStories().First().deleted);
            Assert.Equal(stories.First().time, GetNewsStories().First().time);
            Assert.Equal(stories.First().kids.Count, GetNewsStories().First().kids.Count);
  
        }
    }
}