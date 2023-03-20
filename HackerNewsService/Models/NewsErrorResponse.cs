namespace HackerNewsService.Models
{
    public class NewsErrorResponse
    {
        public NewsErrorResponse( string errorMessage, int errorCode) 
        { 
           ErrorMessage = errorMessage;
           ErrorCode = errorCode;
        }
       public string ErrorMessage { get; }
       public int ErrorCode{ get; }
         
    }
}
