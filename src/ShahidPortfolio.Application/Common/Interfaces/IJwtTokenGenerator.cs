using ShahidPortfolio.Domain.Entities;

namespace ShahidPortfolio.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(AdminUser user);
    string GenerateRefreshToken();
    DateTime GetAccessTokenExpiration();
}
