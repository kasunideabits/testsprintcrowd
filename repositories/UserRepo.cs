using System;
using System.Collections.Generic;
using SprintCrowdBackEnd.Enums;
using SprintCrowdBackEnd.interfaces;
using SprintCrowdBackEnd.Models;
using System.Linq;

namespace SprintCrowdBackEnd.repositories
{
    public class UserRepo: IUserRepo
    {
        private readonly ApplicationDbContext _context;      
        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        /*Get user by email*/
        public User GetUser(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email.Equals(email));
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();  
        }
    }
}