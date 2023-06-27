namespace Infrastructure.QueryModels;

public class BookDiscoveryQueryModel
{
    public IEnumerable<BookDiscoveryItem>? NotOnReadingList { get; set; }
    public IEnumerable<BookDiscoveryItem>? RecentlyAdded { get; set; }
}

public class BookDiscoveryItem
{
    public string? Title { get; set; }
    public int BookId { get; set; }
    public string? CoverImgUrl { get; set; }
}