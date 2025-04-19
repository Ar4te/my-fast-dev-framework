using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Common.Helper;

public class JwtHelper
{
    private readonly JwtConfig _jwtConfig;

    public JwtHelper(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }

    public string GetToken(IList<Claim> claims)
    {
        var jwtSecurityToken = new JwtSecurityToken(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            claims,
            _jwtConfig.NotBefore,
            _jwtConfig.Expiration,
            _jwtConfig.SigningCredentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return token;
    }

    public IEnumerable<Claim> GetClaims(string token)
    {
        token = token.StartsWith("Bearer ") ? token.Replace("Bearer ", "") : token;
        var payload = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return payload.Claims;
    }
}
