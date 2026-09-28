using FluentAssertions;
using ShahidPortfolio.Application.Features.Contact.Commands.SubmitContactMessage;
using Xunit;

namespace ShahidPortfolio.Tests.UnitTests.Validators;

public class SubmitContactMessageValidatorTests
{
    private readonly SubmitContactMessageCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var command = new SubmitContactMessageCommand("", "valid@email.com", "Subject", "Valid message text here");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    public void Should_Have_Error_When_Email_Is_Invalid(string invalidEmail)
    {
        var command = new SubmitContactMessageCommand("John Doe", invalidEmail, "Subject", "Valid message text here");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Message_Is_Too_Short()
    {
        var command = new SubmitContactMessageCommand("John Doe", "john@example.com", "Subject", "Short");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Message");
    }

    [Fact]
    public void Should_Pass_When_All_Fields_Are_Valid()
    {
        var command = new SubmitContactMessageCommand("John Doe", "john@example.com", "Project Inquiry", "Hello, I want to discuss a full-stack engineering role.");
        var result = _validator.Validate(command);

        result.IsValid.Should().BeTrue();
    }
}
