using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Services.Tools
{
    public class TokenGenerator
    {
        private string Name;
        private string Role;
        private string? Token;
        private string Email;

        public TokenGenerator(string name,string role,string email)
        {
            this.Name = name;
            this.Role = role;
            this.Email = email;
            this.GenerateToken();
        }

        private void GenerateToken()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,this.Name),
                new Claim(ClaimTypes.Email,this.Email),
                new Claim(ClaimTypes.Role,this.Role),
                new Claim(ClaimTypes.Expired, DateTime.Now.AddSeconds(30).ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.Siqnature));
            var creed = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creed,
                expires: DateTime.Now.AddSeconds(40));
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            this.Token = jwt;
        }

        public string? GetToken()
        {
            return this.Token;
        }
    }
}
