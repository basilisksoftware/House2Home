using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Helpers
{
    public class JwtHelper
    {
        /// <summary>
        /// Creates a JWT token for a user
        /// </summary>
        /// <param name="userFromDb">The user from the database</param>
        /// <param name="config">An injected instance of the config service</param>
        /// <returns>Returns a JWT token as a string</returns>
        public static string JwtToken(User userFromDb, IConfiguration config, bool isAdmin = false)
        {
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromDb.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromDb.Username),
                };

            // Add admin role if specified
            if (isAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            else
                claims.Add(new Claim(ClaimTypes.Role, "user"));

            var key = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        // public static bool IsAdmin(string bearerToken)
        // {

        //     // System.Console.WriteLine(bearerToken);

        //     // var strippedToken = bearerToken.Replace("Bearer ", "");
        //     // var tokenHandler = new JwtSecurityTokenHandler();
        //     // var decodedToken = tokenHandler.ReadJwtToken(strippedToken);

        //     // decodedToken


        //     return true;
        // }
    }




}