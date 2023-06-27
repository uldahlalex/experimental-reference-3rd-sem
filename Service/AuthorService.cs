using Infrastructure.DataModels;
using Infrastructure.Repositories;
namespace Service;

public class AuthorService
{
    private readonly AuthorRepository _authorRepository;

    public AuthorService(AuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }


    public IEnumerable<Author> GetAuthorsForFeed(string authorSearchTerm, string orderBy, int pageSize, int startAt)
    {
        return _authorRepository.AuthorsForFeedQuery(authorSearchTerm, orderBy, pageSize, startAt);
    }
}