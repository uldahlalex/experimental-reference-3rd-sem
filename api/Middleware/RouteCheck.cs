namespace api.Middleware;

public class RouteCheck
{
    private readonly RequestDelegate _next;

    public RouteCheck(RequestDelegate next)
    {
        _next = next;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
        {
            context.Response.Clear();
            context.Response.StatusCode = 404;
            await context.Response.WriteAsJsonAsync(new
            {
                messageToClient =
                    "It was not possible to process this request. Are you sure the targeted route + request type is correct?"
            });
        }
    }

    // For all other cases, allow the request to proceed
}