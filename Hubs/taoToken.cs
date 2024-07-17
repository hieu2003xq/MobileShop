using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace ado.Hubs
{
    public class taoToken
    {
        public string GenerateToken(string userId)
        {
            var identity = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, userId),
            // Các claim khác tùy thuộc vào yêu cầu của bạn
        },
            DefaultAuthenticationTypes.ApplicationCookie);

           

            byte[] keyBytes = Encoding.UTF8.GetBytes("ADONET");
            Microsoft.IdentityModel.Tokens.SymmetricSecurityKey key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes);
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: userId,
                audience:userId,
                claims: identity.Claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            var accessToken = handler.WriteToken(token);

            return accessToken;
        }
    }
}