using System.Security.Cryptography;
using ShahidPortfolio.Application.Common.Interfaces;

namespace ShahidPortfolio.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    void IPasswordHasher.CreatePasswordHash(string password, out string hash, out string salt)
    {
        CreatePasswordHash(password, out hash, out salt);
    }

    bool IPasswordHasher.VerifyPassword(string password, string storedHash, string storedSalt)
    {
        return VerifyPassword(password, storedHash, storedSalt);
    }

    public static void CreatePasswordHash(string password, out string hash, out string salt)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, Algorithm, HashSize);

        hash = Convert.ToBase64String(hashBytes);
        salt = Convert.ToBase64String(saltBytes);
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);
        byte[] expectedHashBytes = Convert.FromBase64String(storedHash);

        byte[] actualHashBytes = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(actualHashBytes, expectedHashBytes);
    }
}
