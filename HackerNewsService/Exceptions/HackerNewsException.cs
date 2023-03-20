using System;

namespace HackerNewsService.Exceptions
{
    public class HackerNewsException : Exception
    {
        public readonly HackerNewsErrorCode errorCode;
        public HackerNewsException(string message, HackerNewsErrorCode hackerNewsErrorCode ) : this(message, hackerNewsErrorCode, null)
        {
        }

        public HackerNewsException(string message, HackerNewsErrorCode hackerNewsErrorCode, Exception inner) : base(message, inner)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(message, "message");
            errorCode = hackerNewsErrorCode;
        }
    }
}

