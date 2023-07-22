using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Service;

public class PasswordHashAlgorithm
{
    private readonly byte[] _pepper;

    public PasswordHashAlgorithm()
    {
        var pepper = Environment.GetEnvironmentVariable("pepper");
        if (pepper == null) throw new InvalidOperationException("Missing \'pepper\' in environment variables!");
        _pepper = Encoding.UTF8.GetBytes(pepper);
    }

    public string GenerateSalt()
    {
        return Encode(RandomNumberGenerator.GetBytes(128));
    }

    public string HashPassword(string password, string salt)
    {
        using var hashAlgo = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = Decode(salt),
            MemorySize = 12288,
            Iterations = 3,
            DegreeOfParallelism = 1,
            KnownSecret = _pepper
        };
        return Encode(hashAlgo.GetBytes(256));
    }

    public bool VerifyHashedPassword(string password, string salt, string hash)
    {
        return HashPassword(password, salt).SequenceEqual(hash);
    }

    private byte[] Decode(string value) => Convert.FromBase64String(value);
    private string Encode(byte[] value) => Convert.ToBase64String(value);
}