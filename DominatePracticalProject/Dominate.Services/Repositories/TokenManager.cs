using Dominate.Data.ViewModel;
using Dominate.Services.IRepositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dominate.Services.Repositories
{
    public static class TokenManager
    {

       public static string GenerateToken(TokenRequestViewModel? model)
        {
            try
            {


                var claims = new List<Claim>{
                    new Claim(JwtRegisteredClaimNames.Email, model.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("UserFullname",model.UserFullName),
                    new Claim("Email", model.UserName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model.JwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var expires = DateTime.UtcNow.AddHours(2);
                var token = new JwtSecurityToken(
                           claims: claims,
                           expires: expires,
                           signingCredentials: creds
                       );
                //return new AuthenticationResponseViewModel
                //{
                //    Token = new JwtSecurityTokenHandler().WriteToken(token),
                //    ExpiresIn = (int)expires.Subtract(DateTime.Now).TotalSeconds,
                //};
                  return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
