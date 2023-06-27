namespace Infrastructure.DataModels;

public class EndUser
{
    public int EndUserId { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Salt { get; set; }
    public string? Role { get; set; }
    public string? Status { get; set; }
    public int PravatarId { get; set; }
}