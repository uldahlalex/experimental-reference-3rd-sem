using api.ActionFilters;
using api.Helpers;
using api.TransferObjects;
using Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace api.Endpoints;

public class BookEndpoints : ControllerBase
{
    private readonly BookService _bookService;
    private readonly ResponseHelper _response;

    public BookEndpoints(
        BookService bookService,
        ResponseHelper response)
    {
        _response = response;
        _bookService = bookService;
    }


    [HttpGet]
    [Route("/api/books")]
    [ValidateModelFilter]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public ResponseDto GetBooks([FromQuery] RequestBookSearchOptionsDto dto)
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;
        return _response.Success(HttpContext, 200, "Success",
            _bookService.GetBooksForFeed(endUser.EndUserId, dto.BookSearchTerm, dto.OrderBy, dto.PageSize, dto.StartAt));
    }

    [HttpPost]
    [Route("/api/books")]
    [ValidateModelFilter]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public ResponseDto CreateBook([FromBody] RequestBookCreationDto bookDto)
    {
        return _response.Success(HttpContext, 201, "Successful insertion",
            _bookService.CreateBook(bookDto.Title, bookDto.Publisher, bookDto.CoverImgUrl));
    }

    [HttpGet]
    [Route("/api/books/{id}")]
    [ValidateModelFilter]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public ResponseDto GetBookById(
        [FromRoute] int id)
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;
        return _response.Success(HttpContext, 200, "Book found", _bookService.GetBookByIdForDescriptionPage(endUser.EndUserId, id));
    }

    [HttpGet]
    [Route("/api/discover")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public ResponseDto GetBooksForDiscoverFeed()
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;

        return _response.Success(HttpContext, 200, "Successful read",
            _bookService.GetBooksForDiscover(endUser.EndUserId));
    }
}

