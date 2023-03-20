namespace HackerNewsService.Exceptions
{
    public enum HackerNewsErrorCode
    {
        [HttpStatusCode(502)]
        GetItemError = 1000,

        [HttpStatusCode(502)]
        MaxItemError = 1001,
    }
}
