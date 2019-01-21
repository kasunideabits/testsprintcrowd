using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SprintCrowdBackEnd.Models;
using System.Linq;
using SprintCrowdBackEnd.repositories;
using SprintCrowdBackEnd.Enums;
using SprintCrowdBackEnd.Models.GraphApi;
using SprintCrowdBackEnd.Logger;
using SprintCrowdBackEnd.Interfaces;
using SprintCrowdBackEnd.Persistence;

namespace SprintCrowdBackEnd.services
{
    public class UserService: IUserService
    {

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepo, IFacebookService fbService)
        {
            this._appSettings = appSettings.Value;
            this._userRepo = userRepo;
            this._fbService = fbService;
        }

        private IUserRepository _userRepo;
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private IFacebookService _fbService;
        private readonly AppSettings _appSettings;


        public User Authenticate(string fbAccessToken)
        {
            if(_fbService.ValidateAccessToken(fbAccessToken))
            {
                //Hurray! Valid access token
                //lets get details about /me => Graph Api
                FaceBoookUser userDetails = _fbService.GetFbUserDetails(fbAccessToken);
                User user = _userRepo.GetUser(userDetails.Email);
                if(user == null)
                {
                    //User is a new user, need to register the user
                    user = new User(){
                        Email = userDetails.Email,
                        FbUserId = userDetails.Id,
                        FirstName = userDetails.FirstName,
                        LastName = userDetails.LastName,
                        LastLoggedInTime = DateTime.UtcNow,
                        ProfilePicture = userDetails.ProfilePicture.PictureData.Url,
                        Token = fbAccessToken
                    };
                    this.RegisterUser(user);
                }
                UpdateLastLoggedInTIme(user);
                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, user.Email.ToString()),
                    }),
                    Expires = DateTime.UtcNow.AddDays(365), //TODO: Decide whether to expire or keep forever
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _appSettings.Issuer,
                    Audience = _appSettings.Audience
                };
                var jwt_token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(jwt_token);

                return user;

            }
            else
            {
                ScrowdLogger.Log($"Invalid user access token : {fbAccessToken}", LogType.Warning);
                return null;
            }

        }

        private void RegisterUser(User user)
        {
            User existingUser = _userRepo.GetUser(user.Email);
            ResponseObject response = new ResponseObject();
            if (existingUser != null)
            {
                _userRepo.AddUser(user);
            }
        }

        /*Keep a record of last logged in time to every user.
          May come in handy in future
         */
        private void UpdateLastLoggedInTIme(User user)
        {
            user.LastLoggedInTime = DateTime.UtcNow;
            _userRepo.UpdateUser(user);
        }
    }
}