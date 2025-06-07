using System.Diagnostics;

namespace Jakub_Zajega_14987.WebAPI.Middleware;

public class TimingMiddleware : IMiddleware
{
    private readonly ILogger<TimingMiddleware> _logger;

    public TimingMiddleware(ILogger<TimingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        context.Response.OnStarting(() =>
        {
            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
            context.Response.Headers.Append("X-Processing-Time-Milliseconds", elapsedMilliseconds.ToString());

            _logger.LogInformation("Request [{method}] at {path} took {time} ms",
                context.Request.Method,
                context.Request.Path,
                elapsedMilliseconds);

            return Task.CompletedTask;
        });

        await next(context);
    }
}