using Dapper;
using Infrastructure.DataModels;
using Npgsql;

namespace Infrastructure.Repositories;

public class AuthorRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public AuthorRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public IEnumerable<Author> AuthorsForFeedQuery(string authorSearchTerm, string orderBy, int pageSize, int startAt)
    {
        string baseQuery = $@"
SELECT author_id as authorid, name, birthday, nationality 
FROM library.authors
WHERE authors.name LIKE @authorSearchTerm
OR public.levenshtein(LOWER(authors.name), @authorSearchTerm) < 4 
ORDER BY {orderBy} LIMIT @pageSize OFFSET @startAt;";
        var parameters = new
        {
            authorSearchTerm = "%" + authorSearchTerm + "%",
            pageSize,
            startAt
        };
        using (var conn = _dataSource.OpenConnection())
        {
            return conn.Query<Author>(baseQuery, parameters);
        }
    }
}