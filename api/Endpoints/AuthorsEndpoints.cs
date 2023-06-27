using api.ActionFilters;
using api.Helpers;
using api.TransferObjects;
using Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace api.Endpoints;

public class AuthorsEndpoints : ControllerBase
{
    private readonly AuthorService _authorService;
    private readonly ResponseHelper _response;


    public AuthorsEndpoints(AuthorService authorService,
        ResponseHelper response)
    {
        _response = response;
        _authorService = authorService;
    }


    [HttpGet]
    [Route("/api/authors")]
    [ValidateModelFilter]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public ResponseDto GetAuthors([FromQuery] RequestAuthorSearchOptionsDto dto)
    {
        IEnumerable<Author> authors =
            _authorService.GetAuthorsForFeed(dto.AuthorSearchTerm, dto.OrderBy, dto.PageSize, dto.StartAt);
        return _response.Success(HttpContext, 200, "Successfully fetched authors", authors);
    }
}