using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using my_shoppinglist_api.Helpers;
using my_shoppinglist_api.Helpers.enums;

namespace my_shoppinglist_api.Models.Authorization
{
  public class TokenResponse
  {
    public int Id { get; set; }
    public Role Role { get; set; }
    public string Nbf { get; set; }
    public string Exp { get; set; }

    public JWTToken ToToken(IConfiguration config)
    {
      var nbf = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
      var exp = new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds();
      var claims = new Claim[]
      {
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Role, Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, nbf.ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, exp.ToString()),
      };

      var token = new JwtSecurityToken(
          new JwtHeader(new SigningCredentials(
              new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("SecretKey"))),
                                       SecurityAlgorithms.HmacSha256)),
          new JwtPayload(claims));

      return new JWTToken() {
        JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
        RefreshToken = null,
        Nbf = nbf,
        Exp = exp
      };
    }

    public static TokenResponse FromToken(string token)
    {
      var jwttoken = new JwtSecurityToken(token);
      int id = Int32.Parse(jwttoken.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value);
      string role = jwttoken.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault()?.Value;
      string nbf = jwttoken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Nbf).FirstOrDefault()?.Value;
      string exp = jwttoken.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Exp).FirstOrDefault()?.Value;

      var roleEnum = Enum.TryParse(role,out Role roleType);
      return new TokenResponse { Id = id, Role = roleType, Nbf = nbf, Exp = exp };
    }
  }
}