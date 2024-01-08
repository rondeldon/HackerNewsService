using Castle.Core.Logging;
using HackerNewsService.Controllers;
using HackerNewsService.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using HNS = HackerNewsService.Services;
namespace HackerNewsServiceUnitTests
{
    public class HackerNewsServiceTests
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
                                                                            deleted = false,
                                                                            dead = false,
                                                                            type = "story",
                                                                            kids = new List<int>() { 1, 2, 3, 4 },
                                                                            descendants = 279,

                                                                        }
                                                            };

            return stories;
        }

        List<NewsStory> GetBadNewStories()
        {
            List<NewsStory> stories = new List<NewsStory>() { new NewsStory()
                                                                        {
                                                                            score = 1,
                                                                            title = "titel",
                                                                            url = "url",
                                                                            id = 1,
                                                                            time = 1234343,
                                                                            by = "by",
                                                                            deleted = false,
                                                                            dead = false,
                                                                            type = "bad",
                                                                            kids = new List<int>() { 1, 2, 3, 4 },
                                                                            descendants = 279,

                                                                        }
                                                            };

            return stories;
        }


        [Fact]
        public async void GetStoriesSuccess()
        {
            var logger = new Mock<ILogger<HNS.HackerNewsService>>();
            var httpClient = GetHttpClient(CreateOkResponseMessages(GetNewsStories().First().kids, GetNewsStories().First(), GetNewsStories().First(), GetNewsStories().First(), GetNewsStories().First()));
            HNS.HackerNewsService hns = new HNS.HackerNewsService(httpClient, logger.Object);

            var resp = await hns.GetNewStoriesAsync();

            Assert.Equal(4, resp.Count);
            Assert.Equal(1, resp.First().id);

        }

        [Fact]
        public async void ExcludeNonUserSuccess()
        {
            var logger = new Mock<ILogger<HNS.HackerNewsService>>();
            var httpClient = GetHttpClient(CreateOkResponseMessages(GetNewsStories().First().kids, GetBadNewStories().First(), GetNewsStories().First(), GetBadNewStories().First(), GetNewsStories().First()));
            HNS.HackerNewsService hns = new HNS.HackerNewsService(httpClient, logger.Object);

            var resp = await hns.GetNewStoriesAsync();

            Assert.Equal(2, resp.Count);
            Assert.Equal(1, resp.First().id);

        }

        private static HttpClient GetHttpClient(IEnumerable<HttpResponseMessage> httpResponseMessagesInOrder)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var handlerPart = handlerMock.Protected()
                                        .SetupSequence<Task<HttpResponseMessage>>(
                                          "SendAsync",
                                          ItExpr.IsAny<HttpRequestMessage>(),
                                          ItExpr.IsAny<CancellationToken>()
                                          );

            foreach (var message in httpResponseMessagesInOrder)
            {
                handlerPart = AddHttpResponses(handlerPart, message);
            }

            handlerMock.Verify();

            HttpClient httpClient = new HttpClient(handlerMock.Object);
            httpClient.BaseAddress = new Uri("https://somedomain.com");
            return httpClient;
        }
        private static IEnumerable<HttpResponseMessage> CreateOkResponseMessages(params object[] objects)
        {
            foreach (var obj in objects)
            {
                yield return CreateHttpResponseMessage(obj, HttpStatusCode.OK);
            }
        }

        private static HttpResponseMessage CreateHttpResponseMessage(object obj, HttpStatusCode httpStatusCode)
        {
            var data = new StringContent(System.Text.Json.JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

            return new HttpResponseMessage
            {
                Content = data,
                StatusCode = httpStatusCode
            };
        }

        private static Moq.Language.ISetupSequentialResult<Task<HttpResponseMessage>> AddHttpResponses(Moq.Language.ISetupSequentialResult<Task<HttpResponseMessage>> handlerPart, HttpResponseMessage message)
        {
            return handlerPart.ReturnsAsync(message);
        }
    }
}