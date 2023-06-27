using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.DataModels;
using Infrastructure.Repositories;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Service;

public class AuthenticationService
{
    private readonly UserRepository _userRepository;

    private static readonly byte[] Secret = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("dotnetsecret")!);

    public AuthenticationService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public void PrintWarningIfSecretIsNotValid()
    {
        if (Secret.Length < 64)
        {
            Console.WriteLine("\nWARNING: SECRET IS SHORTER THAN 64 CHARACTERS." +
                              "\nSECOND ARGUMENT PASSED IN DOTNET RUN SHOULD BE LONG RANDOM SECRET\n");
        }
    }


    public string IssueToken(List<KeyValuePair<string, object>> payloadItems)
    {
        var claimsList = new List<Claim>();
        foreach (KeyValuePair<string, object> o in payloadItems)
        {
            claimsList.Add(new Claim(o.Key, o.Value.ToString() ?? string.Empty));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claimsList),
            Expires = DateTime.UtcNow.AddYears(1),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(Secret), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
        catch (Exception e)
        {
            throw new Exception("Failed to create token", e.InnerException);
        }
        
    }


    private JwtSecurityToken ValidateAndReturnToken(StringValues authHeader)
    {
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Secret),
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            new JwtSecurityTokenHandler().ValidateToken(authHeader[0],
                validationParameters,
                out SecurityToken token);
            return (JwtSecurityToken)token;
        }
        catch (Exception e)
        {
            throw new AuthenticationException("Failed to validate user identity from token", e.InnerException);
        }
        
    }


    private EndUser IsUserInDatabaseAndNotBanned(JwtSecurityToken decodedToken)
    {
        var userIdClaim = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "endUserId");
        if (ReferenceEquals(userIdClaim, null))
        {
            throw new AuthenticationException("No User ID in JWT payload");
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            throw new FormatException("Cannot parse user ID as integer");
        }

        try
        {
            return _userRepository.GetUserById(userId);
        }
        catch (Exception e)
        {
            throw new Exception("User could not be found", e);
        }
        
    }

    public List<KeyValuePair<string, object>> DefaultKeyValuesForJwtPayload(EndUser endUser)
    {
        return new List<KeyValuePair<string, object>>
        {
            new("endUserId", endUser.EndUserId),
            new("role", endUser.Role ?? string.Empty),
            new("status", endUser.Status ?? string.Empty),
            new("email", endUser.Email!),
            new("pravatarId", endUser.PravatarId)
        };
    }

    public EndUser VerifyTokenAndReturnUserIfNotBanned(StringValues authHeader)
    {
        return IsUserInDatabaseAndNotBanned(ValidateAndReturnToken(authHeader));
    }

    public EndUser LogIn(string email, string password)
    {
        EndUser endUser = _userRepository.GetUserByEmailOrNull(email);
        if (ReferenceEquals(endUser, null)) throw new KeyNotFoundException("User does not exist");

        if (!BCrypt.Net.BCrypt.Verify(password + endUser.Salt, endUser.PasswordHash))
        {
            throw new InvalidCredentialException("Wrong email and password");
        }

        return endUser;
    }

    public EndUser Register(string email, string password, string role = "user")
    {
        if (_userRepository.IsThisEmailTaken(email))
        {
            throw new ValidationException("Email is already taken");
        }

        string salt = RandomNumberGenerator.GetBytes(32).ToString()!;
        string hash = BCrypt.Net.BCrypt.HashPassword(password + salt);

        return _userRepository.CreateAndReturnUser(email, salt, hash, role);
    }

    public void DeleteUserById(int userId)
    {
        var result = _userRepository.DeleteUserById(userId);
        if (!result) throw new Exception("Could not delete user");
    }

    public void UpdateAvatarForUser(int userId, int prvatarId)
    {
        var result = _userRepository.UpdateUsersAvatar(userId, prvatarId);
        if (!result) throw new Exception("Could not update avatar");
    }
}