using api.ActionFilters;
using api.Helpers;
using api.TransferObjects;
using Infrastructure.DataModels;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace api.Endpoints;

public class ReadingListEndpoints : ControllerBase
{
    private readonly ReadingListService _readingListService;
    private readonly ResponseHelper _response;
    private readonly BookService _bookService;

    public ReadingListEndpoints(
        ResponseHelper response,
        ReadingListService readingListService,
        BookService bookService)
    {
        _response = response;
        _bookService = bookService;
        _readingListService = readingListService;
    }

    [HttpPost]
    [Route("/api/addToMyReadingList/book/{bookId}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    public ResponseDto AddBookToMyReadingList([FromRoute] int bookId)
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;
        _readingListService.AddToReadingList(endUser.EndUserId, bookId);
        return _response.Success(HttpContext, 201, "Added to reading list");
    }


    [HttpDelete]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [Route("/api/removeFromMyReadingList/book/{bookId}")]
    public ResponseDto RemoveFromMyReadingList([FromRoute] int bookId)
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;
        _readingListService.RemoveFromReadingList(endUser.EndUserId, bookId);
        return _response.Success(HttpContext, 201, "Removed to reading list");
    }

    [HttpGet]
    [ServiceFilter(typeof(AuthenticationFilter))]
    [Route("/api/myReadingList")]
    public ResponseDto GetMyReadingList()
    {
        EndUser endUser = (HttpContext.Items["user"] as EndUser)!;
        var queryResult = _bookService.GetAllBooksInReadingListForUser(endUser.EndUserId);
        return _response.Success(HttpContext, 200, "Success", queryResult);
    }
}