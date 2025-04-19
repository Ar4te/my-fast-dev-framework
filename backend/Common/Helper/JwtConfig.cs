using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Common.Helper;

public class JwtConfig
{
    //密钥
    public string SecretKey { get; set; }
    //发布人
    public string Issuer { get; set; }
    //
    public string Audience { get; set; }
    //有效期
    public int Expired { get; set; }
    //生效时间
    public System.DateTime NotBefore => DateTime.Now;
    //过期时间
    public DateTime Expiration => NotBefore.AddMinutes(Expired);
    //密钥
    public SymmetricSecurityKey SymmetricSecurityKey => new(Encoding.UTF8.GetBytes(SecretKey));
    //
    public SigningCredentials SigningCredentials => new(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
    //
    public SecurityKey SecurityKey => SymmetricSecurityKey;
}