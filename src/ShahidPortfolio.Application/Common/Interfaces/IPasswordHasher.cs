namespace ShahidPortfolio.Application.Common.Interfaces;

public interface IPasswordHasher
{
    void CreatePasswordHash(string password, out string hash, out string salt);
    bool VerifyPassword(string password, string storedHash, string storedSalt);
}
