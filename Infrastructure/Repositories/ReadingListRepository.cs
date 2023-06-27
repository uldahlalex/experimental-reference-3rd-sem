using Dapper;
using Npgsql;

namespace Infrastructure.Repositories;

public class ReadingListRepository
{
    private NpgsqlDataSource _dataSource;

    public ReadingListRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public bool AddToReadingList(int userId, int bookId)
    {
        var query = "INSERT INTO library.reading_list_items (user_id, book_id) VALUES (@userId, @bookId)";
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(query, new { userId, bookId }) == 1;
        }
    }

    public bool IsThisBookOnUsersReadingList(int userId, int bookId)
    {
        var query = "SELECT COUNT(*) FROM library.reading_list_items WHERE user_id = @userId AND book_id = @bookId;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.ExecuteScalar<int>(query, new { userId, bookId }) == 1;
        }
    }

    public bool RemoveFromReadingList(int userId, int bookId)
    {
        var query = "DELETE FROM library.reading_list_items WHERE user_id = @userId AND book_id = @bookId;";

        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Execute(query, new { userId, bookId }) == 1;
        }
    }
}