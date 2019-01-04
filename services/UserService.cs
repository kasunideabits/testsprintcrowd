using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SprintCrowdBackEnd.interfaces;
using SprintCrowdBackEnd.Models;
using System.Linq;
using SprintCrowdBackEnd.repositories;
using SprintCrowdBackEnd.Enums;

namespace SprintCrowdBackEnd.services
{
    public class UserService: IUserService
    {
        private IUserRepo _userRepo;
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IUserRepo userRepo)
        {
            _appSettings = appSettings.Value;
            _userRepo = userRepo;
        }

        public User Authenticate(string email, string password)
        {
            var user = _userRepo.GetUser(email);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Email.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public ResponseObject RegisterUser(User user)
        {
            User existingUser = _userRepo.GetUser(user.Email);
            ResponseObject response = new ResponseObject();
            if (existingUser != null)
            {
                response.StatusId = (int)ResponseStatus.AlreadyRegistered;
                response.Data = "User already exists.";
                return response;
            }
            _userRepo.AddUser(user);
            response.StatusId = (int)ResponseStatus.AllOk;

            return response;
        }
    }
}