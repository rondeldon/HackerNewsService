using Polly;
using Polly.Extensions.Http;
using HackerNewsService.Services;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Runtime.Serialization;
using System;
using HackerNewsService.Exceptions;
using System.Reflection;
using Newtonsoft.Json.Linq;
using HackerNewsService.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*");
                      });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "HackerNews Service",
        Description = "A high performing service to deliver up HackerNews items.",
        Contact = new OpenApiContact
        {
            Name = "Developer",
            Url = new Uri("https://github.com/rondeldon/")
        }
    });
});
string baseUrl = builder.Configuration.GetValue("BaseUrl", "https://hacker-news.firebaseio.com");
builder.Services.AddHttpClient<IHackerNewsService, HackerNewsService.Services.HackerNewsService>(client =>
                {
                    client.BaseAddress = new Uri(baseUrl);
                })
               .AddPolicyHandler(GetRetryPolicy());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);

//Global exception handler
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception?.Error is HackerNewsException)
        {
            HackerNewsException  hackerNewsException = (HackerNewsException) exception.Error;
            context.Response.StatusCode = GetHttpStatusCode(hackerNewsException.errorCode);
            NewsErrorResponse errorResponse = new NewsErrorResponse(hackerNewsException.Message, (int)hackerNewsException.errorCode);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
        else
        {
            context.Response.StatusCode = 500;
            NewsErrorResponse errorResponse = new NewsErrorResponse("Unexpected error.", 0000);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        }
    });
});


app.Run();
IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError()
                    .WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}

int GetHttpStatusCode( HackerNewsErrorCode errorCode)
{
    Type enumType = errorCode.GetType();
    string? name = Enum.GetName(enumType, errorCode);
    if( name == null)
        return (int)HttpStatusCode.InternalServerError;

    var httpStatusAttribute = enumType?.GetField(name)?.GetCustomAttributes(false).OfType<HttpStatusCodeAttribute>().SingleOrDefault();
    if( httpStatusAttribute == null )
        return (int)HttpStatusCode.InternalServerError;

    return httpStatusAttribute.StatusCode;
}
