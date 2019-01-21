namespace SprintCrowdBackEnd.Repositories
{
    using System;
    using System.Collections.Generic;
    using SprintCrowdBackEnd.Enums;
    using SprintCrowdBackEnd.Models;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SprintCrowdBackEnd.Persistence;
    using SprintCrowdBackEnd.Interfaces;

    public class UserRepository : IUserRepository
    {
        public UserRepository(SprintCrowdDbContext context)
        {
            _context = context;
        }

        private readonly SprintCrowdDbContext _context;      


        /*Get user by email*/
        public User GetUser(string email)
        {
            return _context.Users
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