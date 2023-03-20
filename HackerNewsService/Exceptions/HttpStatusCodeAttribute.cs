namespace HackerNewsService.Exceptions
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HttpStatusCodeAttribute : Attribute
    {
        public int StatusCode { get; }

        public HttpStatusCodeAttribute(int statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
