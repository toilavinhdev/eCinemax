using System.IdentityModel.Tokens.Jwt;
using eCinemas.API.Shared.ValueObjects;

namespace eCinemas.API.Services;

public interface IBaseService
{
    UserClaimValue UserClaims();
}

public class BaseService(IHttpContextAccessor httpContextAccessor) : IBaseService
{
    public UserClaimValue UserClaims()
    {
        var stringValues = httpContextAccessor.HttpContext?.Request.Headers
            .FirstOrDefault(x => x.Key.Equals("Authorization")).Value;
        
        var accessToken = stringValues?.ToString().Split(" ").LastOrDefault();

        if (string.IsNullOrEmpty(accessToken)) return default!;
        
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        
        var decodedToken = jwtSecurityTokenHandler.ReadJwtToken(accessToken);
        
        return new UserClaimValue()
        {
            Id = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value 
                 ?? throw new NullReferenceException("Claim type 'id' is required"),
            FullName = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("fullName"))?.Value 
                       ?? throw new NullReferenceException("Claim type 'fullName' is required"),
            Email = decodedToken.Claims.FirstOrDefault(x => x.Type.Equals("email"))?.Value 
                    ?? throw new NullReferenceException("Claim type 'email' is required"),
        };
    }
}