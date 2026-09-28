using FluentAssertions;
using ShahidPortfolio.Application.Features.Auth.Commands.Login;

namespace ShahidPortfolio.Tests.UnitTests.Validators;

public class LoginCommandValidatorTests
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTests()
    {
        _validator = new LoginCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var command = new LoginCommand("", "SecretPassword123!");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid_Format()
    {
        var command = new LoginCommand("not-an-email", "SecretPassword123!");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var command = new LoginCommand("admin@portfolio.dev", "");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Password");
    }

    [Fact]
    public void Should_Pass_When_Command_Is_Valid()
    {
        var command = new LoginCommand("admin@portfolio.dev", "ValidPassword123!");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
