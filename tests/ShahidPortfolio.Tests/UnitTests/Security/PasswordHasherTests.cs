using FluentAssertions;
using ShahidPortfolio.Infrastructure.Security;
using Xunit;

namespace ShahidPortfolio.Tests.UnitTests.Security;

public class PasswordHasherTests
{
    [Fact]
    public void CreatePasswordHash_Should_Generate_NonEmpty_Hash_And_Salt()
    {
        PasswordHasher.CreatePasswordHash("TestP@ssword123", out string hash, out string salt);

        hash.Should().NotBeNullOrWhiteSpace();
        salt.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void VerifyPassword_Should_Return_True_For_Correct_Password()
    {
        string password = "SecurePassword2026!";
        PasswordHasher.CreatePasswordHash(password, out string hash, out string salt);

        bool isValid = PasswordHasher.VerifyPassword(password, hash, salt);

        isValid.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_Should_Return_False_For_Incorrect_Password()
    {
        string password = "CorrectPassword";
        PasswordHasher.CreatePasswordHash(password, out string hash, out string salt);

        bool isValid = PasswordHasher.VerifyPassword("WrongPassword", hash, salt);

        isValid.Should().BeFalse();
    }
}
