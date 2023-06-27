using System.Security.Authentication;
using Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Service;

namespace api.ActionFilters;

public class AuthenticationFilter : IActionFilter
{
    private readonly AuthenticationService _authentication;

    public AuthenticationFilter(AuthenticationService authentication)
    {
        _authentication = authentication;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var authHeaders = context.HttpContext.Request.Headers.Authorization;
       if (authHeaders.IsNullOrEmpty() || authHeaders[0].IsNullOrEmpty())
        {
            throw new AuthenticationException("No user authentication present");
        }
        EndUser endUser = _authentication.VerifyTokenAndReturnUserIfNotBanned(authHeaders);
        context.HttpContext.Items["user"] = endUser;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}