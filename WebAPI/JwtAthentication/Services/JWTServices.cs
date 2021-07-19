using Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.JwtAthentication.Services
{
    public static class JWTServices
    {
        public static string GenerateJsonWebToken(User userInfo, IConfiguration confing)
        {
            var securityKey = new
                SymmetricSecurityKey(Encoding.UTF8.GetBytes(confing["Jwt:Key"]));

            var credentials =
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim("ID", userInfo.ID.ToString()),
                new Claim("Name", userInfo.Name),
                new Claim("Age", userInfo.Age.ToString()),
                new Claim("Address", userInfo.Address),
                new Claim("Email", userInfo.Email)
            };

            var token = new JwtSecurityToken(confing["Jwt:Issuer"],
                                             confing["Jwt:Issuer"],
                                             claims,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
