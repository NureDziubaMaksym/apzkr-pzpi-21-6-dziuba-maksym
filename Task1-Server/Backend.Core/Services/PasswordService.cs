using Backend.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Services
{
    public class PasswordService
    {
        private readonly DataContext _context;
        private readonly string _fixedKey;

        public PasswordService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _fixedKey = configuration["JWT:Key"]; // Используем фиксированный ключ для хеширования
        }

        public void EncryptAll()
        {
            var users = _context.Users.ToList();

            foreach (var user in users)
            {
                user.Password = CreatePasswordHash(user.Password); // Перехешируем все пароли
                _context.Users.Update(user);
            }

            _context.SaveChanges();
        }

        public string CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_fixedKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerifyPasswordHash(string password, string storedHash)
        {
            var hash = CreatePasswordHash(password); // Используем единый метод для хеширования
            return hash == storedHash;
        }

        private bool IsPasswordHashed(string password)
        {
            return password.Length == 44; // Базовая длина хеша при использовании HMACSHA256
        }
    }

}
