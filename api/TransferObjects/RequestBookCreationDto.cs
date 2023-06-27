using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace api.TransferObjects;

public class RequestBookCreationDto
{
    [NotNull]
    public string? Title { get; set; }
    [NotNull]
    public string? Publisher { get; set; }
    [NotNull]
    [Url]
    public string? CoverImgUrl { get; set; }
}