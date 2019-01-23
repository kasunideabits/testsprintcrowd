namespace SprintCrowd.Backend.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using SprintCrowd.Backend.Models;
    using System.Linq;
    using SprintCrowd.Backend.Repositories;
    using SprintCrowd.Backend.Enums;
    using SprintCrowd.Backend.Models.GraphApi;
    using SprintCrowd.Backend.Logger;
    using SprintCrowd.Backend.Interfaces;
    using SprintCrowd.Backend.Persistence;

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
                FaceBookUser userDetails = _fbService.GetFbUserDetails(fbAccessToken);
                User user = _userRepo.GetUser(userDetails.Email);
                if(user == null)
                {
                    user = new User()
                    {
                        Email = userDetails.Email,
                        FbUserId = userDetails.Id,
                        FirstName = userDetails.FirstName,
                        LastName = userDetails.LastName,
                        LastLoggedInTime = DateTime.UtcNow,
                        ProfilePicture = new ProfilePicture() {
                            Url = userDetails.ProfilePicture.PictureData.Url,
                            Height = userDetails.ProfilePicture.PictureData.Height,
                            Width = userDetails.ProfilePicture.PictureData.Width
                        },
                        Token = fbAccessToken
                    };
                    this.RegisterUser(user);
                }
                this.UpdateLastLoggedInTime(user);
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
                SecurityToken jwtToken = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(jwtToken);

                return user;

            }
            else
            {
                SLogger.Log($"Invalid user access token : {fbAccessToken}", LogType.Warning);
                return null;
            }

        }

        private void RegisterUser(User user)
        {
            User existingUser = this._userRepo.GetUser(user.Email);
            ResponseObject response = new ResponseObject();
            if (existingUser != null)
            {
                this._userRepo.AddUser(user);
            }
        }

        /*Keep a record of last logged in time to every user.
          May come in handy in future
         */
        private void UpdateLastLoggedInTime(User user)
        {
            user.LastLoggedInTime = DateTime.UtcNow;
            this._userRepo.UpdateUser(user);
        }
    }
}