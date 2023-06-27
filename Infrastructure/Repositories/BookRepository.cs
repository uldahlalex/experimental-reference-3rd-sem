using Dapper;
using Infrastructure.DataModels;
using Npgsql;
using BookDiscoveryItem = Infrastructure.QueryModels.BookDiscoveryItem;
using BookFeedQueryModel = Infrastructure.QueryModels.BookFeedQueryModel;

namespace Infrastructure.Repositories;

public class BookRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public BookRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<BookFeedQueryModel> BooksForFeedQuery(int userId, string bookSearchTerm,string orderBy, int pageSize,
        int startAt)
    {
        var query = $@"
SELECT array_agg(DISTINCT authors.name) as authors, 
    books.title, 
    books.cover_img_url as coverimgurl, 
    books.book_id as bookid, 
    BOOL_OR(library.reading_list_items.user_id = @userId) as isonmyreadinglist
FROM library.books
LEFT JOIN library.author_wrote_book_items ON books.book_id = author_wrote_book_items.book_id
LEFT JOIN library.authors ON author_wrote_book_items.author_id = authors.author_id
LEFT JOIN library.reading_list_items ON books.book_id = reading_list_items.book_id
WHERE books.title LIKE @bookSearchTerm
OR public.levenshtein(LOWER(books.title), @bookSearchTerm) < 4
GROUP BY books.title, books.cover_img_url, books.book_id
ORDER BY {orderBy} LIMIT @pageSize OFFSET @startAt;";
        var parameters = new
        {
            userId,
            bookSearchTerm =  "%" + bookSearchTerm + "%",
            pageSize,
            startAt
        };
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<BookFeedQueryModel>(query, parameters);
        }
    }

    public int CountBooksByIdQuery(int bookId)
    {
        string query = "SELECT COUNT(*) FROM library.books WHERE books.book_id = @bookId";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.ExecuteScalar<int>(query, new { bookId });
        }
    }

    public Book InsertAndReturnBookQuery(string? title, string publisher, string coverImgUrl)
    {
        var query = @"
INSERT INTO library.books (title, publisher, cover_img_url) VALUES (@title, @publisher, @coverImgUrl) 
RETURNING book_id as bookId, title, publisher, cover_img_url as coverimgurl;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<Book>(query, new { title, publisher, coverImgUrl });
        }
    }

    public IEnumerable<Book> BooksOnUsersReadingListQuery(int userId)
    {
        var query = @"
SELECT b.title, b.book_id as bookid, b.publisher, b.cover_img_url as coverimgurl FROM library.books b
JOIN library.reading_list_items r ON b.book_id = r.book_id
WHERE r.user_id = @userId;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Book>(query, new { userId });
        }
    }

    public IEnumerable<BookDiscoveryItem> BooksNotOnUsersReadingListQuery(int userId)
    {
        var query = @"
SELECT books.title, books.book_id as bookid, books.cover_img_url as coverimgurl
FROM library.books
LEFT JOIN library.reading_list_items r on books.book_id = r.book_id AND r.user_id = @userId
WHERE r.book_id IS NULL LIMIT 10;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<BookDiscoveryItem>(query, new { userId });
        }
    }

    public IEnumerable<BookDiscoveryItem> RecentBooksQuery()
    {
        var query = @"
SELECT books.title, books.book_id as bookid, books.cover_img_url as coverimgurl
FROM library.books ORDER BY books.book_id DESC LIMIT 10;";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<BookDiscoveryItem>(query);
        }
    }


    public BookFeedQueryModel BookViewModelQuery(int userId, int bookId)
    {
        var query = @"
SELECT array_agg(DISTINCT authors.name) as authors, 
    books.title, 
    books.cover_img_url as coverimgurl, 
    books.book_id as bookid, 
    BOOL_OR(library.reading_list_items.user_id = @userId) as isonmyreadinglist
FROM library.books
LEFT JOIN library.author_wrote_book_items ON books.book_id = author_wrote_book_items.book_id
LEFT JOIN library.authors ON author_wrote_book_items.author_id = authors.author_id
LEFT JOIN library.reading_list_items ON books.book_id = reading_list_items.book_id
WHERE books.book_id = @bookId
GROUP BY books.title, books.cover_img_url, books.book_id;
";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.QueryFirst<BookFeedQueryModel>(query, new {userId, bookId});
        }
    }
}