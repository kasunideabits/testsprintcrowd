namespace SprintCrowd.Backend.Repositories
{
    using System;
    using System.Collections.Generic;
    using SprintCrowd.Backend.Enums;
    using SprintCrowd.Backend.Models;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowd.Backend.Persistence;
    using SprintCrowd.Backend.Interfaces;

    public class UserRepository: IUserRepository
    {
        private readonly SprintCrowdDbContext _context;      
        public UserRepository(SprintCrowdDbContext context)
        {
            _context = context;
        }
        
        /*Get user by email*/
        public User GetUser(string email)
        {
            return _context.Users
                .Include(u => u.ProfilePicture)
                .FirstOrDefault(u => u.Email.Equals(email));
        }

        /*Add user to db */
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        //Id should be set obviously
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

    }
}