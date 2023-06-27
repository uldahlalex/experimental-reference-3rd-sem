namespace Infrastructure.QueryModels;

public class BookFeedQueryModel
{
    public string? Title { get; set; }
    public int BookId { get; set; }
    public bool IsOnMyReadingList { get; set; }
    public IEnumerable<string>? Authors { get; set; }
    public string? CoverImgUrl { get; set; }
}