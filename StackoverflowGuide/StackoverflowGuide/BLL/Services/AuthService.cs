using AutoMapper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StackoverflowGuide.API.DTOs;
using StackoverflowGuide.BLL.Models;
using StackoverflowGuide.BLL.Models.Auth;
using StackoverflowGuide.BLL.Models.User;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StackoverflowGuide.BLL.Services
{
    public class AuthService: IAuthService
    {
        string jwtSecret;
        int jwtLifespan;
        UserManager<User> userManager;
        SignInManager<User> signInManager;
        

        public AuthService(string jwtSecret, int jwtLifespan, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.jwtSecret = jwtSecret;
            this.jwtLifespan = jwtLifespan;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<Guid> CreateNewUser(Registration registration)
        {
            if(registration.Password == registration.RepeatPassword)
            {
                var user = new User {UserName= registration.Email, Email=registration.Email };
                var result = await userManager.CreateAsync(user, registration.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return new Guid(user.Id);
                }
                throw new Exception("Cannot create User with these credentials!");
            }
            throw new Exception("Password and RepeatePassword dosen't match!");
        }

        public async Task<Guid> LogUserIn(Login login)
        {
            var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            if (result.Succeeded)
            {
                var user = userManager.Users.SingleOrDefault(r => r.Email == login.Email);
                return new Guid(user.Id);
            }
            throw new Exception("Bad Credentials!");
        }

        public AuthData GetAuthData(Guid id)
        {
            var expirationTime = DateTime.UtcNow.AddDays(jwtLifespan);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, id.ToString())
                }),
                Expires = expirationTime,
                // new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new AuthData
            {
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
                Id = id.ToString()
            };
        }

    }
}
