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

namespace MetaBlog.Infrastructure.Identity
{
    public interface IJwtService
    {
        string GenerateToken(string Name,string Email, Guid Id, List<string> Roles);
    }

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
    }
}
