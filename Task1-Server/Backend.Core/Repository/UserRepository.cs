using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Interfaces;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Models;

namespace Backend.Core.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext context)
        {
            _dataContext = context;
        }

        public void DeleteUser(int id)
        {
            var user = _dataContext.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                _dataContext.Users.Remove(user);
            }
        }

        public User GetUserById(int id)
        {
            return _dataContext.Users.Where(p => p.UserId == id).FirstOrDefault();
        }

        public User GetUserByLogin(string login)
        {
            return _dataContext.Users
                .Where(u => u.Login.ToLower() == login.ToLower())
                .FirstOrDefault();
        }


        public ICollection<User> GetUsers()
        {
            return _dataContext.Users.OrderBy(p => p.UserId).ToList();
        }

        public bool Save()
        {
            return _dataContext.SaveChanges() >= 0;
        }

        public bool UserExists(int id)
        {
            return _dataContext.Users.Any(p => p.UserId == id);
        }
    }
}
