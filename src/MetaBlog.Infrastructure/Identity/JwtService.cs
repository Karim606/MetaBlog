using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using MetaBlog.Application.Common.Interfaces;

namespace MetaBlog.Infrastructure.Identity
{
    

    public class JwtService(IConfiguration Configuration) : IJwtService
    {
        public string GenerateToken(string Name, string Email, Guid Id, List<string> Roles)
        {
            var claims = new List<System.Security.Claims.Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,Id.ToString() ),
                new Claim(ClaimTypes.Email,Email),
                new Claim(ClaimTypes.Name,Name)
            };
            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Configuration["JwtSettings:Issuer"],
                audience: Configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(Configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public (string,DateTime) GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);
            var expiresAt = DateTime.UtcNow.AddDays(double.Parse(Configuration["JwtSettings:RefreshTokenExpiryDays"]));

           return (token,expiresAt);
        }
        public string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }
    }
}
