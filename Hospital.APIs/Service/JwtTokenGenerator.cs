using Hospital.APIs.Service.IService;
using Hospital.Models;
using Hospital.Utility;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital.APIs.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> Roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtValues.Key);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.Name),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
            };
            claims.AddRange(Roles.Select(x => new Claim(ClaimTypes.Role, x)));
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );
            var tokenDescriper = new SecurityTokenDescriptor()
            {
                Audience = JwtValues.Audience,
                Issuer = JwtValues.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = signingCredentials
            };
            var token = tokenHandler.CreateToken(tokenDescriper);
            return tokenHandler.WriteToken(token);
        }
    }
}
