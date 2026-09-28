namespace ShahidPortfolio.Application.Features.Testimonials.Queries.GetTestimonials;

public class TestimonialDto
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? LinkedInUrl { get; set; }
}
