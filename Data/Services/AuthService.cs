using ComputerTracker.Data.DbModel;
using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerTracker.Data.Models
{
    public class AuthService
    {
        public bool Register(string login, string password, string fullname)
        {
            using (var context = new AppDbContext())
            {
                if (context.Users.Any(u => u.Login == login)) 
                    return false;

                var hashedPassword = HashPassword(password);

                var user = new User
                {
                    Fullname = fullname,
                    Login = login,
                    PasswordHash = hashedPassword
                };

                context.Users.Add(user);
                context.SaveChanges();
                return true;
            }
        }
    
        public User Login(string login, string password)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login);
                if (user != null && VerifyPassword(password, user.PasswordHash))
                {
                    return user;
                }
            }
            return null;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }

    }
}