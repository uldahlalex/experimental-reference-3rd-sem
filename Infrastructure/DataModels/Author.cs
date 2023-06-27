namespace Infrastructure.DataModels;

public class Author
{
    public int AuthorId { get; set; }
    public string? Name { get; set; }
    public DateTime Birthday { get; set; }
    public string? Nationality { get; set; }
}