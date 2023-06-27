
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace api.TransferObjects;

public class RequestAuthorSearchOptionsDto
{
    public string? AuthorSearchTerm { get; set; }

    [NotNull]
    [ValidationAttributes.ValueIsOneOf(new[] { "author.name"}, "Ordering must be by author name")]
    public string? OrderBy { get; set; }

    [Range(1,50)]
    public int PageSize { get; set; }

    [Range(0,Int32.MaxValue)]
    public int StartAt { get; set; }
}