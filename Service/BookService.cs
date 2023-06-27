using Infrastructure.DataModels;
using Infrastructure.QueryModels;
using Infrastructure.Repositories;

namespace Service;

public class BookService
{
    private readonly BookRepository _bookRepository;
    public BookService(BookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public IEnumerable<BookFeedQueryModel> GetBooksForFeed(int userId, string bookSearchTerm, string orderBy, int pageSize, int startAt)
    {
        return _bookRepository.BooksForFeedQuery(userId, bookSearchTerm, orderBy, pageSize, startAt);
    }

    public Book CreateBook(string? title, string publisher, string coverImgUrl)
    {
        return _bookRepository.InsertAndReturnBookQuery(title, publisher, coverImgUrl);
    }

    public BookFeedQueryModel GetBookByIdForDescriptionPage(int userId, int id)
    {
        return _bookRepository.BookViewModelQuery(userId, id);
    }

    public BookDiscoveryQueryModel GetBooksForDiscover(int userId)
    {
        return new BookDiscoveryQueryModel()
        {
            RecentlyAdded = _bookRepository.BooksNotOnUsersReadingListQuery(userId),
            NotOnReadingList = _bookRepository.RecentBooksQuery()
        };
    }

    public IEnumerable<Book> GetAllBooksInReadingListForUser(int userId)
    {
        return _bookRepository.BooksOnUsersReadingListQuery(userId);
    }
}