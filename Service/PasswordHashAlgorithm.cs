using System.Security.Cryptography;

namespace Service;

public class PasswordHashAlgorithm
{
    public string HashPassword(string password, string salt)
    {
        return BCrypt.Net.BCrypt.HashPassword(password + salt);
    }

    public bool VerifyHashedPassword(string password, string salt, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password + salt, hash);
    }

    public string GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(32).ToString()!;
    }
}