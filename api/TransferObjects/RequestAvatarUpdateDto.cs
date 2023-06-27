using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Serialization;


namespace api.TransferObjects;

public class AvatarRequestDto
{
    [Range(1, 101, ErrorMessage = "Must be between 1 and 100, buddy")]
    public int PravatarId { get; set; }
}

