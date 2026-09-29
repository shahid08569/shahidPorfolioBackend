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
    public int Rating { get; set; } = 5;
    public string? Relationship { get; set; } = "Client";
    public bool IsApproved { get; set; }
    public bool IsActive { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime SubmittedAtUtc { get; set; }
}
