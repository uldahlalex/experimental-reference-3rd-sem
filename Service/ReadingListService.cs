using System.ComponentModel.DataAnnotations;
using Infrastructure.Repositories;

namespace Service;

public class ReadingListService
{
    private readonly ReadingListRepository _readingListRepository;
    private readonly BookRepository _bookRepository;

    public ReadingListService(ReadingListRepository readingListRepository,
        BookRepository bookRepository)
    {
        _bookRepository = bookRepository;
        _readingListRepository = readingListRepository;
    }

    public void AddToReadingList(int userId, int bookId)
    {
        //does book exist
        if (_bookRepository.CountBooksByIdQuery(bookId) == 0)
            throw new KeyNotFoundException("Could not find book");

        //is the book already on my reading list
        if (_readingListRepository.IsThisBookOnUsersReadingList(userId,bookId))
            throw new ValidationException("Book is already on your reading list");

        //if successfully added, dapper turns the number of affected rows, which should be 1
        var insertion = _readingListRepository.AddToReadingList(userId, bookId);
        if (!insertion) throw new Exception("Failed to add to reading list");
    }
    

    public void RemoveFromReadingList(int userId, int bookId)
    {
        //does book exist
        if (_bookRepository.CountBooksByIdQuery(bookId) == 0)
            throw new KeyNotFoundException("Could not find book");

        //is the book on my reading list
        if (!_readingListRepository.IsThisBookOnUsersReadingList(userId, bookId))
            throw new ValidationException("Book is not on your reading list");

        //if successfully added, dapper turns the number of affected rows, which should be 1
        var deletion = _readingListRepository.RemoveFromReadingList(userId, bookId);
        if (!deletion) throw new Exception("Failed to remove from reading list");
    }
}