
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace api.TransferObjects;

public class RequestBookSearchOptionsDto
{
    public string? BookSearchTerm { get; set; }
    
    [NotNull]
    [ValidationAttributes.ValueIsOneOf(new[] { "books.title", "books.id" }, "Ordering must be by title or ID")]
    public string? OrderBy { get; set; }

    [Range(1,50)]
    public int PageSize { get; set; }

    [Range(0,Int32.MaxValue)]
    public int StartAt { get; set; }
}